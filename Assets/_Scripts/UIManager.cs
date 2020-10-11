using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject prevWindow;
    
    public void ShowWindow(GameObject window)
    {
        window.SetActive(true);
        prevWindow.SetActive(false);
        prevWindow = window;
    }
}
