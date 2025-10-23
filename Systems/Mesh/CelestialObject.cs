using System;

namespace Perihelion.Mesh
{
    /// <summary> A stellar object representing a location within space. </summary>
    /// <remarks> https://github.com/mchrbn/unity-planetarium/blob/master/Assets/Scripts/CelestialCoordinates.cs </remarks>
    public class CelestialObject : IEquatable<CelestialObject>
    {
        /// <summary> The unique identifier or name of the object. </summary>
        public String Id { get; private set; }

        /// <summary> The mean distance of the orbit in AU. 1 AU is the distance from the Sun to the Earth. </summary>
        public Double SemiMajorAxis { get; private set; }

        /// <summary> The eccentricity of the orbit. Between 0.0 - 1.0, with 1.0 being perfectly circular. </summary>
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


        /*
        public (Single X, Single Y, Single Z) CalculateCartesianPosition(DateTimeOffset time)
        {
            DateTimeOffset j2000 = new DateTimeOffset(2000, 1, 1, 12, 0, 0, TimeSpan.Zero);
            Double julianDays = (time - j2000).TotalDays; // Calculate the number of days since 2000.
            Double julianCenturies = julianDays / 36525.0f; // Calculate the number of centuries since 2000.

            // Calculate mean anomaly.
        }*/


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks> https://www.princeton.edu/~willman/planetary_systems/Sol/ https://www.heavens-above.com/mars.aspx </remarks>
        public static CelestialObject BuildMars()
        {
            return new CelestialObject("Mars", 1.52371, 0.09339, 105.9972, 49.57854, 336.04084, 686.497767);
        }


        /// <inheritdoc/>
        public Boolean Equals(CelestialObject? other) => Id == other?.Id;


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Id);
    }
}
