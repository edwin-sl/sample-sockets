using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SampleSockets.Network.Server
{
	internal class UDPServer
	{
		private bool started;
		public UdpClient udpClient;


		public UDPServer(string name, int port)
		{
			udpClient = new UdpClient(port);
		}

		public void Start()
		{
			started = true;
			var task = Task.Run(() =>
			{
				while (started)
				{
					//IPEndPoint object will allow us to read datagrams sent from any source.
					var RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

					// Blocks until a message returns on this socket from a remote host.
					var receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

					switch ((ServerCommands) receiveBytes[0])
					{
						case ServerCommands.HELLO:
							byte[] data = {(byte) ServerCommands.HELLO, 5};
							udpClient.Send(data, data.Length, RemoteIpEndPoint);
							break;
						default:
							break;
					}
				}
			});
//			task.Wait();
		}

		public static IPEndPoint SearchServer(int port)
		{
			var serverEndPoint = new IPEndPoint(IPAddress.Broadcast, port);
			byte[] toBytes = {(byte) ServerCommands.HELLO};
			var udpClient = new UdpClient(250); // Local Testing

			udpClient.EnableBroadcast = true;
			var r = udpClient.Send(toBytes, toBytes.Length, serverEndPoint);
			// Blocks until a message returns on this socket from a remote host.

			udpClient.Client.ReceiveTimeout = 5000;

			try
			{
				var response = udpClient.Receive(ref serverEndPoint);
				if (response[0] != (byte) ServerCommands.HELLO)
					serverEndPoint = null;
			}
			catch (Exception e)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(e);
				Console.ResetColor();
				serverEndPoint = null;
			}
			udpClient.Close();
			return serverEndPoint;
		}
	}
}