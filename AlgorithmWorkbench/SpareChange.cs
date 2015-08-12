using System;
using System.Linq;
using System.Collections.Generic;

namespace AlgorithmWorkbench
{
	public class SpareChange
	{
		//This list must be sorted smallest to largest, contain no duplicates, and contain nothing less than 1
		int[] _coins = {1,5,10,25,100};
		const int _amount = 1000;

		Dictionary<int, int>[] _solutionCache = null;

		public SpareChange ()
		{
			
		}

		public void Execute(){
			//Initialize the solution cache to have one value for each coin index.
			//Inside each coin index, store a dictionary which holds the solution count for a given remainder.
			//MakeChange must be referentially transparent for this to work.
			_solutionCache = new Dictionary<int,int>[_coins.Length];
			for (var i = 0; i < _solutionCache.Length; i++) {
				_solutionCache [i] = new Dictionary<int, int> ();
			}

			int results = MakeChange (_amount, _coins);
			Console.WriteLine(String.Format ("There are {0} ways to make change.", results)); 
		}

		private int MakeChange(int amount, int[] coinOptions){
			int solutions = 0;

			//For each coin in the coin options
			for (var i = (coinOptions.Length - 1); i >= 0; i--) {
				int remainder = amount - coinOptions [i];

				if (_solutionCache [i].ContainsKey (remainder)) {
					//If a tree of solutions has been found, return the result already computed
					//This is the dynamic part of dynamic programming
					solutions += _solutionCache [i] [remainder];
				} else {
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
						var result = MakeChange (remainder, coinOptions.Take (i + 1).ToArray ());
						_solutionCache [i] [remainder] = result;

						solutions += result;
					}
				}
			}

			return solutions;
		}
	}
}

