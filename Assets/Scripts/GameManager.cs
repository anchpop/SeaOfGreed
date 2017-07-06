using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

[Serializable]
public class RoomData
{
    public string name;
    public string description;
    public GameObject room;
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector2 size;
    [HideInInspector]
    public GameObject roomGameObject;

    public float Area
    {
        get { return size.x * size.y; }
    }
}

namespace SeaOfGreed
{
	public class GameManager : MonoBehaviour
    {
        public List<RoomData> rooms;

        public GameObject currentlyLoadedRoom;
        public RoomData currentRoomPrefab;

        Dictionary<String, Vector2> roomPositions;

        public GameObject player;

        public static GameManager gameManager;
		public static Options options;
		void Awake(){
			DontDestroyOnLoad (gameObject);	
		}

        void placeRooms()
        {
            var packer = new RectanglePacker();

            for (int roomIndex = 0; roomIndex < rooms.Count; roomIndex++)
            {
                var roomData = rooms[roomIndex];
                var roomTiledMap = roomData.room.GetComponent<Tiled2Unity.TiledMap>();
                roomData.size = myMath.getTiledMapSize(roomTiledMap);

                Debug.Log(roomData.size.x);
            }
            rooms.Sort((a, b) => b.Area.CompareTo(a.Area));

            for(int roomIndex = 0; roomIndex < rooms.Count; roomIndex++)
            {
                var roomData = rooms[roomIndex];
                float newx;
                float newy;

                if (!packer.Pack(roomData.size.x, roomData.size.y, out newx, out newy))
                    throw new Exception("Uh oh, we couldn't pack the rectangle :(");

                //This rectangle is now packed into position!
                roomData.position = new Vector3(newx, newy);

                roomData.roomGameObject = Instantiate(roomData.room, roomData.position, Quaternion.identity);
            }
        }

		void Start(){
            placeRooms();
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

        void switchPlayerToRoom(string roomName)
        {
            rooms.Find(roomPrefab => roomPrefab.name == roomName);
        }
	}
}

