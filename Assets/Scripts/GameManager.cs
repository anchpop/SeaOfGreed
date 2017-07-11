using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using TeamUtility.IO;
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

    public RoomData(GameObject prefab)
    {
        roomPrefab = prefab;
    }

    public float Area
    {
        get { return size.x * size.y; }
    }
}

namespace SeaOfGreed
{
	public class GameManager : MonoBehaviour
    {
        List<RoomData> rooms;

        public CameraBlackout mainCameraBlackout;
        public CameraBlackout minimapCameraBlackout;

        Dictionary<String, Vector2> roomPositions;
        Dictionary<String, List<GameObject>> TileAssociations = new Dictionary<string, List<GameObject>>();

        public GameObject player;

        public static GameManager gameManager;
		public static Options options;
        RoomData currentRoom;
		void Awake(){
			if (gameManager != null && gameManager != this) {
				Destroy (this.gameObject);
			} else {
				gameManager = this;
				DontDestroyOnLoad (this.gameObject);
			}
		}

        public void setupCameras(RoomData room)
        {
            setCameraClips(room, mainCameraBlackout);
            setCameraClips(room, minimapCameraBlackout, false);
        }

        public void setCameraClips(RoomData room, CameraBlackout blackout, bool inverty = true)
        {   
            Camera camera = blackout.GetComponent<Camera>();
            var screenPos1 = camera.WorldToScreenPoint(room.position);
            var screenPos2 = camera.WorldToScreenPoint(room.position + new Vector3(room.size.x, 0));
            var screenPos3 = camera.WorldToScreenPoint(room.position + new Vector3(room.size.x, -room.size.y));
            var screenPos4 = camera.WorldToScreenPoint(room.position + new Vector3(0, -room.size.y));

            blackout.x1 = screenPos1.x;
            blackout.x2 = screenPos2.x;
            blackout.x3 = screenPos3.x;
            blackout.x4 = screenPos4.x;
            if (inverty)
            {
                blackout.y1 = camera.pixelHeight - screenPos1.y;
                blackout.y2 = camera.pixelHeight - screenPos2.y;
                blackout.y3 = camera.pixelHeight - screenPos3.y;
                blackout.y4 = camera.pixelHeight - screenPos4.y;
            }
            else
            {
                blackout.y1 = screenPos1.y;
                blackout.y2 = screenPos2.y;
                blackout.y3 = screenPos3.y;
                blackout.y4 = screenPos4.y;
            }
        }

        void getRooms()
        {
            rooms = Resources.LoadAll("", typeof(GameObject)).Where(prefab => (prefab as GameObject).GetComponent<Tiled2Unity.TiledMap>() != null).Select(prefab => new RoomData(prefab as GameObject)).ToList();
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
            getRooms();
            placeRooms();
            getTransitionAssociations();
            gameManager = this;
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

        public void setNewRoom(RoomData newRoom)
        {
            currentRoom = newRoom;
        }

        public RoomData getRoomAtLocation(Vector3 loc)
        {
            for (int roomIndex = 0; roomIndex < rooms.Count; roomIndex++)
            {
                var room = rooms[roomIndex];
                if (new Rect(room.position.x, room.position.y - room.size.y, room.size.x, room.size.y).Contains(new Vector2(loc.x, loc.y))) return room;
            }
            return null;
        }

        public List<GameObject> getTileAssociations(string markerKey)
        {
            var assocs = TileAssociations[markerKey];
            Assert.IsTrue(assocs.Count == 2);
            return assocs;
        }

        public void switchedToRoom()
        {
            mainCameraBlackout.GetComponent<FollowPlayer>().setCameraToPlayer();
        }

    }
}

