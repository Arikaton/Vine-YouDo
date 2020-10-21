using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Main;
    
    [SerializeField] private GameObject prevWindow;
    public GameObject addVineMain;

    private void Awake()
    {
        Main = this;
    }

    public void ShowWindow(GameObject window)
    {
        window.SetActive(true);
        prevWindow.SetActive(false);
        prevWindow = window;
    }
    
}
