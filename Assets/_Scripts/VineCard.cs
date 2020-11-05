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
    private string imagePath;

    private bool _isInited = false;

    public VineData VineData { get; private set; }
    private Texture2D _texture2D;

    private bool imageLoaded = false;

    public void Init(VineData vineData)
    {
        VineData = vineData;
        imagePath = PlayerPrefs.GetString(VineData.Image["url"]);
        StartCoroutine(DelayedInit());
    }

    IEnumerator DelayedInit()
    {
        yield return new WaitForSeconds(0.5f);
        _isInited = true;
        if (IsVisible())
            LoadImage();
    }

    private void LoadImage()
    {
        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            _texture2D = ImageDownloader.LoadTexture2D(imagePath);
            _image.texture = _texture2D;
            downloadAnim.SetActive(false);
        }
        imageLoaded = true;
    }

    public void SetImage(Texture2D texture)
    {
        _texture2D = texture;
        _image.texture = _texture2D;
        downloadAnim.SetActive(false);
    }

    bool IsVisible()
    {
        return transform.position.y < ScrollExtension.main.topBorder.position.y &&
               transform.position.y > ScrollExtension.main.bottomBorder.position.y;
    }

    public void ShowCardView()
    {
        VineView.main.SetData(VineData, _texture2D, gameObject);
        UIManager.Main.ShowVineView();
    }

    private void Update()
    {
        if (!_isInited) return;
        if (!IsVisible() && imageLoaded)
        {
            Destroy(_image.texture);
            imageLoaded = false;
        }
        else if (IsVisible() && !imageLoaded)
        {
            LoadImage();
            imageLoaded = true;
        }
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
