using Godot;
using System;

namespace Perihelion.Types.Extensions
{
    /// <summary> Helpful extensions to the math namespace. </summary>
    public static class MathExtensions
    {
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
    }
}
