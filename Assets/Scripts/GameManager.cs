using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using TeamUtility.IO;

namespace SeaOfGreed
{
	public class GameManager : MonoBehaviour
	{
		
		public static GameManager gameManager;
		public static Options options;
		void Awake(){
			if (gameManager != null && gameManager != this) {
				Destroy (this.gameObject);
			} else {
				gameManager = this;
				DontDestroyOnLoad (this.gameObject);
			}
		}

		void Start(){
			options = new Options ();
			Load ();

		}

		public void Save(){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream optionsFile = File.Open (Application.persistentDataPath + "/options.dat", FileMode.OpenOrCreate);
			bf.Serialize (optionsFile, options);
			optionsFile.Close ();
			InputManager.Save (Application.persistentDataPath + "/inputs.xml");
		}

		public void Load(){
			
			if (File.Exists (Application.persistentDataPath + "/options.dat")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/options.dat", FileMode.Open);
				options = (Options)bf.Deserialize (file);
				file.Close ();
			} else {
				options.Defaults ();
			}
			if (File.Exists (Application.persistentDataPath + "/inputs.xml")) {
				//InputManager.Load (Application.persistentDataPath + "/inputs.xml");
			}
		}

	}
}

