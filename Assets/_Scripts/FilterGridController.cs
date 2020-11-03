using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterGridController : MonoBehaviour
{
    [SerializeField] private RectTransform scrollRect;

    private void OnEnable()
    {
        if (scrollRect.rect.height < 120)
        {
            Debug.Log(scrollRect.rect.height);
            GetComponent<GridLayoutGroup>().constraintCount = 1;
        }
    }
}
