using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.2f;
    public float clickScale = 0.8f;
    public float scaleSpeed = 10f;
    
    private Vector3 defaultScale;
    private bool isHovering = false;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        isHovering = false;
        transform.localScale = defaultScale;
    }

    private void OnDisable()
    {
        transform.localScale = defaultScale;
    }

    private void Update()
    {
        if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale * hoverScale, Time.deltaTime * scaleSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerDown()
    {
        transform.localScale = defaultScale * clickScale;
    }

    public void OnPointerUp()
    {
        transform.localScale = defaultScale;
    }
}