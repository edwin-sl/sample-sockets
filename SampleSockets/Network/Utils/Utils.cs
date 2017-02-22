using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSockets.Network.Utils
{
	class ConnUtils
	{
		public static double BinaryExponentialBackoff(int intent, int truncate = int.MaxValue)
		{
			int power = intent < truncate ? intent : truncate; 
			double time = Math.Pow(2, power);
			return time - 1;
		} 
	}

	class PrintUtils
	{
		public static void PrintWarning(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public static void PrintError(string msg)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public static void PrintImportant(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public static void PrintNormal(string msg)
		{
			Console.ResetColor();
			Console.WriteLine(msg);
		}
	}
}
