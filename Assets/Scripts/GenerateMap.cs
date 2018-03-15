using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateMap : MonoBehaviour {

	public GameObject wallPrefab,
		floorPrefab,
		doorVerticalPrefab,
		doorHorizontalPrefab;

	const int MAP_SIZE = 30, // Map Size
	MIN_LENGTH = 5; // The Smallest Room Size

	Room root; // The Biggest Room
	Tile[][] map; // The Floors / Tiles

	void Start () {
		// Initialize Map Size
		map = new Tile[MAP_SIZE][];
		for (int i = 0; i < MAP_SIZE; i++){
			map[i] = new Tile[MAP_SIZE];

			for (int j = 0; j < MAP_SIZE; j++){
				map[i][j] = new Tile();
			}
		}

		// Create First Room (The Biggest One)
		root = MakeRoom(0, 0, MAP_SIZE, MAP_SIZE);

		// Split The Room
		SplitRoom(root);

		// Flagging Each Point As Wall
		MakeWall(root);

		//GameObject wallInstance, floorInstance;
		int z = 0;
		for (int i = 0; i < MAP_SIZE; i++){
			for (int j = 0; j < MAP_SIZE; j++){
				//Debug.Log ("i:" + i + " j:" + j + " z:" + angka);
				if (map [j] [i].isVerticalDoor) {
					map [j] [i].doorVerticalObject = Instantiate (doorVerticalPrefab, new Vector3 (j * 1, 0, z), Quaternion.identity) as GameObject;
				}else if(map [j] [i].isHorizontalDoor){
					map [j] [i].doorHorizontalObject = Instantiate (doorHorizontalPrefab, new Vector3 (j * 1, 0, z), Quaternion.identity) as GameObject;
				}else if (map [j] [i].isWall) {
					map [j] [i].wallObject = Instantiate (wallPrefab, new Vector3 (j * 1, 0, z), Quaternion.identity) as GameObject;
				} else {
					map [j] [i].floorObject = Instantiate (floorPrefab, new Vector3 (j * 1, 0, z), Quaternion.identity) as GameObject;
				}
			}
			z += 1;
		}
	}

	// Function To Initialize New Room Object
	public Room MakeRoom(int x, int y, int width, int height){
		Room room = new Room();

		room.x = x;
		room.y = y;
		room.width = width;
		room.height = height;
		room.leftRoom = room.rightRoom = null;

		return room;
	}

	// Function To Split The Room Object Into 2 New Sub Room Object
	public void SplitRoom(Room room){
		// For Door X Coordinate or Door Y Coordinate
		int door;

		// For Door X Coordinate or Door Y Coordinate
		int randomPoint;

		// Check If We Should Split The Room Horizontally or Vertically
		if (room.width >= room.height &&
			room.width > MIN_LENGTH * 2){
			do{
				// Split Randomly
				randomPoint = new System.Random().Next() % room.width;
			}while (randomPoint < MIN_LENGTH || // Validating That Random Point Is Not Smaller Than Minimum Length
				room.width - randomPoint < MIN_LENGTH || // Validating That The New Sub Room Is Not Smaller Than Minimum Length
				map[room.y][room.x + randomPoint - 1].isHorizontalDoor || // Validating That There Is No Door At The End Of The Newly Generated Wall
				map[room.y + room.height - 1][room.x + randomPoint - 1].isHorizontalDoor
			);

			// Generate Door at Random Point
			do{
				door = new System.Random().Next() % room.height;
			}while (door % (room.height - 1) == 0); // Validating That Door Is Not At The End of The Newly Generated Wall

			// Place Door
			map[room.y + door][room.x + randomPoint - 1].isVerticalDoor = true;

			// Generate The New Room To Left Sub Room and Split It
			room.leftRoom = MakeRoom(room.x, room.y, randomPoint, room.height);
			SplitRoom(room.leftRoom);

			// Generate The New Room To Right Sub Room and Split It
			room.rightRoom = MakeRoom(room.x + randomPoint - 1, room.y, room.width - randomPoint + 1, room.height);
			SplitRoom(room.rightRoom);
		}else if (room.height > MIN_LENGTH * 2){
			do{
				// Split Randomly
				randomPoint = new System.Random().Next() % room.height;
			}while (randomPoint < MIN_LENGTH || // Validating That Random Point Is Not Smaller Than Minimum Length
				room.height - randomPoint < MIN_LENGTH || // Validating That The New Sub Room Is Not Smaller Than Minimum Length
				map[room.y + randomPoint - 1][room.x].isVerticalDoor || // Validating That There Is No Door At The End Of The Newly Generated Wall
				map[room.y + randomPoint - 1][room.x + room.width - 1].isVerticalDoor
			);

			// Generate Door at Random Point
			do{
				door = new System.Random().Next() % room.width;
			}
			while (door % (room.width - 1) == 0); // Validating That Door Is Not At The End of The Newly Generated Wall

			// Place Door
			map[room.y + randomPoint - 1][room.x + door].isHorizontalDoor = true;

			// Generate The New Room To Left Sub Room
			room.leftRoom = MakeRoom(room.x, room.y, room.width, randomPoint);

			// Generate The New Room To Right Sub Room
			room.rightRoom = MakeRoom(room.x, room.y + randomPoint - 1, room.width, room.height - randomPoint + 1);

			// Split The Room
			SplitRoom(room.leftRoom);
			SplitRoom(room.rightRoom);
		}
	}

	public void MakeWall(Room room){
		if (room != null){
			for (int i = 0; i < room.height; i++){
				for (int j = 0; j < room.width; j++){
					if (i % (room.height - 1) == 0 ||
						j % (room.width - 1) == 0){
						map[i + room.y][j + room.x].isWall = true;
					}
				}
			}

			MakeWall(room.leftRoom);
			MakeWall(room.rightRoom);
		}
	}
}
