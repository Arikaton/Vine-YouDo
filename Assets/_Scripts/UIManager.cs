using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private WindowManager[] windows;

    private void Start()
    {
        windows = FindObjectsOfType<WindowManager>();
    }

    public void ShowWindow(string name)
    {
        bool haveOne = false;
        foreach (var window in windows)
        {
            if (window.WindowName == name)
            {
                haveOne = true;
                window.gameObject.SetActive(true);
            }
            else
            {
                window.gameObject.SetActive(false);
            }
        }

        if (!haveOne)
        {
            print($"Can't find window with name {name}.");
        }
    }
}
