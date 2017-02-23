namespace SampleSockets.Network
{
	internal interface CommBase
	{
		void SendPackage(CommandPackage package);
		void ReceivePackage(CommandPackage package);
		void Start();
	}
}