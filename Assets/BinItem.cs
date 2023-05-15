using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BinItem : MonoBehaviour
{
    public string binItemType;
    public bool isMoving = false;
    

    [SerializeField]
    private Transform scoreTextSpawnPos;
    private Transform worldCanvas;
    private TextMeshProUGUI itemTextUI;
    private Animator fadeIn;
    private Camera mainCam;
    private float speed = 1f;

    void Start()
    {
        mainCam = Camera.main;
        fadeIn =
            GetComponent<Animator>();

        worldCanvas = GameObject.FindGameObjectWithTag("WorldCanvas").transform;
            
        itemTextUI = GetComponent<TextMeshProUGUI>();
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            var dir = (truck.position - transform.position).normalized;
            
            transform.Translate(dir * speed * Time.unscaledDeltaTime);

            if (Vector3.Distance(truck.position, transform.position) < 0.15f)
            {
                OnItemReachedTruck();
            }
        }
    }

    void OnItemReachedTruck()
    {
        isMoving = false;
        fadeIn.enabled = true;
        if (binItemType.Equals("A") || binItemType.Equals("B") || binItemType.Equals("C"))
        {
            // successful collection
            Invoke(nameof(AddScore), 0.5f);
            itemTextUI.color = Color.green;
        }
        else
        {
            // collected contaminated item. apply penalty
            Invoke(nameof(SubtractScore), 0.5f);
            itemTextUI.color = Color.red;
            mainCam.transform.GetComponent<CameraShake>().Shake();
        }
        
        Destroy(this.gameObject, 2f);
    }

    void AddScore()
    {
        GameManager.Instance.AddScore(10); 
    }

    void SubtractScore()
    {
        GameManager.Instance.SubtractScore(5);
        GameManager.Instance.ContaminateByAmount(0.1f);
    }
    
    private Transform truck;
    public void MoveTo(Transform target)
    {
        isMoving = true;
        truck = target;
    }
}
