using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using _Scripts;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class Back4appHelper : MonoBehaviour
{
    public static string COUNTRIES_CLASS = "Countries";
    public static string COUNTRIES_FIELD = "Country";
    public static string REGIONS_CLASS = "Regions";
    public static string REGION_FIELD = "Region";
    public static string COLORS_CLASS = "Colors";
    public static string COLOR_FIELD = "Color";
    public static string GRAPES_CLASS = "Grapes";
    public static string GRAPES_FIELD = "Grape";
    public static string VINE_CLASS = "Vine";
    public static string HEX = "HEX";
    public delegate void OnEndAddDataCallback(AddDataResult data);
    public delegate void OnEndGetDataCallback(GetDataResult dataResult);

    [SerializeField] private string parseApplicationId;
    [SerializeField] private string restApiKey;

    public void AddData(string type, Dictionary<string, object> data, OnEndAddDataCallback callback)
    {
        StartCoroutine(AddDataCor(type, data, callback));
    }

    private IEnumerator AddDataCor(string type, Dictionary<string, object> data, OnEndAddDataCallback callback)
    {
        var stringData = JsonConvert.SerializeObject(data);
        Debug.Log(stringData + "\n" + "https://parseapi.back4app.com/classes/" + type);
        UnityWebRequest www = new UnityWebRequest("https://parseapi.back4app.com/classes/" + type);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(stringData));
        SetHeaders(www);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        Debug.Log(www.downloadHandler.text);
        callback(new AddDataResult(www.downloadHandler.text, www.responseCode));
    }

    public void GetData(string type, OnEndGetDataCallback callback)
    {
        StartCoroutine(GetDataCor(type, callback));
    }

    private IEnumerator GetDataCor(string type, OnEndGetDataCallback callback)
    {
        var www = UnityWebRequest.Get("https://parseapi.back4app.com/classes/" + type);
        SetHeaders(www);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        callback(new GetDataResult(www.downloadHandler.text, www.responseCode));
    }

    public void DeleteObject(string objectId, string objectType)
    {
        StartCoroutine(DeleteObjectCor(objectId, objectType));
    }
    
    private IEnumerator DeleteObjectCor(string objectId, string objectType)
    {
        var www = UnityWebRequest.Delete($"https://parseapi.back4app.com/classes/{objectType}/{objectId}");
        SetHeaders(www);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
    }

    public void UpdateFavorite(bool isFavorite, string objectId, string type)
    {
        StartCoroutine(UpdateFavoriteCor(isFavorite, objectId, type));
    }

    private IEnumerator UpdateFavoriteCor(bool isFavorite, string objectId, string type)
    {
        var data = JsonConvert.SerializeObject(new Dictionary<string, bool>() {{"IsFavorite", isFavorite}});
        var www = UnityWebRequest.Put($"https://parseapi.back4app.com/classes/{type}/{objectId}", Encoding.UTF8.GetBytes(data));
        SetHeaders(www);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }

    private void SetHeaders(UnityWebRequest www)
    {
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
    }

    public void AddVine(Texture2D image, OnEndAddDataCallback onEndAddDataCallback, string color = "",  string grape = "", string country = "", string region = "")
    {
        StartCoroutine(UploadImageToServerCor(image, color, grape, country, region, onEndAddDataCallback));
    }

    IEnumerator DownloadImageFromServerCor(string url)
    {
        var www = UnityWebRequestTexture.GetTexture(url);
        //ww.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        Texture2D tex = DownloadHandlerTexture.GetContent(www);
        System.IO.File.WriteAllBytes( Application.dataPath + "/UnityImage.jpg", tex.EncodeToJPG());
        print(Application.dataPath);
    }

    IEnumerator UploadImageToServerCor(Texture2D image, string color, string grape, string country, string region, OnEndAddDataCallback onEndAddDataCallback)
    {
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        var imageByteData = File.ReadAllBytes(Application.dataPath + "/Sprites/Backgrounds/мОрдынка.jpg");
        data.Add(new MultipartFormFileSection(imageByteData));

        var www = new UnityWebRequest("https://parseapi.back4app.com/files/UnityImage.jpg");
        SetHeaders(www);
        www.method = "POST";
        www.uploadHandler = new UploadHandlerFile(Application.dataPath + "/Sprites/Backgrounds/мОрдынка.jpg");
        
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (!www.isHttpError)
        {
            UploadImageData imageData = JsonConvert.DeserializeObject<UploadImageData>(www.downloadHandler.text);
            StartCoroutine(DownloadImageFromServerCor(imageData.url));
            print(imageData.name);
            print(imageData.url);
            /*StartCoroutine(AddVineCor(imageData.url, imageData.name, color, grape, country, region,
                onEndAddDataCallback));*/
        }
    }
}

public class AddDataResult
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

    public AddDataResult(string json, long responseCode)
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

public class GetDataResult
{
    public long ResponseCode { get; private set; }
    public string Json { get; private set; }
    public List<JToken> Results { get; private set; }

    public string resultsString;

    public GetDataResult(string json, long responseCode)
    {
        ResponseCode = responseCode;
        Json = json;
        resultsString = JObject.Parse(json)["results"].ToString();
        Results = JObject.Parse(json)["results"].Children().ToList();
    }

    public GetDataResult(string resultsString)
    {
        this.resultsString = resultsString;
        ResponseCode = 200;
    }

    public void PrintResults()
    {
        foreach (var result in  Results)
        {
            Debug.Log(result.ToString());
        }
    }
}

public class CommonResult
{
    public string name;
    public string hexColor;
    public bool isFavorite;
    public string objectId;

    public CommonResult(string name, string hexColor, bool isFavorite, string objectId)
    {
        this.name = name;
        this.hexColor = hexColor;
        this.isFavorite = isFavorite;
        this.objectId = objectId;
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

class VineData
{
    public String objectId;
    public string Color;
    public string Grape;
    public string Region;
    public string Country;
    public string Description;
}
