using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SampleSockets.Network.Utils;

namespace SampleSockets.Network.Server
{
	class TCPServer : CommBase
	{
		private List<TcpClient> clients;
		private TcpClient tcpClient;
		private readonly TcpListener tcpListener;

		public TCPServer(int port)
		{
			PrintUtils.PrintImportant("Init Server");
			tcpListener = new TcpListener(IPAddress.Any, port);
		}

		public void Start()
		{
			clients = new List<TcpClient>();
			tcpListener.Start();
			var task = Task.Run(() =>
			{
				while (true)
				{
					// Perform a blocking call to accept requests.
					// You could also user server.AcceptSocket() here.
					var client = tcpListener.AcceptTcpClient();
					PrintUtils.PrintWarning(string.Format("Client Connected ({0})", client.Client.RemoteEndPoint));
					clients.Add(client);

					// Get a stream object for reading and writing

					ThreadPool.QueueUserWorkItem(ReceivingData, client);
				}
			});
		}

		public void SendPackage(CommandPackage package)
		{
			var bytes = new List<byte>(package.data);
			bytes.Insert(0, (byte) package.command);

			clients.RemoveAll(client => !client.Connected);
			NetworkStream stream;
			clients.ForEach(client =>
			{
				if (client.Client.RemoteEndPoint != package.client)
				{
					stream = client.GetStream();
					stream.Write(bytes.ToArray(), 0, bytes.Count);
					stream.Flush();
					stream = null;
				}
			});
		}

		public void ReceivePackage(CommandPackage package)
		{
			switch (package.command)
			{
				case ServerCommands.BROADCAST:
					break;
				case ServerCommands.MESSAGE:
					PrintUtils.PrintNormal(Encoding.ASCII.GetString(package.data, 0, package.data.Length));
					package.command = ServerCommands.BROADCAST;
					SendPackage(package);
					break;
			}
		}

		public void ReceivingData(object state)
		{
			var client = (TcpClient) state;
			int i;
			// Buffer for reading data
			var bytes = new byte[256];
			string data = null;
			// Loop to receive all the data sent by the client.

			while (client.Connected)
				try
				{
					Array.Clear(bytes, 0, bytes.Length);
					var stream = client.GetStream();
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

					PrintUtils.PrintImportant(string.Format("Client Disconnected ({0})", client.Client.RemoteEndPoint));
				}
		}
	}
}