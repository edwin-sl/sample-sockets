using System;

namespace SampleSockets.Network
{
	internal interface CommBase
	{
		event EventHandler ReceivedPackage;

		void SendPackage(CommandPackage package);
		void ReceivePackage(CommandPackage package);
		void Start();
	}
}