using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class VineList : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Transform content;
    [SerializeField] private VineCard vineCardPrefab;

    public void Init(string year, List<VineData> vineDatas)
    {
        text.text = year == "0" ? "Non Vintage" : year;
        foreach (var vineData in vineDatas)
        {
            var vineCard = Instantiate(vineCardPrefab, content);
            vineCard.Init(vineData);
        }
    }
}
