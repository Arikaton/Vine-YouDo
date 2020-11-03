using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VineCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RawImage _image;
    [SerializeField] private GameObject downloadAnim;

    public VineData VineData { get; private set; }
    private Texture2D _texture2D;

    public void Init(VineData vineData)
    {
        VineData = vineData;
        var imagePath = PlayerPrefs.GetString(VineData.Image["url"]);
        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            Debug.Log(imagePath);
            _texture2D = ImageDownloader.LoadTexture2D(imagePath);
            _image.texture = _texture2D;
            downloadAnim.SetActive(false);
        }
        else
        {
            ImageDownloader.main.DownloadImage(this);
        }
    }

    public void SetImage(Texture2D texture)
    {
        _texture2D = texture;
        _image.texture = _texture2D;
        downloadAnim.SetActive(false);
    }

    public void ShowCardView()
    {
        VineView.main.SetData(VineData, _texture2D, gameObject);
        UIManager.Main.ShowVineView();
    }

    public void UpdateCount(int count)
    {
        VineData.Count = count;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowCardView();
    }
}
