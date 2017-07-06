using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

[Serializable]
public struct roomPrefabs
{
    public string name;
    public string description;
    public GameObject room;
}

namespace SeaOfGreed
{
	public class GameManager : MonoBehaviour
    {
        public List<roomPrefabs> rooms;

        public static GameManager gameManager;
		public static Options options;
		void Awake(){
			DontDestroyOnLoad (gameObject);	
		}

		void Start(){
			gameManager = this;
			options = new Options ();
			Load ();

		}

		public void Save(){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/options.dat", FileMode.OpenOrCreate);
			bf.Serialize (file, options);
			file.Close ();
		}

		public void Load(){
			
			if (File.Exists (Application.persistentDataPath + "/options.dat")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/options.dat", FileMode.Open);
				options = (Options)bf.Deserialize (file);
			} else {
				options.Defaults ();
			}
		}
	}
}

