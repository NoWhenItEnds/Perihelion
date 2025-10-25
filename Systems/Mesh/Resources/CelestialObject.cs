using System;
using Godot;
using Perihelion.Types.Extensions;

namespace Perihelion.Mesh.Resources
{
    /// <summary> A stellar object representing a location within space. </summary>
    [GlobalClass]
    public partial class CelestialObject : Resource, IEquatable<CelestialObject>
    {
        /// <summary> The unique identifier or name of the object. </summary>
        [ExportGroup("Properties")]
        [Export] public String Id { get; private set; }

        /// <summary> The mean distance of the orbit in AU. 1 AU is the distance from the Sun to the Earth. </summary>
        [Export] public Double SemiMajorAxis { get; private set; }

        /// <summary> The eccentricity of the orbit. Between 0.0 - 1.0, with 0.0 being perfectly circular. </summary>
        [Export] public Double Eccentricity { get; private set; }

        /// <summary> The angle of the orbit's tilt relative to the plane of the ecliptic. </summary>
        [Export] public Double Inclination { get; private set; }

        /// <summary> The longitude of the ascending node in degrees. </summary>
        /// <remarks> This is the position in the orbit where the elliptical path of the planet passes through the plane of the ecliptic, from below the plane to above the plane. </remarks>
        [Export] public Double Ascending { get; private set; }

        /// <summary> The argument of perihelion in degrees. This is the position in the orbit where the planet is closest to the Sun. </summary>
        [Export] public Double Perihelion { get; private set; }

        /// <summary> How long, in days, the object takes to complete an orbit. </summary>
        [Export] public Double OrbitalPeriod { get; private set; }

        /// <summary> The radius of the object in km. </summary>
        [Export] public Double Radius { get; private set; }

        /// <summary> The object's axial tilt from its orbital plane in degrees. </summary>
        [Export] public Double Obliquity { get; private set; }

        /// <summary> How many earth days it takes for the object to complete a single rotation. </summary>
        [Export] public Double RotationPeriod { get; private set;}


        /// <summary> The material to apply to the object's mesh. </summary>
        [ExportGroup("Resources")]
        [Export] public Material MeshMaterial { get; private set; }


        /// <summary> A stellar object representing a location within space. </summary>
        public CelestialObject() {}


        /// <summary> Calculate the current position of the object in 3D space given a time. </summary>
        /// <param name="time"> The time to calculate the position at. </param>
        /// <returns> The current position of the object in 3D space. </returns>
        public Vector3 CalculateCartesianPosition(DateTimeOffset time)
        {
            DateTimeOffset j2000 = new DateTimeOffset(2000, 1, 1, 12, 0, 0, TimeSpan.Zero);
            Double julianDays = (time - j2000).TotalDays; // Calculate the number of days since 2000.

            Double meanAnomaly = (2 * Math.PI / OrbitalPeriod) * julianDays;

            // Solve for eccentric anomaly using Newton-Raphson method.
            Double eccentricAnomaly = meanAnomaly;
            for (Int32 i = 0; i < 100; i++)
            {
                Double f = eccentricAnomaly - Eccentricity * Mathf.Sin(eccentricAnomaly) - meanAnomaly;
                Double df = 1.0 - Eccentricity * Mathf.Cos(eccentricAnomaly);
                eccentricAnomaly -= f / df;
                if (Mathf.Abs(f) < 1e-10) { break; }
            }

            // Calculate true anomaly
            Double sqrtTerm = Math.Sqrt((1 + Eccentricity) / (1 - Eccentricity));
            Double trueAnomaly = 2.0 * Mathf.Atan2(sqrtTerm * Mathf.Sin(eccentricAnomaly / 2.0), Mathf.Cos(eccentricAnomaly / 2.0));

            // Calculate distance from the Sun
            Double r = SemiMajorAxis * (1 - Eccentricity * Eccentricity) / (1 + Eccentricity * Mathf.Cos(trueAnomaly));

            // Some values need to be in radians.
            Double inclinationRad = Mathf.DegToRad(Inclination);
            Double ascendingRad = Mathf.DegToRad(Ascending);
            Double perihelionRad = Mathf.DegToRad(Perihelion);
            Double arg = perihelionRad + trueAnomaly;

            Double x = r * (Mathf.Cos(ascendingRad) * Mathf.Cos(arg) - Mathf.Sin(ascendingRad) * Mathf.Sin(arg) * Mathf.Cos(inclinationRad));
            Double y = r * (Mathf.Sin(ascendingRad) * Mathf.Cos(arg) + Mathf.Cos(ascendingRad) * Mathf.Sin(arg) * Mathf.Cos(inclinationRad));
            Double z = r * Mathf.Sin(arg) * Mathf.Sin(inclinationRad);

            return new Vector3((Single)x, (Single)y, (Single)z);
        }


        public Vector3 CalculateRotation(DateTimeOffset time)
        {
            DateTimeOffset j2000 = new DateTimeOffset(2000, 1, 1, 12, 0, 0, TimeSpan.Zero);
            Double julianDays = (time - j2000).TotalDays; // Calculate the number of days since 2000.

            Double fraction = julianDays / RotationPeriod;
            Double spinAngle = MathExtensions.WrapValue(360.0 * fraction, 360.0);
            return new Vector3((Single)Obliquity, 0f, (Single)spinAngle);
        }


        /// <inheritdoc/>
        public Boolean Equals(CelestialObject? other) => Id.ToLower() == other?.Id.ToLower();


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Id.ToLower());
    }
}
