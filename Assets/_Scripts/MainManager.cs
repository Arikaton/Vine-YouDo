﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Back4appHelper _back4AppHelper;
    [SerializeField] private UIManager _uiManager;

    private void Start()
    {
        _uiManager.ShowWindow("Root Canvas");
    }
}
