using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private RawImage image;
    [SerializeField] GameObject cameraCanvas;
    
    private Texture2D _newPhoto;

    public void TakePhoto()
    {
        NativeCamera.TakePicture(OnTakePicture, 512);
    }

    private void OnTakePicture(string path)
    {
        if (!string.IsNullOrEmpty(path))
            _uiManager.ShowWindow(cameraCanvas);
        _newPhoto = NativeCamera.LoadImageAtPath(path);
        image.texture = _newPhoto;
    }
}
