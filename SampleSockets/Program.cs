using System;
using System.Text;
using SampleSockets.Network;
using SampleSockets.Network.Server;

namespace SampleSockets
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// Get server info
			var serverEndPoint = new Server().ConfigServer();
			// Config this machine
			var chat = new TCPComm(serverEndPoint.Address);

			chat.Start();

			while (true)
			{
				var msg = Console.ReadLine();
				chat.SendPackage(new CommandPackage(ServerCommands.MESSAGE, Encoding.ASCII.GetBytes(msg), null));
			}
		}
	}
}