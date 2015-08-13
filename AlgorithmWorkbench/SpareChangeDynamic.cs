using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmWorkbench
{
	public class SpareChangeDynamic
	{
		//This list must be sorted smallest to largest, contain no duplicates, and contain nothing less than 1
		int[] _coins = {1,5,10,25,100};
		const int _amount = 5000000;

		Dictionary<int, Int64>[] _solutionCache = null;

		public void Execute(){
			//Initialize the solution cache to have one value for each coin index.
			//Inside each coin index, store a dictionary which holds the solution count for a given remainder.
			//MakeChange must be referentially transparent for this to work.
			_solutionCache = new Dictionary<int,Int64>[_coins.Length];
			for (var i = 0; i < _solutionCache.Length; i++) {
				_solutionCache [i] = new Dictionary<int, Int64> ();
			}

			var results = MakeChange (_amount, _coins);
			Console.WriteLine(String.Format ("There are {0} ways to make change.", results)); 
		}

		Int64 MakeChange(int amount, int[] coinArray){
			Int64 solutions = 0;

			//For each coin in the coin options
			for (var coinIdx = (coinArray.Length - 1); coinIdx >= 0; coinIdx--) {
				int remainder = amount - coinArray [coinIdx];

				if (_solutionCache [coinIdx].ContainsKey (remainder)) {
					//If a tree of solutions has been found, return the result already computed
					//This is the dynamic part of dynamic programming
					solutions += _solutionCache [coinIdx] [remainder];
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
						var result = MakeChange (remainder, coinArray.Take (coinIdx + 1).ToArray ());
						_solutionCache [coinIdx] [remainder] = result;

						solutions += result;
					}
				}
			}

			return solutions;
		}
	}
}

