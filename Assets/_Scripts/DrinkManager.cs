using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject filterWindow;
    [SerializeField] Text filterWindowName;

    public void ShowFilterWindow(string cellarName)
    {
        _uiManager.ShowWindow(filterWindow);
        filterWindowName.text = cellarName;
    }
}
