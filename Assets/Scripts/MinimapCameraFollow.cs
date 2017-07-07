using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MinimapCameraFollow : MonoBehaviour
	{
		public Transform player;
		public Vector3 offset;
		public bool RotateWithPlayer;

		


		public void Start(){
			
		}

		public void Update() {
			transform.position = player.position + offset;
			if (RotateWithPlayer) {
				transform.rotation = player.rotation;
			} else {
				//Doesnt work
				transform.rotation.Set (0f, 0f, 0f, 1f);
			}
		}
	}
}

