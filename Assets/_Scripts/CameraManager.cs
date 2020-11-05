using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AddVineManager _addVineManager;
    [SerializeField] GameObject cellarWindow;
    
    public void TakePhoto()
    {
        NativeCamera.TakePicture(OnTakePicture, 512);
    }

    private void OnTakePicture(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            _addVineManager.SetImage(NativeCamera.LoadImageAtPath(path), path);
            _uiManager.ShowWindow(cellarWindow);
        }
    }

    public void PickFromGallery()
    {
        NativeGallery.GetImageFromGallery(OnTakePicture);
    }
}
