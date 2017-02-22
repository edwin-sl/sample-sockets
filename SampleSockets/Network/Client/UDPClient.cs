using System.Net.Sockets;

namespace SampleSockets.Network.Client
{
	internal class UDPClient
	{
		public UdpClient udpClient;


		public UDPClient(string name, int port)
		{
			udpClient = new UdpClient(port);
		}
	}
}