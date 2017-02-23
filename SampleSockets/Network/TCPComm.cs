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
		public event EventHandler ReceivedPackage;

		private readonly CommBase tcpBase;
		private TcpClient tcpClient;
		private TcpListener tcpListener;

		public TCPComm(IPAddress server)
		{
			if (server.GetAddressBytes().SequenceEqual(IPAddress.Any.GetAddressBytes()))
				tcpBase = new TCPServer(portTCP);
			else
				tcpBase = new TCPClient(server, portTCP);

			tcpBase.ReceivedPackage += (sender, args) => ReceivePackage((CommandPackage)args);
		}

		public void Start()
		{
			tcpBase.Start();
			PrintUtils.PrintImportant("READY!!!");
			PrintUtils.PrintImportant(" - - - - - - - - - - - - - - - - - - - - - - - - - ");
		}

		public void SendPackage(CommandPackage package)
		{
			tcpBase.SendPackage(package);
		}

		public void ReceivePackage(CommandPackage package)
		{
			ReceivedPackage?.Invoke(this, package);
		}

		
	}
}