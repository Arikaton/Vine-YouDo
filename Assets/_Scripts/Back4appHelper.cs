using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Back4appHelper : MonoBehaviour
{
    public static string COUNTRIES = "Countries";
    public static string REGIONS = "Regions";
    public static string COLORS = "Colors";
    public static string GRAPES = "Grapes";
    public static string VINE = "Vine";
    
    [SerializeField] private string parseApplicationId;
    [SerializeField] private string restApiKey;
    [SerializeField] private Texture2D testImage;

    private string testImageUrl =
        "https://parsefiles.back4app.com/3wnDQP64MYfQ9VRJub2SFyepUpPxw0bkK1YKIzeI/2f50e0f41d28ab65d2cee56677432013_TestPicture.jpg";

    private string testImageName = "2f50e0f41d28ab65d2cee56677432013_TestPicture.jpg";

    private void Start()
    {
        StartCoroutine(AddVine(testImageUrl, testImageName, color: "Test"));
    }

    IEnumerator AddColor(string color)
    {
        var data = CreateDataFromString("Color", color);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COLORS);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));

        yield return www.SendWebRequest();
    }

    IEnumerator GetColors()
    {
        UnityWebRequest www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COLORS);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        print(www.downloadHandler.text);

    }
    
    IEnumerator AddCountry(string country)
    {
        var data = CreateDataFromString("Country", country);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COUNTRIES);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));

        yield return www.SendWebRequest();
    }
    
    IEnumerator GetCountries(string country)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COUNTRIES);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        print(www.downloadHandler.text);
    }

    IEnumerator AddRegion(string region)
    {
        var data = CreateDataFromString("Region", region);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + REGIONS);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));

        yield return www.SendWebRequest();
    }
    
    IEnumerator GetRegions(string country)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + REGIONS);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        print(www.downloadHandler.text);
    }
    
    IEnumerator AddGrape(string grape)
    {
        var data = CreateDataFromString("Grape", grape);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + GRAPES);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));

        yield return www.SendWebRequest();
    }
    
    IEnumerator GetGrapes(string grape)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + GRAPES);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        print(www.downloadHandler.text);
    }
    
    IEnumerator AddVine(string imageUrl, string imageName, string color = "",  string grape = "", string country = "", string region = "")
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        if (color != "")
            data.Add("Color", color);
        if (grape != "")
            data.Add("Grape", grape);
        if (country != "")
            data.Add("Country", country);
        if (region != "")
            data.Add("Region", region);

        var imageData = string.Format("{{\"name\":\"{0}\",\"url\":\"{1}\",\"__type\":\"File\"}}", imageName, imageUrl);
            /*CreateDataFromDict(new Dictionary<string, string>()
        {
            {"name", imageName},
            {"url", imageUrl},
            {"__type", "File"}
        });*/
        data.Add("Image", imageData);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + VINE);
        www.method = "POST";
        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(CreateDataFromDict(data)));
        print(CreateDataFromDict(data));

        yield return www.SendWebRequest();

        if (www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print(www.downloadHandler.text);
        }
    }

    IEnumerator UploadImageToServer(string fileName, Texture2D image)
    {
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        data.Add(new MultipartFormFileSection(image.EncodeToJPG()));
        //WWWForm form = new WWWForm();
        //form.AddBinaryData("Image", image.EncodeToPNG(), fileName, "image/png");
        
        var www = UnityWebRequest.Post("https://parseapi.back4app.com/files/" + fileName + ".jpg", data);
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        www.SetRequestHeader("Content-Type", "image/jpeg");
        
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print(www.downloadHandler.text);
        }
    }

    private string CreateDataFromDict(Dictionary<string, string> query)
    {
        string result = "{";
        bool isFirst = true;
        foreach (var key in  query.Keys)
        {
            if (isFirst)
            {
                isFirst = false;
                result += string.Format("\"{0}\":\"{1}\"", key, query[key]);
            }
            else
            {
                if (query[key].StartsWith("{"))
                {
                    result += string.Format(",\"{0}\":{1}", key, query[key]);
                }
                else
                {
                    result += string.Format(",\"{0}\":\"{1}\"", key, query[key]);
                }
            }
        }
        result += "}";
        return result;
    }

    private string CreateDataFromString(string key, string value)
    {
        return string.Format("{{\"{0}\":\"{1}\"}}", key, value);
    }

    private UnityWebRequest CreateRequestAndSetHeaders(string url)
    {
        UnityWebRequest www = new UnityWebRequest(url);
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
        return www;
    }
}
