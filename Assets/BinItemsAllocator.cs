using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BinItemsAllocator : MonoBehaviour
{
    public TextMeshProUGUI[] binItemTexts = { };
    public void SetBinItemTexts(string[] randomTexts)
    {
        for (int i = 0; i < 3; i++)
        {
            binItemTexts[i].text = randomTexts[i];
        }
    }
}
