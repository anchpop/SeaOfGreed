using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Keybindings
	{
		public static KeyCode playerForward;
		public static KeyCode playerReverse;
		public static KeyCode playerLeft;
		public static KeyCode playerRight;

		public static KeyCode use;
		public static KeyCode enterShip;

		public static KeyCode shipForward;
		public static KeyCode shipReverse;
		public static KeyCode shipLeft;
		public static KeyCode shipRight;

		//Load previous keybindings
		public static void Load(){
			
			 playerForward = KeyCode.W;
			 playerReverse = KeyCode.S;
			 playerLeft = KeyCode.A;
			 playerRight = KeyCode.D;

			 use = KeyCode.E;
			 enterShip = KeyCode.V;

			 shipForward = KeyCode.W;
			 shipReverse = KeyCode.S;
			 shipLeft = KeyCode.A;
			 shipRight = KeyCode.D;
		}

	}

}

