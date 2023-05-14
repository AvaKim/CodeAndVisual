using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFloor : MonoBehaviour
{
    public const float moveSpeed = 0.5f;
    public Transform truckTransform;
    
    private RoadGenerator _roadGenerator;
    private Queue<Transform> _rotatingPoints;
    private Transform nextRotatingPoint => _rotatingPoints.Count > 0 ? _rotatingPoints.Peek() : null;
    public bool isMovingRight = true;
    
    
    void Start()
    {
        _roadGenerator = GetComponent<RoadGenerator>();
        _rotatingPoints = _roadGenerator.rotatingPoints;
    }

    void Update()
    {
        if (nextRotatingPoint == null)
        {
            return;
        }
        
        if (CalculateDistanceExceptY(truckTransform.position, nextRotatingPoint.position) < 0.01f)
        {
            Debug.Log("Turning point reached");

            // change direction
            isMovingRight = !isMovingRight;
            truckTransform.rotation = Quaternion.Euler(isMovingRight ? new Vector3(0, 0, 0) : new Vector3(0, -90f, 0f));
            
            _rotatingPoints.Dequeue();
        }
    }
    
    private float CalculateDistanceExceptY (Vector3 v1, Vector3 v2)
    {
        float xDiff = v1.x - v2.x;
        float zDiff = v1.z - v2.z;
        return Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
    }

    private void FixedUpdate()
    {
        // move the map right if truck is moving left on X, move the map down on Z if truck is moving right/upward.
        transform.Translate(isMovingRight ? Vector3.back  * (moveSpeed * Time.deltaTime) : Vector3.right * (moveSpeed * Time.deltaTime));
    }

    public void SetTruckDirection(bool movingRight)
    {
        isMovingRight = movingRight;
        truckTransform.rotation = Quaternion.Euler(isMovingRight ? new Vector3(0, 0, 0) : new Vector3(0, -90f, 0f));
        
    }
}
