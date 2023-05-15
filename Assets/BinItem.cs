using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BinItem : MonoBehaviour
{
    public string binItemType;
    public bool isMoving = false;

    [SerializeField]
    private GameObject onCollectionScoreTextUI;

    private TextMeshProUGUI itemTextUI;
    private Animator fadeIn;
    private float speed = 1f;

    void Start()
    {
        fadeIn =
            GetComponent<Animator>();


        itemTextUI = GetComponent<TextMeshProUGUI>();
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            var dir = (truck.position - transform.position).normalized;
            
            transform.Translate(dir * speed * Time.unscaledDeltaTime);

            if (Vector3.Distance(truck.position, transform.position) < 0.2f)
            {
                OnItemReachedTruck();
            }
        }
    }

    void OnItemReachedTruck()
    {
        isMoving = false;
        fadeIn.enabled = true;
        var textColor = Color.green;
        if (binItemType.Equals("A") || binItemType.Equals("B") || binItemType.Equals("C"))
        {
            // successful collection
            Invoke(nameof(AddScore), 0.5f);
        }
        else
        {
            // collected contaminated item. apply penalty
            Invoke(nameof(SubtractScore), 0.5f);
            textColor = Color.red;
        }
        
        //Spawn score text
        itemTextUI.color = textColor;
        var textUI = Instantiate(onCollectionScoreTextUI, transform);
        textUI.GetComponent<TextMeshProUGUI>().color = textColor; 
        Destroy(this.gameObject, 0.5f);
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
