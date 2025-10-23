using Godot;
using Perihelion.Types.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Perihelion.Mesh
{
    public class Network
    {
        public readonly HashSet<Server> Servers;

        private Network(HashSet<Server> servers)
        {
            Servers = servers;
        }


        public static Network FromGenerator(Random random, HashSet<CelestialObject> celestialObjects, Int64 numberOfServers)
        {
            HashSet<Server> servers = new HashSet<Server>();

            foreach (CelestialObject celestialObject in celestialObjects)
            {
                Location currentLocation = Location.GenerateLocation(random, celestialObject);

                List<Server> localServers = new List<Server>(); // A grouping of all the servers at the current location.
                List<Gateway> gateways = new List<Gateway>(); // A grouping of all the network gateways.
                for (Int32 i = 0; i < numberOfServers; i++)
                {
                    IPAddress address = IPAddressExtensions.GenerateIPv6Address(random);
                    // Decide whether to continue populating the network, or move onto the next.
                    if (localServers.Count() == 0 || random.Next(0, 5) > 0) // We should always have at least one device.
                    {
                        Terminal server = new Terminal(address, currentLocation);
                        localServers.Add(server);
                        servers.Add(server);
                    }
                    else
                    {
                        // Build the final gateway into the network.
                        Gateway gateway = new Gateway(address, currentLocation);
                        gateway.Connections.UnionWith(localServers);
                        foreach (Server server in localServers) // Add a two-way reference to the parent gateway.
                        {
                            server.Connections.Add(gateway);
                        }
                        gateways.Add(gateway);
                        servers.Add(gateway);

                        localServers.Clear();
                        currentLocation = Location.GenerateLocation(random, celestialObject);
                    }
                }

                // Add a connection for all the gateways to the other gateways.
                foreach (Server gateway in gateways)
                {
                    gateway.Connections.UnionWith(gateways);
                    gateway.Connections.Remove(gateway);
                }
            }

            return new Network(servers);
        }


        public static Network FromFile()
        {
            return new Network(new HashSet<Server>());
        }
    }
}
