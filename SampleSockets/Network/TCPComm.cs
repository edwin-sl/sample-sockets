using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using SampleSockets.Network.Client;
using SampleSockets.Network.Server;
using SampleSockets.Network.Utils;

namespace SampleSockets.Network
{
	internal class TCPComm : NetworkNode, CommBase
	{
		private readonly CommBase tcpBase;
		private TcpClient tcpClient;
		private TcpListener tcpListener;

		public TCPComm(IPAddress server)
		{
			if (server.GetAddressBytes().SequenceEqual(IPAddress.Any.GetAddressBytes()))
				tcpBase = new TCPServer(portTCP);
			else
				tcpBase = new TCPClient(server, portTCP);
		}

		public void Start()
		{
			tcpBase.Start();
			PrintUtils.PrintImportant("READY!!!");
			PrintUtils.PrintImportant(" - - - - - - - - - - - - - - - - - - - - - - - - - ");
		}

		public void SendPackage(ServerCommands command, byte[] data)
		{
			tcpBase.SendPackage(command, data);
		}

		public void ReceivePackage(ServerCommands command, byte[] data)
		{
			throw new NotImplementedException();
		}
	}
}