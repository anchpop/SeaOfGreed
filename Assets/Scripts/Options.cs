using System;
using UnityEngine;

namespace SeaOfGreed
{
	[Serializable]
	public class Options
	{

		public bool MinimapRotateWithPlayer;
		public bool MinimapRotateWithShip;


		public void Defaults(){
			MinimapRotateWithPlayer = false;
			MinimapRotateWithShip = false;
		}
	}
}