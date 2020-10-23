using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AddVineManager _addVineManager;
    [SerializeField] private RawImage image;
    [SerializeField] GameObject cameraCanvas;
    
    private Texture2D _newPhoto;

    public void TakePhoto()
    {
        NativeCamera.TakePicture(OnTakePicture, 512);
    }

    private void OnTakePicture(string path)
    {
        if (!string.IsNullOrEmpty(path) && cameraCanvas != image.transform.parent.gameObject)
            _uiManager.ShowWindow(cameraCanvas);
        _newPhoto = NativeCamera.LoadImageAtPath(path);
        image.texture = _newPhoto;
    }

    public void Save()
    {
        if (_newPhoto != null)
        {
            _addVineManager.SetImage(_newPhoto, _newPhoto.);

        }
        else
        {
            throw new Exception("Photo is null");
        }
    }
}
