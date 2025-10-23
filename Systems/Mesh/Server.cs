using Perihelion.Types.AStar;
using Perihelion.Types.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Perihelion.Mesh
{
    /// <summary> A single accessible node within the entire network. </summary>
    public abstract class Server : IGraphable, IEquatable<Server>
    {
        /// <summary> The server's unique address upon the network. </summary>
        public readonly IPAddress Address;

        /// <summary> The sever's location in physical space. </summary>
        public readonly Location Position;

        /// <summary> The other servers this one has the ability to connect to. </summary>
        public readonly HashSet<Server> Connections;


        /// <summary> A single accessible node within the entire network. </summary>
        /// <param name="address"> The server's unique address upon the network. </param>
        /// <param name="position"> The sever's location in physical space. </param>
        public Server(IPAddress address, Location position)
        {
            Address = address;
            Position = position;
            Connections = new HashSet<Server>();
        }


        /// <inheritdoc/>
        public IGraphable[] GetNeighbours() => Connections.ToArray();


        /// <inheritdoc/>
        public Single CalculateHeuristic(IGraphable other)
        {
            Single result = 1f;
            if (other is Server server)
            {
                Location otherPosition = server.Position;
                if (Position.CelestialObject == otherPosition.CelestialObject) // If we're on the same planet...
                {
                    result = (Single)MathExtensions.CalculateHaversineDistance(Position.Coordinates, server.Position.Coordinates);
                }
                else
                {
                    // TODO - Have some calculation for planet to planet communication.
                }
            }
            return result;
        }


        /// <inheritdoc/>
        public Boolean Equals(Server? other) => Address == other?.Address;


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Address);
    }
}
