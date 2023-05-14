using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class RoadGenerator : MonoBehaviour
{
    private GameFloor _gameFloor;

    public GameObject housePrefab;

    public GameObject roadPrefab;
    private Vector3[] houseSpawns = new Vector3[4]; // every road has 2 house spawn points on each side. 0, 1 = left / 2, 3 = right

    public int maxWaypoints = 10;

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

        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"Instantiating a house {i} {road.transform.GetChild(i).name} for road {spawnedWaypoints}");
            houseSpawns[i] = road.transform.GetChild(i).position;
        }

        foreach (var spawnPoint in houseSpawns)
        {
            Instantiate(housePrefab, spawnPoint, Quaternion.identity, this.transform);
        }
        
        // repeat
        spawnedWaypoints++;
        if (spawnedWaypoints < maxWaypoints)
        {
            GenerateWaypoint(movingRight, waypoint);
        }
    }
}
