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
        NativeCamera.TakePicture(OnTakePictureFromCamera, 512);
    }

    private void OnTakePictureFromCamera(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            var tex = ImageDownloader.LoadTexture2D(path);
            NativeGallery.SaveImageToGallery(tex, "WineImages", "WineImage.jpg");
            TextureScale.Scale(tex, 512, 512);
            _addVineManager.SetImage(tex, path);
            _uiManager.ShowWindow(cellarWindow);
        }
    }
    
    private void OnTakePictureFromGallery(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            var tex = ImageDownloader.LoadTexture2D(path);
            TextureScale.Scale(tex, 512, 512);
            _addVineManager.SetImage(tex, path);
            _uiManager.ShowWindow(cellarWindow);
        }
    }

    public void PickFromGallery()
    {
        NativeGallery.GetImageFromGallery(OnTakePictureFromGallery);
    }
}
