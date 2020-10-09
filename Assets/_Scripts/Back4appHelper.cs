﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class Back4appHelper : MonoBehaviour
{
    public static string COUNTRIES = "Countries";
    public static string REGIONS = "Regions";
    public static string COLORS = "Colors";
    public static string GRAPES = "Grapes";
    public static string VINE = "Vine";
    public delegate void OnEndAddDataCallback(AddDataCallback data);
    public delegate void OnEndGetDataCallback(GetDataCallback data);

    [SerializeField] private string parseApplicationId;
    [SerializeField] private string restApiKey;
    [SerializeField] private UnityEngine.UI.Image testImage;

    //private string testImageUrl =
    //    "https://parsefiles.back4app.com/3wnDQP64MYfQ9VRJub2SFyepUpPxw0bkK1YKIzeI/2f50e0f41d28ab65d2cee56677432013_TestPicture.jpg";
    //private string testImageName = "2f50e0f41d28ab65d2cee56677432013_TestPicture.jpg";

    private void Start()
    {
        //StartCoroutine(DownloadImageFromServerCor());
    }

    public void AddColor(string color, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddColorCor(color, onEndAddDataCallback));
    }

    public void GetColors(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetColorsCor(onEndGetDataCallback));
    }

    public void AddCountry(string country, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddCountryCor(country, onEndAddDataCallback));
    }

    public void GetCountries(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetCountriesCor(onEndGetDataCallback));
    }

    public void AddRegion(string region, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddRegionCor(region, onEndAddDataCallback));
    }

    public void GetRegions(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetRegionsCor(onEndGetDataCallback));
    }

    public void AddGrape(string grape, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddGrapeCor(grape, onEndAddDataCallback));
    }

    public void GetGrapes(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetGrapesCor(onEndGetDataCallback));
    }

    public void AddVine(Texture2D image, OnEndAddDataCallback onEndAddDataCallback, string color = "",  string grape = "", string country = "", string region = "")
    {
        StartCoroutine(UploadImageToServerCor(image, color, grape, country, region, onEndAddDataCallback));
    }

    IEnumerator AddColorCor(string color, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromString("Color", color);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COLORS);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndAddDataCallback(new AddDataCallback(www.downloadHandler.text, www.responseCode));
    }

    IEnumerator GetColorsCor(OnEndGetDataCallback onEndGetDataCallback)
    {
        UnityWebRequest www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COLORS);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        print(www.downloadHandler.text);
        
        onEndGetDataCallback(new GetDataCallback(www.downloadHandler.text, www.responseCode));

    }
    
    IEnumerator AddCountryCor(string country, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromString("Country", country);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COUNTRIES);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndAddDataCallback(new AddDataCallback(www.downloadHandler.text, www.responseCode));
    }
    
    IEnumerator GetCountriesCor(OnEndGetDataCallback onEndGetDataCallback)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + COUNTRIES);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndGetDataCallback(new GetDataCallback(www.downloadHandler.text, www.responseCode));
    }

    IEnumerator AddRegionCor(string region, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromString("Region", region);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + REGIONS);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndAddDataCallback(new AddDataCallback(www.downloadHandler.text, www.responseCode));
    }
    
    IEnumerator GetRegionsCor(OnEndGetDataCallback onEndGetDataCallback)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + REGIONS);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndGetDataCallback(new GetDataCallback(www.downloadHandler.text, www.responseCode));
    }
    
    IEnumerator AddGrapeCor(string grape, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromString("Grape", grape);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + GRAPES);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(data));
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndAddDataCallback(new AddDataCallback(www.downloadHandler.text, www.responseCode));
    }
    
    IEnumerator GetGrapesCor(OnEndGetDataCallback onEndGetDataCallback)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + GRAPES);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndGetDataCallback(new GetDataCallback(www.downloadHandler.text, www.responseCode));
    }
    
    IEnumerator AddVineCor(string imageUrl, string imageName, string color,  string grape, string country, string region, OnEndAddDataCallback onEndAddDataCallback)
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
        
        data.Add("Image", imageData);
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + VINE);
        www.method = "POST";
        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler = new UploadHandlerRaw(UTF8Encoding.UTF8.GetBytes(CreateDataFromDict(data)));
        //print(CreateDataFromDict(data));

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);
        onEndAddDataCallback(new AddDataCallback(www.downloadHandler.text, www.responseCode));
    }

    IEnumerator DownloadImageFromServerCor(string url)
    {
        var www = UnityWebRequestTexture.GetTexture(url);
        //ww.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        Texture2D tex = DownloadHandlerTexture.GetContent(www);
        testImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        System.IO.File.WriteAllBytes( Application.dataPath + "/UnityImage.jpg", tex.EncodeToJPG());
        print(Application.dataPath);
    }

    IEnumerator UploadImageToServerCor(Texture2D image, string color, string grape, string country, string region, OnEndAddDataCallback onEndAddDataCallback)
    {
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        /*yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();*/
        data.Add(new MultipartFormFileSection(image.EncodeToJPG()));

        WWWForm form = new WWWForm();
        //form.AddField("", "UnityImage.png",);
        form.AddBinaryData("File", image.EncodeToJPG(), "UnityImage.jpg", "image/jpeg");
        
        //System.IO.File.WriteAllBytes(Application.persistentDataPath + "/testImage.png", image.EncodeToPNG());
        //print(Application.persistentDataPath);


        var www = UnityWebRequest.Post("https://parseapi.back4app.com/files/UnityImage.jpg", form);
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        //www.SetRequestHeader("Content-Type", "image/jpg");
        
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (!www.isHttpError)
        {
            UploadImageData imageData = JsonConvert.DeserializeObject<UploadImageData>(www.downloadHandler.text);
            StartCoroutine(DownloadImageFromServerCor(imageData.url));
            print(imageData.name);
            print(imageData.url);
            StartCoroutine(AddVineCor(imageData.url, imageData.name, color, grape, country, region,
                onEndAddDataCallback));
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
                if (query[key].StartsWith("{"))
                    result += string.Format("\"{0}\":{1}", key, query[key]);
                else 
                    result += string.Format("\"{0}\":\"{1}\"", key, query[key]);
            }
            else
            {
                if (query[key].StartsWith("{"))
                    result += string.Format(",\"{0}\":{1}", key, query[key]);
                else
                    result += string.Format(",\"{0}\":\"{1}\"", key, query[key]);
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

public class AddDataCallback
{
    public long ResponseCode
    {
        get;
        private set;
    }
    public string Json
    {
        get;
        private set;
    }
    
    public string ObjectId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public AddDataCallback(string json, long responseCode)
    {
        Json = json;
        ResponseCode = responseCode;
        ParseJson(json);
    }

    private void ParseJson(string json)
    {
        JObject jObject = JObject.Parse(json);
        ObjectId = jObject["objectId"].ToString();
        CreatedAt = DateTime.Parse(jObject["createdAt"].ToString());
    }
}

public class GetDataCallback
{
    public long ResponseCode { get; private set; }
    public string Json { get; private set; }
    public List<JToken> Results { get; private set; }

    public GetDataCallback(string json, long responseCode)
    {
        ResponseCode = responseCode;
        Json = json;
        Results = JObject.Parse(json)["results"].Children().ToList();
    }

    public void PrintResults()
    {
        foreach (var result in  Results)
        {
            Debug.Log(result.ToString());
        }
    }
}

public class UploadImageData
{
    [JsonProperty]
    public string url;
    [JsonProperty]
    public string name;

    [Newtonsoft.Json.JsonConstructor]
    public UploadImageData(string url, string name)
    {
        this.url = url;
        this.name = name;
    }
}

/*public class DataPiece
{
    public string ObjectId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdateAt { get; private set; }
}*/