using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

namespace SeaOfGreed
{
	public class StateChangedEventArgs : System.EventArgs
	{
		public states beforeState;
		public states afterState;

		public StateChangedEventArgs(states before, states after){
			beforeState = before;
			afterState = after;
		}
	}



}