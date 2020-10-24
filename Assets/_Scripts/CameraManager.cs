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
    private string _path;

    public void TakePhoto()
    {
        NativeCamera.TakePicture(OnTakePicture, 1000);
    }

    private void OnTakePicture(string path)
    {
        _newPhoto = NativeCamera.LoadImageAtPath(path);
        _path = path;
        image.texture = _newPhoto;
        if (!string.IsNullOrEmpty(path) && cameraCanvas != image.transform.parent.gameObject)
            _uiManager.ShowWindow(cameraCanvas);
    }

    public void Save()
    {
        if (_newPhoto != null)
        {
            _addVineManager.SetImage(_newPhoto, _path);

        }
        else
        {
            throw new Exception("Photo is null");
        }
    }
}
