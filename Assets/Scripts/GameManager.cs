using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using UnityEngine.Assertions;

[Serializable]
public class RoomData
{
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
        Dictionary<String, List<GameObject>> TileAssociations = new Dictionary<string, List<GameObject>>();

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

        void getTransitionAssociations()
        {
            var markers = GameObject.FindGameObjectsWithTag("TransitionMarker");
            for (int markerIndex = 0; markerIndex < markers.Length; markerIndex++)
            {
                var marker = markers[markerIndex];
                var key = marker.GetComponent<TransitionMarker>().markerKey;
                if (TileAssociations.ContainsKey(key)) TileAssociations[key].Add(marker);
                else TileAssociations[key] = new List<GameObject> { marker };
            }
        }

		void Start(){
            placeRooms();
            getTransitionAssociations();
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

        public RoomData getRoomAtLocation(Vector3 loc)
        {
            return rooms.Find(room => new Rect(room.position.x, room.position.y, room.size.x, room.size.y).Contains(loc));
        }

        public List<GameObject> getTileAssociations(string markerKey)
        {
            var assocs = TileAssociations[markerKey];
            Assert.IsTrue(assocs.Count == 2);
            return assocs;
        }
	}
}

