using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Main;
    //Stack<GameObject> windowsStack = new Stack<GameObject>();
    
    [SerializeField] private GameObject prevWindow;
    [SerializeField] private GameObject vineView;
    public GameObject addVineMain;

    private void Awake()
    {
        Main = this;
        //windowsStack.Push(prevWindow);
    }

    public void ShowWindow(GameObject window)
    {
        //windowsStack.Push(window);
        window.SetActive(true);
        prevWindow.SetActive(false);
        prevWindow = window;
    }

    /*public void GoBack()
    {
        //windowsStack.Pop();
        //ShowWindow(windowsStack.Peek());
    }*/

    public void ShowVineView()
    {
        ShowWindow(vineView);
    }
    
}
