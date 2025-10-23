using System.Net;

namespace Perihelion.Mesh
{
    public class Terminal : Server
    {
        public Terminal(IPAddress address, Location position) : base(address, position)
        {
        }
    }
}
