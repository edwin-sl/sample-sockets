using System;
using System.Net;
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
			// Manual connection with argument OR automatic configuration
			var serverEndPoint = args.Length > 0 ?
				new IPEndPoint(IPAddress.Parse(args[0]), NetworkNode.portTCP) :
				new Server().ConfigServer();

			// Config communication				
			var chat = new TCPComm(serverEndPoint.Address);
			chat.Start();

			// Event to receive messages
			chat.ReceivedPackage += ReceivedPackage;
			// Input to send messages
			while (true)
			{
				var msg = Console.ReadLine();
				chat.SendPackage(new CommandPackage(ServerCommands.MESSAGE, Encoding.ASCII.GetBytes(msg), null));
			}
		}

		private static void ReceivedPackage(object sender, EventArgs e)
		{
			CommandPackage package = (CommandPackage) e;
			Console.WriteLine(Encoding.ASCII.GetString(package.data));
		}
	}
}