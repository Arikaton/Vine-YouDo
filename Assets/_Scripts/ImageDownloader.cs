using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageDownloader : MonoBehaviour
{
    public static ImageDownloader main;

    private bool isDownloading = false;

    private void Awake()
    {
        main = this;
    }

    public IEnumerator DownloadImageCor(List<VineData> vineList, Text infoText)
    {
        var uploadImageCount = 0;
        foreach (var vineData in vineList)
        {
            if (!File.Exists(GetPathFromImageUrl(vineData.Image["url"])))
            {
                var www = UnityWebRequestTexture.GetTexture(vineData.Image["url"]);
                yield return www.SendWebRequest();
                var texture2D = DownloadHandlerTexture.GetContent(www);
                www.Dispose();
                SaveImageLocal(texture2D, vineData.Image["url"]);
            }
            uploadImageCount++;
            infoText.text = $"Загружаем изображения {uploadImageCount}/{vineList.Count}";
        }
    }

    public static void SaveImageLocal(Texture2D texture2D, string prefKey)
    {
        var path = GetPathFromImageUrl(prefKey);
        TextureScale.Scale(texture2D, 512, 512);
        File.WriteAllBytes(path, texture2D.EncodeToJPG());
        //PlayerPrefs.SetString(prefKey, path);
        Resources.UnloadUnusedAssets();
    }

    public static string GetPathFromImageUrl(string url)
    {
        return Application.persistentDataPath +  $"/{url.Replace("https://parsefiles.back4app.com/", "").Split('/')[1]}";
    }
    
    public static Texture2D LoadTexture2D(string filePath)
    {
        Texture2D tex;
        byte[] fileData;
 
        if (File.Exists(filePath))     {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else
        {
            throw new Exception("File not exists\nFolder: " + filePath);
        }
        return tex;
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
