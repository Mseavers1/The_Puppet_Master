using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

namespace Source.Game
{
    /*
     * By Nebula Coding
     * Edited and Tweaks for project -- Michael Seavers
     * https://www.youtube.com/watch?v=nADIYwgKHv4&t=259s
     */
    public class LevelGeneration : MonoBehaviour
    {
	    public Vector2 worldSize = new Vector2(4,4);
		Room[,] _rooms;
		List<Vector2> _takenPositions = new List<Vector2>();
		private int _gridSizeX, _gridSizeY;
		public int numberOfRooms = 20;
		public GameObject roomWhiteObj;
		public Transform mapRoot;
		public List<GameObject> roomObjs;
		public float[] typeChances; // 0: normal, 1: story, 2: mini-boss, 3: secret, 4: trap, 5: loot, 6: special, 7: secret boss
		
		void Start () 
		{
			if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)){ // make sure we dont try to make more rooms than can fit in our grid
				numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
			}
			_gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
			_gridSizeY = Mathf.RoundToInt(worldSize.y);
			CreateRooms(); //lays out the actual map
			SetRoomDoors(); //assigns the doors where rooms would connect
			DrawMap(); //instantiates objects to make up a map

		}
		void CreateRooms()
		{
			//setup
			_rooms = new Room[_gridSizeX * 2,_gridSizeY * 2];
			_rooms[_gridSizeX,_gridSizeY] = new Room(Vector2.zero, 0);
			_takenPositions.Insert(0,Vector2.zero);
			Vector2 checkPos = Vector2.zero;
			//magic numbers
			float randomCompare = 0.2f, randomCompareStart = 0.15f, randomCompareEnd = 0.01f;
			//add rooms
			for (int i =0; i < numberOfRooms -1; i++){
				float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
				randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
				//grab new position
				checkPos = NewPosition();
				//test new position
				if (NumberOfNeighbors(checkPos, _takenPositions) > 1 && Random.value > randomCompare){
					int iterations = 0;
					do{
						checkPos = SelectiveNewPosition();
						iterations++;
					}while(NumberOfNeighbors(checkPos, _takenPositions) > 1 && iterations < 100);
					if (iterations >= 50)
						print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, _takenPositions));
				}
				//finalize position
				_rooms[(int) checkPos.x + _gridSizeX, (int) checkPos.y + _gridSizeY] = new Room(checkPos, 0);
				_takenPositions.Insert(0,checkPos);
			}	
		}
		Vector2 NewPosition()
		{
			int x = 0, y = 0;
			Vector2 checkingPos = Vector2.zero;
			do{
				int index = Mathf.RoundToInt(Random.value * (_takenPositions.Count - 1)); // pick a random room
				x = (int) _takenPositions[index].x;//capture its x, y position
				y = (int) _takenPositions[index].y;
				bool upDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
				bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
				if (upDown){ //find the position bnased on the above bools
					if (positive){
						y += 1;
					}else{
						y -= 1;
					}
				}else{
					if (positive){
						x += 1;
					}else{
						x -= 1;
					}
				}
				checkingPos = new Vector2(x,y);
			}while (_takenPositions.Contains(checkingPos) || x >= _gridSizeX || x < -_gridSizeX || y >= _gridSizeY || y < -_gridSizeY); //make sure the position is valid
			return checkingPos;
		}
		Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
			int index = 0, inc = 0;
			int x =0, y =0;
			Vector2 checkingPos = Vector2.zero;
			do{
				inc = 0;
				do{ 
					//instead of getting a room to find an adject empty space, we start with one that only 
					//as one neighbor. This will make it more likely that it returns a room that branches out
					index = Mathf.RoundToInt(Random.value * (_takenPositions.Count - 1));
					inc ++;
				}while (NumberOfNeighbors(_takenPositions[index], _takenPositions) > 1 && inc < 100);
				x = (int) _takenPositions[index].x;
				y = (int) _takenPositions[index].y;
				bool upDown = (Random.value < 0.5f);
				bool positive = (Random.value < 0.5f);
				if (upDown){
					if (positive){
						y += 1;
					}else{
						y -= 1;
					}
				}else{
					if (positive){
						x += 1;
					}else{
						x -= 1;
					}
				}
				checkingPos = new Vector2(x,y);
			}while (_takenPositions.Contains(checkingPos) || x >= _gridSizeX || x < -_gridSizeX || y >= _gridSizeY || y < -_gridSizeY);
			if (inc >= 100){ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
				print("Error: could not find position with only one neighbor");
			}
			return checkingPos;
		}
		int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
		{
			int ret = 0; // start at zero, add 1 for each side there is already a room
			if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
				ret++;
			}
			if (usedPositions.Contains(checkingPos + Vector2.left)){
				ret++;
			}
			if (usedPositions.Contains(checkingPos + Vector2.up)){
				ret++;
			}
			if (usedPositions.Contains(checkingPos + Vector2.down)){
				ret++;
			}
			return ret;
		}

		public Room[,] GetAllRooms()
		{
			return _rooms;
		}
		
		void DrawMap()
		{
			var randomRoom = Random.Range(1, numberOfRooms);
			var realRooms = 0;
			
			foreach (Room room in _rooms)
			{
				if (room == null){
					continue; //skip where there is no room
				}

				realRooms++;

				// Pick a random room to be the start
				if (realRooms == randomRoom)
				{
					room.Type = 8;
				}
				else
				{
					var rand = Random.Range(0f, 100f);
					var total = 0f;
					
					var t = 0;
					foreach (var chance in typeChances)
					{
						total += chance;

						if (rand <= total)
						{
							room.Type = t;
							break;
						}

						t++;
					}

				}

				Vector2 drawPos = room.GridPos;
				drawPos.x *= 1.5f;//aspect ratio of map sprite
				drawPos.y *= 1.5f;
				//create map obj and assign its variables
				MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
				mapper.type = room.Type;
				mapper.up = room.DoorTop;
				mapper.down = room.DoorBot;
				mapper.right = room.DoorRight;
				mapper.left = room.DoorLeft;
				var o = mapper.gameObject;
				o.transform.parent = mapRoot;
				mapper.name = room.GridPos.x + " " + room.GridPos.y;
				roomObjs.Add(o);

				if (room.Type == 8)
				{
					var pos = mapper.transform.position;
					Camera.main.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
					Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(pos));
				}
			}
		}
		void SetRoomDoors()
		{
			for (int x = 0; x < ((_gridSizeX * 2)); x++){
				for (int y = 0; y < ((_gridSizeY * 2)); y++){
					if (_rooms[x,y] == null){
						continue;
					}
					Vector2 gridPosition = new Vector2(x,y);
					if (y - 1 < 0){ //check above
						_rooms[x,y].DoorBot = false;
					}else{
						_rooms[x,y].DoorBot = (_rooms[x,y-1] != null);
					}
					if (y + 1 >= _gridSizeY * 2){ //check bellow
						_rooms[x,y].DoorTop = false;
					}else{
						_rooms[x,y].DoorTop = (_rooms[x,y+1] != null);
					}
					if (x - 1 < 0){ //check left
						_rooms[x,y].DoorLeft = false;
					}else{
						_rooms[x,y].DoorLeft = (_rooms[x - 1,y] != null);
					}
					if (x + 1 >= _gridSizeX * 2){ //check right
						_rooms[x,y].DoorRight = false;
					}else{
						_rooms[x,y].DoorRight = (_rooms[x+1,y] != null);
					}
				}
			}
		}
    }
}
