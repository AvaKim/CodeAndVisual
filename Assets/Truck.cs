using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public int numHousePassed = 0;
    private GameFloor _gameFloor;
    [SerializeField] private float moveSpeedSlowRate = 0.5f;

    void Start()
    {
        _gameFloor = FindObjectOfType<GameFloor>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "House")
        {
            _gameFloor.moveSpeed *= moveSpeedSlowRate;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "House")
        {
            _gameFloor.moveSpeed /= moveSpeedSlowRate;

            Debug.Log("House passed: " + ++numHousePassed);
        }
        
    }
}
