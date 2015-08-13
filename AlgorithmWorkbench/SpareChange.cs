using System;
using System.Collections.Generic;
using Amount = System.Int32;
using Solutions = System.Int64;
using MaxCoinIndex = System.Int32;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AlgorithmWorkbench
{
	public class SpareChange
	{
		//This list must be sorted smallest to largest, contain no duplicates, and contain nothing less than 1
		int[] _coins = {1,5,10,25,100};
		const int _amount = 5000000;

		//There will be one dictionary in the array for each MaxCoinIndex.
		//After accessing by MaxCoinIndex, use a remainder value to cache and recall the number of solutions.
		ConcurrentDictionary<Amount, Solutions>[] _solutionCache = null;

		public SpareChange ()
		{
			
		}

		public void Execute(){
			var numberOfCoins = _coins.Length;
			var maxCoinIndex = numberOfCoins - 1;

			//Initialize the solution cache to have one value for each coin index.
			//Inside each coin index, store a dictionary which holds the solution count for a given remainder.
			//MakeChange must be referentially transparent for this to work.
			_solutionCache = new ConcurrentDictionary<Amount,Solutions>[numberOfCoins];
			for (var i = 0; i < _solutionCache.Length; i++) {
				_solutionCache [i] = new ConcurrentDictionary<Amount, Solutions> ();
			}

			var results = MakeChange (maxCoinIndex, _amount, _coins).Result.Item3;
			Console.WriteLine (String.Format ("There are {0} ways to make change.", results)); 
		}

		async Task<Tuple<MaxCoinIndex, Amount, Solutions>> MakeChange(MaxCoinIndex maxIdx, Amount amount, int[] coinArray){
			Int64 solutions = 0;

			var taskList = new List<Task<Tuple<MaxCoinIndex, Amount, Solutions>>>();

			//For each coin in the coin options
			for (var coinIdx = maxIdx; coinIdx >= 0; coinIdx--) {
				int remainder = amount - coinArray[coinIdx];

				if (_solutionCache[coinIdx].ContainsKey (remainder)) {
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
						taskList.Add(MakeChange (coinIdx, remainder, coinArray));
					}
				}
			}

			var asyncResults = await Task.WhenAll (taskList);

			foreach (var result in asyncResults) {
				solutions += result.Item3;
				_solutionCache [result.Item1] [result.Item2] = result.Item3;
			}

			return new Tuple<MaxCoinIndex, Amount, Solutions>(maxIdx, amount, solutions);
		}
	}
}

