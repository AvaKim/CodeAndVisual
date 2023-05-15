using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class RoadGenerator : MonoBehaviour
{
    private GameFloor _gameFloor;
    private const int houseGenInterval = 3;

    private Camera mainCam;
    public GameObject housePrefab;
    public GameObject roadPrefab;
    private Vector3[] houseSpawns = new Vector3[4]; // every road has 2 house spawn points on each side. 0, 1 = left / 2, 3 = right

    private int maxWaypoints = 89;

    private int spawnedWaypoints = 0;
    private Vector3 lastSpawnedWaypointPos;
    private bool movingRight = true;

    private bool initialDirectionSet = false;

    public Queue<Transform> rotatingPoints = new();
    public Queue<Transform> housesQueue = new();

    void Start()
    {
        _gameFloor = GetComponent<GameFloor>();
        lastSpawnedWaypointPos = transform.position;

        mainCam = Camera.main;
        var initialWaypoint = new GameObject("StartPoint").transform;
        GenerateWaypoint(true, initialWaypoint);
    }

    void GenerateWaypoint(bool wasRightFacing, Transform previousWaypoint)
    {
        // randomly face right or left (50%)
        movingRight = Random.Range(0f, 1f) > 0.5f;
        
        // make a road towards random direction at the current waypoint
        var rotationAngle = movingRight ? Vector3.forward : new Vector3(0, -90, 0);
        var road = Instantiate(roadPrefab, lastSpawnedWaypointPos, Quaternion.Euler(rotationAngle), this.transform);
        
        // create new waypoint at the end of the road
        var waypointGO = new GameObject("Waypoint");
        Transform waypoint = waypointGO.transform;
        waypoint.SetParent(this.transform);
        waypoint.position = movingRight ? new Vector3(lastSpawnedWaypointPos.x, lastSpawnedWaypointPos.y, lastSpawnedWaypointPos.z + 1) : new Vector3(lastSpawnedWaypointPos.x - 1, lastSpawnedWaypointPos.y, lastSpawnedWaypointPos.z);

        if (initialDirectionSet)
        {
            // add waypoint to rotating points
            var dirChanged = wasRightFacing != movingRight;
            if (dirChanged)
            {
                rotatingPoints.Enqueue(previousWaypoint);
            }
        } else // start changing direction only after reaching the first turning point
        {
            _gameFloor.SetTruckDirection(movingRight);
            initialDirectionSet = true;
        }

        // update variables
        lastSpawnedWaypointPos = waypoint.position;

        // spawn houses at waypoints interval.
        SpawnHouse(road);
        
        
        // repeat
        spawnedWaypoints++;
        if (spawnedWaypoints < maxWaypoints)
        {
            GenerateWaypoint(movingRight, waypoint);
        }
    }

    // spawn houses at waypoints interval.
    void SpawnHouse(GameObject roadToSpawnHouseOn)
    {
        if (spawnedWaypoints % houseGenInterval == 0)
        {
            // 50% spawn on start
            if (spawnedWaypoints == 0)
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    maxWaypoints += houseGenInterval; // extend road to make 30 houses
                    return;
                }
            }
            
            SpawnHousePrefab(roadToSpawnHouseOn);
        }
    }

    void SpawnHousePrefab(GameObject road)
    {
        float ranNum = Random.Range(0, 1);

        // if road is facing right, instantiate houses on index 0, 1 which is left side of the road
        if (movingRight)
        {
            if (ranNum > 0.5f)
                InstantiateHousePrefab(0);
            else
                InstantiateHousePrefab(1);
        }
        else
            // if road is facing left, instantiate houses on index 0, 1 which is left side of the road
        {
            if (ranNum > 0.5f)
                InstantiateHousePrefab(2);
            else
                InstantiateHousePrefab(3);
        }

        void InstantiateHousePrefab(int index)
        {
            var yRotation = movingRight ? 90 : 180;
            Debug.Log($"Instantiating a house {spawnedWaypoints} for road {spawnedWaypoints}");
            houseSpawns[index] = road.transform.GetChild(index).position;
            var newHouse = Instantiate(housePrefab, houseSpawns[index], Quaternion.Euler(0f, yRotation, 0f),
                this.transform);
            var binItemsCanvas = newHouse.GetComponentInChildren<Canvas>();
            binItemsCanvas.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            var binItemsAllocator = binItemsCanvas.GetComponent<BinItemsAllocator>();

            binItemsAllocator.SetBinItemTexts(GetRandomBinItems());
            housesQueue.Enqueue(newHouse.transform);
        }

    }

    private string[] binItemTypes = { "A", "B", "C", "D", "E", "F" };

    public string[] GetRandomBinItems()
    {
        var randomBinItems = new string[3];
        float r1 = Random.Range(0f, 1f);

        if (r1 > 0.5f)
        {
            for(int i = 0; i < 3; i++)
            {
                float r2 = Random.Range(0f, 3f);
                if (r2 < 1f)
                {
                    randomBinItems[i] = binItemTypes[0]; // A
                } else if (r2 >= 1f && r2 < 2f)
                {
                    randomBinItems[i] = binItemTypes[1]; // B
                }
                else if (r2 >= 2f && r2 < 3f)
                {
                    randomBinItems[i] = binItemTypes[2]; // C
                }
            }
        }
        else
        {
            float r2 = Random.Range(0f, 0.9f);
            int contaminatedSlot;
            if (r2 < 0.3f)
            {
                contaminatedSlot = 0;
            } else if (r2 < 0.6f)
            {
                contaminatedSlot = 1;
            } else
            {
                contaminatedSlot = 2;
            }

            for (int i = 0; i < 3; i++)
            {
                
                int r3 = (int) Random.Range(0f, 5f);
                if (i == contaminatedSlot)
                {
                    randomBinItems[i] = binItemTypes[movingRight ? 1 : 2];
                    continue;
                }
                randomBinItems[i] = binItemTypes[r3];
            }
        }
        Debug.Log("Bin Randomly Assigned: " + randomBinItems[0] + randomBinItems[1] + randomBinItems[2]);
        return randomBinItems;
    }
}
