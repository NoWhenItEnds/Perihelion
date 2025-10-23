using System;
using Godot;

namespace Perihelion.Mesh
{
    /// <summary> A stellar object representing a location within space. </summary>
    public class CelestialObject : IEquatable<CelestialObject>
    {
        /// <summary> The unique identifier or name of the object. </summary>
        public String Id { get; private set; }

        /// <summary> The mean distance of the orbit in AU. 1 AU is the distance from the Sun to the Earth. </summary>
        public Double SemiMajorAxis { get; private set; }

        /// <summary> The eccentricity of the orbit. Between 0.0 - 1.0, with 0.0 being perfectly circular. </summary>
        public Double Eccentricity { get; private set; }

        /// <summary> The angle of the orbit's tilt relative to the plane of the ecliptic. </summary>
        public Double Inclination { get; private set; }

        /// <summary> The longitude of the ascending node in degrees. </summary>
        /// <remarks> This is the position in the orbit where the elliptical path of the planet passes through the plane of the ecliptic, from below the plane to above the plane. </remarks>
        public Double Ascending { get; private set; }

        /// <summary> The argument of perihelion in degrees. This is the position in the orbit where the planet is closest to the Sun. </summary>
        public Double Perihelion { get; private set; }

        /// <summary> How long, in days, the object takes to complete an orbit. </summary>
        public Double OrbitalPeriod { get; private set; }


        /// <summary> A stellar object representing a location within space. </summary>
        /// <param name="id">The unique identifier or name of the object.</param>
        /// <param name="semiMajor">The mean distance of the orbit in AU.</param>
        /// <param name="eccentricity">The eccentricity of the orbit.</param>
        /// <param name="inclination">The angle of the orbit's tilt relative to the plane of the ecliptic. </param>
        /// <param name="ascending">The longitude of the ascending node in degrees.</param>
        /// <param name="perihelion">The argument of perihelion in degrees.</param>
        /// <param name="period">How long, in days, the object takes to complete an orbit.</param>
        public CelestialObject(String id, Double semiMajor, Double eccentricity, Double inclination, Double ascending, Double perihelion, Double period)
        {
            Id = id;
            SemiMajorAxis = semiMajor;
            Eccentricity = eccentricity;
            Inclination = inclination;
            Ascending = ascending;
            Perihelion = perihelion;
            OrbitalPeriod = period;
        }


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


        /// <inheritdoc/>
        public Boolean Equals(CelestialObject? other) => Id.ToUpper() == other?.Id.ToUpper();


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Id.ToUpper());


        /// <summary> Construct the planet Mercury. </summary>
        public static CelestialObject MERCURY => new CelestialObject("Mercury", 0.3870993, 0.20564, 7.0, 75.05, 48.33, 87.9090455);

        /// <summary> Construct the planet Venus. </summary>
        public static CelestialObject VENUS => new CelestialObject("Venus", 0.723336, 0.00678, 3.39, 76.64, 75.47, 224.5469999);

        /// <summary> Construct the planet Earth. </summary>
        public static CelestialObject EARTH => new CelestialObject("Earth", 1.0, 0.01671, 0, 114.21, 102.94, 365.006351);

        /// <summary> Construct the planet Mars. </summary>
        public static CelestialObject MARS => new CelestialObject("Mars", 1.52371, 0.09339, 1.85, 49.56, 336.04, 686.497767);

        /// <summary> Construct the planet Jupiter. </summary>
        public static CelestialObject JUPITER => new CelestialObject("Jupiter", 5.2029, 0.0484, 1.3, 100.49, 273.66, 4329.854475);

        /// <summary> Construct the planet Saturn. </summary>
        public static CelestialObject SATURN => new CelestialObject("Saturn", 9.537, 0.0539, 2.49, 113.25, 339.42, 10748.33677);

        /// <summary> Construct the planet Uranus. </summary>
        public static CelestialObject URANUS => new CelestialObject("Uranus", 19.189, 0.04726, 0.77, 73.14, 312.01, 30666.14879);

        /// <summary> Construct the planet Neptune. </summary>
        public static CelestialObject NEPTUNE => new CelestialObject("Neptune", 30.0699, 0.00859, 1.77, 131.78, 280.87, 60148.8318);

        /// <summary> Construct the planet Pluto. </summary>
        public static CelestialObject PLUTO => new CelestialObject("Pluto", 39.4821, 0.24883, 17.14, 110.3, 113.75, 90527.592);
    }
}
