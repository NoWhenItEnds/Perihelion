using Perihelion.Types;
using System;

namespace Perihelion.Mesh
{
    /// <summary> A data structure representing a location with celestial space. </summary>
    public record Location
    {
        /// <summary> The physical stellar object the location is upon. </summary>
        public CelestialObject CelestialObject { get; private set; }

        /// <summary> The geographical position of the location. </summary>
        public GeographicalCoordinate Coordinates { get; private set; }


        /// <summary> A data structure representing a location with celestial space. </summary>
        /// <param name="celestialObject"> The physical stellar object the location is upon. </param>
        /// <param name="coordinates"> The geographical position of the location. </param>
        public Location(CelestialObject celestialObject, GeographicalCoordinate coordinates)
        {
            CelestialObject = celestialObject;
            Coordinates = coordinates;
        }


        /// <summary> Sets / updates the location's stellar object. </summary>
        /// <param name="celestialObject"> The physical stellar object the location is upon. </param>
        public void SetCelestialObject(CelestialObject celestialObject)
        {
            CelestialObject = celestialObject;
        }


        /// <summary> Sets / updates the location's geographical position. </summary>
        /// <param name="coordinates"> The geographical position of the location. </param>
        public void SetCoordinates(GeographicalCoordinate coordinates)
        {
            Coordinates = coordinates;
        }


        /// <summary> Generate a random location on the given celestial object. </summary>
        /// <param name="random"> A reference to the random class to use. </param>
        /// <param name="celestialObject"> The celestial object the location is on. </param>
        /// <returns> A formed location. </returns>
        /// <remarks> Ideally this pulls information from the celestial object to make a smart decision where to place the location. </remarks>
        public static Location GenerateLocation(Random random, CelestialObject celestialObject)
        {
            // TODO - Make smarter. Have the location pulled from interesting locations on the object.
            return new Location(celestialObject, new GeographicalCoordinate(random.NextSingle() * 180f - 90f, random.NextSingle() * 360f - 180f));
        }
    }
}
