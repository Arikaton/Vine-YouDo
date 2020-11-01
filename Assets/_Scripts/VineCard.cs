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

    private VineData _vineData;
    private Texture2D _texture2D;

    public void Init(VineData vineData)
    {
        _vineData = vineData;
        var imagePath = PlayerPrefs.GetString(_vineData.Image["url"]);
        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            _texture2D = NativeGallery.LoadImageAtPath(imagePath);
            _image.texture = _texture2D;
            downloadAnim.SetActive(false);
        }
        else
        {
            StartCoroutine(DownloadImage());
        }
    }

    IEnumerator DownloadImage()
    {
        var www = UnityWebRequestTexture.GetTexture(_vineData.Image["url"]);
        yield return www.SendWebRequest();
        _texture2D = DownloadHandlerTexture.GetContent(www);
        var guid = Guid.NewGuid().ToString();
        var path = Application.persistentDataPath + $"/{guid}.jpeg";
        File.WriteAllBytes(path, _texture2D.EncodeToJPG());
        PlayerPrefs.SetString(_vineData.Image["url"], path);

#if UNITY_IOS
        _image.texture = _texture2D;
#else
        _texture2D = RotateTexture(_texture2D, true);
        _image.texture = _texture2D;
#endif
        downloadAnim.SetActive(false);
    }

    public void ShowCardView()
    {
        VineView.main.SetData(_vineData, _texture2D, gameObject);
        UIManager.Main.ShowVineView();
    }

    public void UpdateCount(int count)
    {
        _vineData.Count = count;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowCardView();
    }
    
    public static Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;
 
        int iRotated, iOriginal;
 
        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }
 
        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}
