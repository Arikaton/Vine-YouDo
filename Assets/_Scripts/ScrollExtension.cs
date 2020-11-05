using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ScrollExtension : MonoBehaviour
{
    public static ScrollExtension main;
    
    public RectTransform bottomBorder;
    public RectTransform topBorder;

    private void Awake()
    {
        main = this;
    }
}
