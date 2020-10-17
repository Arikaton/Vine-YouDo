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
    [SerializeField] private GameObject takePhotoButton;
    [SerializeField] private GameObject confirmButtons;

    public Texture2D lastSavedPhoto;

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        webCamTexture.Stop();
    }

    public void EnableCamera() 
    {
        if (webCamTexture == null)
        {
            webCamTexture = new WebCamTexture();
            devices = WebCamTexture.devices;
        }

        image.texture = webCamTexture; 
        webCamTexture.Play();
    }

    public void SavePhoto()
    {
        StartCoroutine(SavePhotoCor());
        takePhotoButton.SetActive(false);
        confirmButtons.SetActive(true);
    }

    public void Reset()
    {
        EnableCamera();
        takePhotoButton.SetActive(true);
        confirmButtons.SetActive(false);
    }

    IEnumerator SavePhotoCor()  // Start this Coroutine on some button click
    {
        yield return new WaitForEndOfFrame();

        lastSavedPhoto = new Texture2D(webCamTexture.width, webCamTexture.height);
        lastSavedPhoto.SetPixels(webCamTexture.GetPixels());
        lastSavedPhoto.Apply();
        webCamTexture.Stop();
        image.texture = lastSavedPhoto;

        //Encode to a PNG and save to file
        /*byte[] bytes = lastSavedPhoto.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        string path = Application.streamingAssetsPath + "/" + Guid.NewGuid() + "photo.png";
        print(path);
        File.WriteAllBytes(path, bytes);*/
    }
}
