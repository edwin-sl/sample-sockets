using System;
using System.Net;

namespace SampleSockets.Network
{
	public enum ServerCommands
	{
		HELLO,
		MESSAGE,
		BROADCAST,
		RESTART
	}

	public class CommandPackage : EventArgs
	{
		public ServerCommands command { get; set; }
		public byte[] data { get; set; }
		public EndPoint client { get; set; }

		public CommandPackage(ServerCommands command, byte[] data, EndPoint client)
		{
			this.command = command;
			this.data = data;
			this.client = client;
		}
	}
}