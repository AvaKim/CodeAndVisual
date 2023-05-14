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
        
        if (Vector3.Distance(truckTransform.position, nextRotatingPoint.position) < 0.12f)
        {
            Debug.Log("Turning point reached");
            // clamp position
            truckTransform.position = nextRotatingPoint.position;

            // change direction
            isMovingRight = !isMovingRight;
            truckTransform.LookAt(isMovingRight ? Vector3.forward : Vector3.left);
            
            _rotatingPoints.Dequeue();
        }
    }

    private void FixedUpdate()
    {
        // move the map right if truck is moving left on X, move the map down on Z if truck is moving right/upward.
        transform.Translate(isMovingRight ? Vector3.back  * (moveSpeed * Time.deltaTime) : Vector3.right * (moveSpeed * Time.deltaTime));
    }

    public void SetTruckDirection(bool movingRight)
    {
        isMovingRight = movingRight;
        truckTransform.LookAt(isMovingRight ? Vector3.forward : Vector3.left);
        
        Debug.Log($"Initial direction set to right: {movingRight}");
    }
}
