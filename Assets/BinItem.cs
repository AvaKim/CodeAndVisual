using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinItem : MonoBehaviour
{
    public bool isMoving = false;

    private Animator fadeIn;
    private float speed = 1f;

    void Start()
    {
        fadeIn =
            GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            var dir = (truck.position - transform.position).normalized;
            
            transform.Translate(dir * speed * Time.unscaledDeltaTime);

            if (Vector3.Distance(truck.position, transform.position) < 0.2f)
            {
                isMoving = false;
                fadeIn.enabled = true;
                Destroy(this.gameObject, 0.2f);
            }
        }
    }

    private Transform truck;
    public void MoveTo(Transform target)
    {
        isMoving = true;
        truck = target;
    }
}
