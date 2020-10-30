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
            GetComponent<GridLayoutGroup>().constraintCount = 1;
        }
    }
}
