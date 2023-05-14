using UnityEngine;
using System.Collections.Generic;


public class RoadGenerator : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform truckTransform;
    private Vector3 truckPos;

    public int maxWaypoints = 10;

    private int currentSpawnedWaypoints = 0;
    private Vector3 currentWaypointPos;
    
    void Start()
    {
        truckPos = truckTransform.position;
        currentWaypointPos = this.transform.position;
        GenerateWaypoint();
    }
    void GenerateWaypoint()
    {
        // choose right or left (50%)
        bool rightDir = Random.Range(0f, 1f) > 0.5f;
        
        // make a road towards selected direction at the current waypoint
        var direction = rightDir ? Vector3.zero : new Vector3(0, -90, 0);
        var road = Instantiate(roadPrefab, currentWaypointPos, Quaternion.Euler(direction), this.transform);

        // create new waypoint at the end of the road
        var waypointGO = new GameObject("Waypoint");
        Transform waypoint = waypointGO.transform;
        waypoint.SetParent(this.transform);
        waypoint.position = rightDir ? new Vector3(currentWaypointPos.x, currentWaypointPos.y, currentWaypointPos.z + 1) : new Vector3(currentWaypointPos.x - 1, currentWaypointPos.y, currentWaypointPos.z);

        currentWaypointPos = waypoint.position;
        
        // repeat
        currentSpawnedWaypoints++;
        if (currentSpawnedWaypoints < maxWaypoints)
        {
            GenerateWaypoint();
        }
    }
}
