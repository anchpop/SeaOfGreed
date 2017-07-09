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
    public GameObject roomPrefab;
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

        public CameraBlackout mainCameraBlackout;

        Dictionary<String, Vector2> roomPositions;
        Dictionary<String, List<GameObject>> TileAssociations = new Dictionary<string, List<GameObject>>();

        public GameObject player;

        public static GameManager gameManager;
		public static Options options;

        RoomData currentRoom;

        void Awake(){
			DontDestroyOnLoad (gameObject);	
		}

        public void setCameraClips(RoomData room)
        {   
            
            Camera camera = mainCameraBlackout.GetComponent<Camera>();
            var screenPos1 = camera.WorldToScreenPoint(room.position);
            var screenPos2 = camera.WorldToScreenPoint(room.position + new Vector3(room.size.x, 0));
            var screenPos3 = camera.WorldToScreenPoint(room.position + new Vector3(room.size.x, -room.size.y));
            var screenPos4 = camera.WorldToScreenPoint(room.position + new Vector3(0, -room.size.y));

            mainCameraBlackout.x1 = screenPos1.x;
            mainCameraBlackout.y1 = camera.pixelHeight - screenPos1.y;
            mainCameraBlackout.x2 = screenPos2.x;
            mainCameraBlackout.y2 = camera.pixelHeight - screenPos2.y;
            mainCameraBlackout.x3 = screenPos3.x;
            mainCameraBlackout.y3 = camera.pixelHeight - screenPos3.y;
            mainCameraBlackout.x4 = screenPos4.x;
            mainCameraBlackout.y4 = camera.pixelHeight - screenPos4.y;

        }

        void placeRooms()
        {
            var packer = new RectanglePacker();

            for (int roomIndex = 0; roomIndex < rooms.Count; roomIndex++)
            {
                var roomData = rooms[roomIndex];
                var roomTiledMap = roomData.roomPrefab.GetComponent<Tiled2Unity.TiledMap>();
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

                roomData.roomGameObject = Instantiate(roomData.roomPrefab, roomData.position, Quaternion.identity);
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

        public void setNewRoom(RoomData newRoom)
        {
            currentRoom = newRoom;
        }

        public RoomData getRoomAtLocation(Vector3 loc)
        {
            for (int roomIndex = 0; roomIndex < rooms.Count; roomIndex++)
            {
                var room = rooms[roomIndex];
                if (new Rect(room.position.x, room.position.y, room.size.x, room.size.y).Contains(new Vector2(loc.x, -loc.y))) return room;
            }
            return null;
        }

        public List<GameObject> getTileAssociations(string markerKey)
        {
            var assocs = TileAssociations[markerKey];
            Assert.IsTrue(assocs.Count == 2);
            return assocs;
        }
	}
}

