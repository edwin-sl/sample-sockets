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

		public void SendPackage(ServerCommands command, byte[] data)
		{
			var bytes = new List<byte>(data);
			bytes.Insert(0, (byte) command);

			var stream = tcpClient.GetStream();
			stream.Write(bytes.ToArray(), 0, bytes.Count);
			stream.Flush();
			stream = null;
		}

		public void ReceivePackage(ServerCommands command, byte[] data)
		{
			switch (command)
			{
				case ServerCommands.BROADCAST:
					PrintUtils.PrintNormal(Encoding.ASCII.GetString(data, 0, data.Length));
					break;
				case ServerCommands.MESSAGE:
					PrintUtils.PrintNormal(Encoding.ASCII.GetString(data, 0, data.Length));
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

					//					{
					// Translate data bytes to a ASCII string.
					ReceivePackage((ServerCommands) bytes[0], bytes.Skip(1).Take(length).ToArray());
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