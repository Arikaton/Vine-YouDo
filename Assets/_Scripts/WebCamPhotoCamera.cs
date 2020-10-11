using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebCamPhotoCamera : MonoBehaviour 
{
    WebCamTexture webCamTexture;
    private WebCamDevice[] devices;
    public RawImage image;

    private bool isActive = false;
    private void OnEnable()
    {
        print("Enable Camera");
        EnableCamera();
    }

    private void OnDisable()
    {
        webCamTexture.Stop();
    }

    public void SwitchCam()
    {
        if (devices.Length > 1)
        {
            webCamTexture.deviceName = webCamTexture.deviceName == devices[0].name ? devices[1].name : devices[0].name;
            webCamTexture.Play();
        }
    }

    public void EnableCamera() 
    {
        if (webCamTexture == null)
        {
            webCamTexture = new WebCamTexture();
            devices = WebCamTexture.devices;
        }
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(webCamTexture.width, webCamTexture.height);
        image.texture = webCamTexture; 
        webCamTexture.Play();
        isActive = true;
    }
    
    private void Update()
    {
        if (isActive)
            image.texture = webCamTexture;
    }

    public void SavePhoto()
    {
        StartCoroutine(SavePhotoCor());
        isActive = false;
    }

    IEnumerator SavePhotoCor()  // Start this Coroutine on some button click
    {
        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        image.texture = photo;

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        string path = Application.streamingAssetsPath + "/" + Guid.NewGuid() + "photo.png";
        print(path);
        File.WriteAllBytes(path, bytes);
    }
}
