using System;
using System.Linq;

namespace AlgorithmWorkbench
{
	public class SpareChange
	{
		//This list must be sorted smallest to largest, contain no duplicates, and contain nothing less than 1
		int[] _coins = {1,5,10,25,100};
		const int _amount = 1000;

		public SpareChange ()
		{
		}

		public void Execute(){
			int results = MakeChange (_amount, _coins);
			Console.WriteLine(String.Format ("There are {0} ways to make change.", results)); 
		}

		private int MakeChange(int amount, int[] coinOptions){
			int solutions = 0;

			//For each coin in the coin options
			for(var i = (coinOptions.Length-1
			); i>=0; i--){
				int remainder = amount - coinOptions [i];

			//	If amount - value == 0
			//		You've found an option, add one
				if (remainder == 0) {
					solutions++;
				}

			//	If amount - value < 0
			//		You cannot make change with this coin, do not add
				else if (remainder < 0) {
					continue;
				}
			//	Otherwise
			//		Find the number of MakeChange options if any coin larger is not an option
				else {
					solutions += MakeChange (remainder, coinOptions.Take (i + 1).ToArray ());
				}
			}

			return solutions;
		}
	}
}

