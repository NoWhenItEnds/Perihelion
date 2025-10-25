using Godot;
using System;

namespace Perihelion.Types.Extensions
{
    /// <summary> Helpful extensions to the math namespace. </summary>
    public static class MathExtensions
    {
        /// <summary> The radius of Earth in km. </summary>
        public static Double EARTH_RADIUS => 6378.0;


        /// <summary> Clamps an value and ensures it wraps. </summary>
        /// <param name="value"> The value to wrap. </param>
        /// <param name="max"> The maximum value before it wraps around from zero. </param>
        /// <returns> The clamped value. </returns>
        public static Double WrapValue(Double value, Double max)
        {
            Double remainder = value % max;
            if (remainder < 0)
            {
                remainder += max;
            }
            return remainder;
        }


        /// <summary> Convert geographical coordinates to cartesian positions upon a unit sphere. </summary>
        /// <param name="latitudeDeg"> The north-south coordinate.</param>
        /// <param name="longitudeDeg"> The east-west coordinate. </param>
        /// <param name="radius"> The radius of the unit sphere. </param>
        /// <returns> The cartesian position in 3D space. </returns>
        public static Vector3 GeographicalToCartesian(Single latitudeDeg, Single longitudeDeg, Single radius = 1f)
        {
            Single latitudeRad = Mathf.DegToRad(latitudeDeg);
            Single longitudeRad = Mathf.DegToRad(longitudeDeg);

            Double x = radius * Math.Cos(latitudeRad) * Math.Cos(longitudeRad);
            Double y = radius * Math.Sin(latitudeRad);
            Double z = radius * Math.Cos(latitudeRad) * Math.Sin(longitudeRad);

            return new Vector3((Single)x, (Single)y, (Single)z);
        }


        /// <summary> Find the distance, in km, between to geographical positions. </summary>
        /// <param name="coordinate0"> The starting coordinates. </param>
        /// <param name="coordinate1"> The ending coordinates. </param>
        /// <returns> The distance between the two points in km. </returns>
        public static Double CalculateHaversineDistance(GeographicalCoordinate coordinate0, GeographicalCoordinate coordinate1)
        {
            Double dLat = Mathf.DegToRad(coordinate1.Latitude - coordinate0.Latitude);
            Double dLon = Mathf.DegToRad(coordinate1.Longitude - coordinate0.Longitude);
            Double dLat0 = Mathf.DegToRad(coordinate0.Latitude);
            Double dLat1 = Mathf.DegToRad(coordinate1.Latitude);

            Double a = Math.Pow(Math.Sin(dLat / 2), 2) +
            Math.Pow(Math.Sin(dLon / 2), 2) *
            Math.Cos(dLat0) * Math.Cos(dLat1);
            Double c = 2 * Math.Asin(Math.Sqrt(a));
            return EARTH_RADIUS * c;
        }


        /// <summary> Convert km to astronomical units. </summary>
        /// <param name="km"> The value to convert. </param>
        /// <returns> The converted value in astronomical units. </returns>
        public static Double KmToAu(Double km) => km * 6.68459e-9;


        /// <summary> Convert astronomical units to km. </summary>
        /// <param name="au"> The value to convert. </param>
        /// <returns> The converted value in km. </returns>
        public static Double AuToKm(Double au) => au / 6.68459e-9;
    }
}
