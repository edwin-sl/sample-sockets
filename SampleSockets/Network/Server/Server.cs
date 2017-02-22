using System;
using System.Net;
using SampleSockets.Network.Utils;

namespace SampleSockets.Network.Server
{
	internal class Server : NetworkNode
	{
		private UDPServer udpServer;

		public IPEndPoint ConfigServer()
		{
			IPEndPoint serverEndPoint;

			if (ServerValidation(out serverEndPoint))
			{
				PrintUtils.PrintImportant("YES SERVER!!!");
				return serverEndPoint;
			}

			PrintUtils.PrintImportant("NO SERVER!!!");
			udpServer = new UDPServer("server_udp", portUDP);
			udpServer.Start();
			return udpServer.udpClient.Client.LocalEndPoint as IPEndPoint;
		}

		public bool ServerValidation(out IPEndPoint serverEndPoint)
		{
			serverEndPoint = UDPServer.SearchServer(portUDP);
			return serverEndPoint != null;
		}
	}
}