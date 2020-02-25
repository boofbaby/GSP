using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int mapDimensions;
    public Vector2Int numberOfRooms;
    public GameObject player;
    public Vector2 unitsPerRoom;
    public Room[,] rooms;

    private List<Vector2Int> usedPositions = new List<Vector2Int>();
    private int selectedNumberOfRooms;

    private GameObject[] genericRooms;
    private GameObject[] bossRooms;
    private GameObject[] spiritRooms;
    private GameObject[] treasureRooms;
    private Vector2Int origin;
    private GameObject playerInstance;
    private GameObject spiritInstance;

    void Start()
    {
        LoadResources();

        Random.InitState(317999); // Set the level seed

        selectedNumberOfRooms = Random.Range(numberOfRooms.x, numberOfRooms.y + 1); // Randomly set a number of rooms between the two given values
        if (selectedNumberOfRooms > mapDimensions.x * mapDimensions.y) selectedNumberOfRooms = mapDimensions.x * mapDimensions.y; // Make sure there is enough space in the map

        CreateRooms();
        SetDoors();
        InitializeRooms();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnSpirit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReactivatePlayer();
        }
    }

    private void CreateRooms()
    {
        rooms = new Room[mapDimensions.x, mapDimensions.y];

        float randomCompare = 0.01f, randomCompareStart = 0.01f, randomCompareEnd = 0.01f;
        
        origin = new Vector2Int(Mathf.RoundToInt(mapDimensions.x / 2), Mathf.RoundToInt(mapDimensions.y / 2));
        CreateRoom(origin, 0);
        playerInstance = Instantiate(player);
        playerInstance.transform.position = rooms[origin.x, origin.y].transform.position + new Vector3(0,1,0);

        int iterations = 0;

        Vector2Int checkPos = Vector2Int.zero;
        for (int i = 1; i < selectedNumberOfRooms - 1; i++)
        {
            checkPos = NewPosition();

            if (NumberOfNeighbors(checkPos) > 1 && Random.value > randomCompare)
            {
                iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos));
            }

            if (i == selectedNumberOfRooms - 2) CreateRoom(checkPos, 1);
            else CreateRoom(checkPos, 0);
        }

        // Create a spirit room
        AddRoom(2);

        AddRoom(3);
    }

    public void SpawnSpirit()
    {
        playerInstance.SetActive(false);

        if (spiritInstance == null)
        {
            spiritInstance = Instantiate(player);
            spiritInstance.layer = 16;
            Transform[] ts = GetComponentsInChildren<Transform>();
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i].gameObject.layer = 16;
            }
        }
        else
        {
            spiritInstance.SetActive(true);
        }
        
        spiritInstance.transform.position = rooms[origin.x, origin.y].transform.position + new Vector3(0, 3, 0);
    }

    public void ReactivatePlayer()
    {
        spiritInstance.SetActive(false);
        playerInstance.SetActive(true);
    }

    private Vector2Int NewPosition()
    {
        int x = 0, y = 0;
        Vector2Int checkingPos = Vector2Int.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (usedPositions.Count - 1)); // pick a random room
            x = (int)usedPositions[index].x;//capture its x, y position
            y = (int)usedPositions[index].y;
            bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
            bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
            if (UpDown)
            { //find the position bnased on the above bools
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2Int(x, y);
        } while (usedPositions.Contains(checkingPos) || x >= mapDimensions.x || x < 0 || y >= mapDimensions.y || y < 0); //make sure the position is valid
        return checkingPos;
    }

    private int NumberOfNeighbors(Vector2Int checkingPos)
    {
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2Int.right))
        { //using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2Int.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2Int.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2Int.down))
        {
            ret++;
        }
        return ret;
    }

    Vector2Int SelectiveNewPosition()
    { // method differs from the above in the two commented ways
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2Int checkingPos = Vector2Int.zero;
        do
        {
            inc = 0;
            do
            {
                //instead of getting a room to find an adject empty space, we start with one that only 
                //as one neighbor. This will make it more likely that it returns a room that branches out
                index = Mathf.RoundToInt(Random.value * (usedPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(usedPositions[index]) > 1 && inc < 100);
            x = (int)usedPositions[index].x;
            y = (int)usedPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2Int(x, y);
        } while (usedPositions.Contains(checkingPos) || x >= mapDimensions.x || x < 0 || y >= mapDimensions.y || y < 0);
        if (inc >= 100)
        { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
            print("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }

    private void InitializeRooms()
    {
        for (int i = 0; i < usedPositions.Count; i++)
        {

        }
    }

    private void CreateRoom(Vector2Int _position, int _type)
    {
        if (_position.x >= 0 && _position.x < mapDimensions.x && _position.y >= 0 && _position.y < mapDimensions.y)
        {
            switch(_type)
            {
                case 0:
                    rooms[_position.x, _position.y] = Instantiate(genericRooms[Random.Range(0, genericRooms.Length)]).GetComponent<Room>();
                    break;
                case 1:
                    rooms[_position.x, _position.y] = Instantiate(bossRooms[Random.Range(0, bossRooms.Length)]).GetComponent<Room>();
                    break;
                case 2:
                    rooms[_position.x, _position.y] = Instantiate(spiritRooms[Random.Range(0, spiritRooms.Length)]).GetComponent<Room>();
                    break;
                case 3:
                    rooms[_position.x, _position.y] = Instantiate(treasureRooms[Random.Range(0, treasureRooms.Length)]).GetComponent<Room>();
                    break;
            }
            
            rooms[_position.x, _position.y].transform.position = new Vector3(unitsPerRoom.x * _position.x, 0, unitsPerRoom.y * _position.y);
            rooms[_position.x, _position.y].transform.SetParent(transform);
            usedPositions.Insert(0, _position);
        }
    }

    private void SetDoors()
    {
        for (int i = 0; i < usedPositions.Count; i++)
        {
            // Upper door check
            if (usedPositions[i].y + 1 < mapDimensions.y)
            {
                if (usedPositions.Contains(new Vector2Int(usedPositions[i].x, usedPositions[i].y + 1)))
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[2] = true;
                }
                else
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[2] = false;
                }
            }

            // Lower door check
            if (usedPositions[i].y - 1 > 0)
            {
                if (usedPositions.Contains(new Vector2Int(usedPositions[i].x, usedPositions[i].y - 1)))
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[3] = true;
                }
                else
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[3] = false;
                }
            }

            // Right door check
            if (usedPositions[i].x + 1 < mapDimensions.x)
            {
                if (usedPositions.Contains(new Vector2Int(usedPositions[i].x + 1, usedPositions[i].y)))
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[1] = true;
                }
                else
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[1] = false;
                }
            }


            // Left door check
            if (usedPositions[i].x - 1 > 0)
            {
                if (usedPositions.Contains(new Vector2Int(usedPositions[i].x - 1, usedPositions[i].y)))
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[0] = true;
                }
                else
                {
                    rooms[usedPositions[i].x, usedPositions[i].y].neighbors[0] = false;
                }
            }

            rooms[usedPositions[i].x, usedPositions[i].y].Activate(Random.Range(0, 32000));
        }
    }

    private void AddRoom(int _type)
    {
        Vector2Int checkPos = Vector2Int.zero;
        int iterations = 0;
        float randomCompare = 0.01f, randomCompareStart = 0.01f, randomCompareEnd = 0.01f;
        
        checkPos = NewPosition();

        if (NumberOfNeighbors(checkPos) > 1 && Random.value > randomCompare)
        {
            iterations = 0;
            do
            {
                checkPos = SelectiveNewPosition();
                iterations++;
            } while (NumberOfNeighbors(checkPos) > 1 && iterations < 100);
            if (iterations >= 50)
                print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos));
        }

        CreateRoom(checkPos, _type);
    }

    private void LoadResources()
    {
        genericRooms = Resources.LoadAll<GameObject>("Level Prefabs/Generic Rooms");
        bossRooms = Resources.LoadAll<GameObject>("Level Prefabs/Boss Rooms");
        spiritRooms = Resources.LoadAll<GameObject>("Level Prefabs/Spirit Rooms");
        treasureRooms = Resources.LoadAll<GameObject>("Level Prefabs/Treasure Rooms");
    }
}