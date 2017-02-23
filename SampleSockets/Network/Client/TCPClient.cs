using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SampleSockets.Network.Exceptions;
using SampleSockets.Network.Utils;

namespace SampleSockets.Network.Client
{
	internal class TCPClient : CommBase
	{
		public event EventHandler ReceivedPackage;
		private readonly TcpClient tcpClient;

		public TCPClient(IPAddress server, int port)
		{
			PrintUtils.PrintImportant("Init Client -> " + server);
			var serverEndPoint = new IPEndPoint(server, port);
			tcpClient = new TcpClient(server.ToString(), port); //(serverEndPoint);
		}

		public void Start()
		{
			var task = Task.Run(() => ReceivingData());
		}

		public void SendPackage(CommandPackage package)
		{
			var bytes = new List<byte>(package.data);
			bytes.Insert(0, (byte) package.command);

			var stream = tcpClient.GetStream();
			stream.Write(bytes.ToArray(), 0, bytes.Count);
			stream.Flush();
			stream = null;
		}

		public void ReceivePackage(CommandPackage package)
		{
			switch (package.command)
			{
				case ServerCommands.BROADCAST:
					ReceivedPackage?.Invoke(this, package);
//					PrintUtils.PrintNormal(Encoding.ASCII.GetString(package.data, 0, package.data.Length));
					break;
				case ServerCommands.MESSAGE:
					ReceivedPackage?.Invoke(this, package);
//					PrintUtils.PrintNormal(Encoding.ASCII.GetString(package.data, 0, package.data.Length));
					break;
			}
		}

		public void ReceivingData()
		{
			// Buffer for reading data
			var bytes = new byte[256];
			string data = null;
			// Loop to receive all the data sent by the client.

			while (tcpClient.Connected)
				try
				{
					Array.Clear(bytes, 0, bytes.Length);
					var stream = tcpClient.GetStream();
					var length = stream.Read(bytes, 0, bytes.Length);
					
					CommandPackage package = new CommandPackage(
						(ServerCommands)bytes[0], 
						bytes.Skip(1).Take(length).ToArray(), 
						tcpClient.Client.RemoteEndPoint);
					// Translate data bytes to a ASCII string.
					ReceivePackage(package);
					stream.Flush();
				}
				catch (Exception e)
				{
//					Console.BackgroundColor = ConsoleColor.Red;
//					Console.ForegroundColor = ConsoleColor.White;
//					Console.WriteLine(e.ToString());
//					Console.ResetColor();

					PrintUtils.PrintWarning(string.Format("Server Disconnected ({0})", tcpClient.Client.RemoteEndPoint));
					throw new ServerLostEsception();
				}
		}
	}
}