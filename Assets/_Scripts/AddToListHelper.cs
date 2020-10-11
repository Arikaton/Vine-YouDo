using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToListHelper : MonoBehaviour
{
    public static AddToListHelper current;
    
    [SerializeField] private GameObject downloadImage;
    [SerializeField] private GameObject successText;
    [SerializeField] private GameObject errorText;
    [SerializeField] private Image color;

    private bool isLoading = false;

    private void OnEnable()
    {
        current = this;
        downloadImage.SetActive(false);
        successText.SetActive(false);
        errorText.SetActive(false);
    }

    public void StartAddingData()
    {
        downloadImage.SetActive(true);
        successText.SetActive(false);
        errorText.SetActive(false);
    }

    public void FinishedAddingData()
    {
        downloadImage.SetActive(false);
        successText.SetActive(true);
    }

    public void ErrorWhileAdding()
    {
        downloadImage.SetActive(false);
        errorText.SetActive(true);
    }
}
