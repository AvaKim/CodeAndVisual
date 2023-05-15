using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BinItemsAllocator : MonoBehaviour
{
    public TextMeshProUGUI[] binItemTexts = { };
    public Transform[] binItemUIs = { };
    public string[] binItems = { };
    private Canvas worldCanvas;

    private Transform truck;

    private void Start()
    {
        truck = GameObject.FindGameObjectWithTag("Player").transform;
        worldCanvas = GameObject.FindGameObjectWithTag("WorldCanvas").GetComponent<Canvas>();
    }

    public void SetBinItemTexts(string[] randomTexts)
    {
        for (int i = 0; i < 3; i++)
        {
            binItemTexts[i].text = randomTexts[i];
            binItemUIs[i].GetComponent<BinItem>().binItemType = randomTexts[i];
            binItems = randomTexts;
        }
    }

    public void CollectBin()
    {
        StartCoroutine(CollectNextBinItem());
    }


    
    public IEnumerator CollectNextBinItem()
    {
        for (int i = 0; i < 3; i++)
        {
            binItemUIs[i].SetParent(worldCanvas.transform);
            binItemUIs[i].GetComponent<BinItem>().MoveTo(truck);

            yield return new WaitForSeconds(0.35f);
        }
    }
}
