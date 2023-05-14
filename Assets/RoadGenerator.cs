using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class RoadGenerator : MonoBehaviour
{
    private GameFloor _gameFloor;
    private const int houseGenInterval = 3;

    private Camera mainCam;
    public GameObject housePrefab;
    [SerializeField] private GameObject binItemsPrefab; // to be spawned for each house
    [SerializeField] private Canvas worldCanvas; // to spawn binItemUI
    
    public GameObject roadPrefab;
    private Vector3[] houseSpawns = new Vector3[4]; // every road has 2 house spawn points on each side. 0, 1 = left / 2, 3 = right

    private int maxWaypoints = 30;

    private int spawnedWaypoints = 0;
    private Vector3 lastSpawnedWaypointPos;
    private bool movingRight = true;

    private bool initialDirectionSet = false;

    public Queue<Transform> rotatingPoints = new();
    private Queue<Transform> waypointsQueue = new();

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
        waypointsQueue.Enqueue(waypoint);
        lastSpawnedWaypointPos = waypoint.position;

        // spawn houses at waypoints interval
        if(spawnedWaypoints % houseGenInterval == 0) SpawnHousePrefab(road);
        
        
        // repeat
        spawnedWaypoints++;
        if (spawnedWaypoints < maxWaypoints)
        {
            GenerateWaypoint(movingRight, waypoint);
        }
    }

    void SpawnHousePrefab(GameObject road)
    {
        float ranNum = Random.Range(0, 1);

        // 25% chance of skipping TODO: erase magic no
        if (ranNum > 0.75f)
        {
            return;
        }
        
        // if road is facing right, instantiate houses on index 0, 1 which is left side of the road
        if (movingRight)
        {
            if(ranNum > 0.5f) 
                InstantiateHousePrefab(0);
            else 
                InstantiateHousePrefab(1);
        }
        else
        // if road is facing left, instantiate houses on index 0, 1 which is left side of the road
        {
            if(ranNum > 0.5f)
                InstantiateHousePrefab(2);
            else
                InstantiateHousePrefab(3);
        }
        
        void InstantiateHousePrefab(int index)
        {
            var yRotation = movingRight ? 90 : 180;
            Debug.Log($"Instantiating a house {road.transform.GetChild(index).name} for road {spawnedWaypoints}");
            houseSpawns[index] = road.transform.GetChild(index).position;
            var newHouse = Instantiate(housePrefab, houseSpawns[index], Quaternion.Euler(0f, yRotation, 0f), this.transform);
            newHouse.GetComponentInChildren<Canvas>().transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        
    }
}
