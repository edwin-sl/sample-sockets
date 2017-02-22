namespace SampleSockets.Network
{
	internal interface CommBase
	{
		void SendPackage(ServerCommands command, byte[] data);
		void ReceivePackage(ServerCommands command, byte[] data);
		void Start();
	}
}