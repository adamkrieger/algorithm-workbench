using System;
using LogExec;

namespace AlgorithmWorkbench
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			var spareChange = new SpareChange ();

			using (new ExecutionTimeLogger ("Code")) {
				spareChange.Execute ();
			}

			Console.ReadLine ();
		}
	}
}
