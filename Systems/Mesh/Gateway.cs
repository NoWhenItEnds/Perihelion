using System.Net;

namespace Perihelion.Mesh
{
    public class Gateway : Server
    {
        public Gateway(IPAddress address, Location position) : base(address, position)
        {
        }
    }
}
