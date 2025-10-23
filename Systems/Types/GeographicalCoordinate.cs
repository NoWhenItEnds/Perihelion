using System;

namespace Perihelion.Types
{
    /// <summary> A data structure representing a geographical position on a sphere. </summary>
    public record GeographicalCoordinate
    {
        /// <summary> The position north-to-south. </summary>
        public readonly Single Latitude;

        /// <summary> The position east-to-west. </summary>
        public readonly Single Longitude;


        /// <summary> A data structure representing a geographical position on a sphere. </summary>
        /// <param name="latitude"> The position north-to-south. </param>
        /// <param name="longitude"> The position east-to-west.</param>
        public GeographicalCoordinate(Single latitude, Single longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }


        /// <inheritdoc/>
        public override string ToString() => $"({Latitude:F5}, {Longitude:F5})";
    }
}
