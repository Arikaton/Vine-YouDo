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
    public static string COUNTRIES = "Countries";
    public static string REGIONS = "Regions";
    public static string COLORS = "Colors";
    public static string GRAPES = "Grapes";
    public static string VINE = "Vine";
    public delegate void OnEndAddDataCallback(AddDataCallback data);
    public delegate void OnEndGetDataCallback(CommonData data);

    [SerializeField] private string parseApplicationId;
    [SerializeField] private string restApiKey;

    public void AddColor(string color, string hex, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddColorCor(color, hex, onEndAddDataCallback));
    }

    public void GetColors(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetColorsCor(onEndGetDataCallback));
    }

    public void AddCountry(string country, string hexColor, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddCountryCor(country, hexColor, onEndAddDataCallback));
    }

    public void GetCountries(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetCountriesCor(onEndGetDataCallback));
    }

    public void AddRegion(string region, string country, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddRegionCor(region, country, onEndAddDataCallback));
    }

    public void GetRegions(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetRegionsCor(onEndGetDataCallback));
    }

    public void AddGrape(string grape, string hex, OnEndAddDataCallback onEndAddDataCallback)
    {
        StartCoroutine(AddGrapeCor(grape, hex, onEndAddDataCallback));
    }

    public void GetGrapes(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetGrapesCor(onEndGetDataCallback));
    }

    public void DeleteObject(string objectId, string objectType)
    {
        StartCoroutine(DeleteObjectCor(objectId, objectType));
    }
    
    private IEnumerator DeleteObjectCor(string objectId, string objectType)
    {
        var www = UnityWebRequest.Delete($"https://parseapi.back4app.com/classes/{objectType}/{objectId}");
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
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
        var www = UnityWebRequest.Put($"https://parseapi.back4app.com/classes/{type}/{objectId}", UTF8Encoding.UTF8.GetBytes(data));
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }

    public void GetVine(OnEndGetDataCallback onEndGetDataCallback)
    {
        StartCoroutine(GetVineCor(onEndGetDataCallback));
    }

    public void AddVine(Texture2D image, OnEndAddDataCallback onEndAddDataCallback, string color = "",  string grape = "", string country = "", string region = "")
    {
        StartCoroutine(UploadImageToServerCor(image, color, grape, country, region, onEndAddDataCallback));
    }

    IEnumerator AddColorCor(string color, string hex, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromDict(new Dictionary<string, string>() {{"Color", color}, {"HEX", hex}});
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
        
        onEndGetDataCallback(new CommonData(www.downloadHandler.text, www.responseCode));

    }
    
    IEnumerator AddCountryCor(string country, string hexColor, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromDict(new Dictionary<string, string>() {{"Country", country}, {"HEX", hexColor}});
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
        
        onEndGetDataCallback(new CommonData(www.downloadHandler.text, www.responseCode));
    }

    IEnumerator AddRegionCor(string region, string country, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromDict(new Dictionary<string, string>() {{"Region", region}, {"Country", country}});
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
        
        onEndGetDataCallback(new CommonData(www.downloadHandler.text, www.responseCode));
    }
    
    IEnumerator AddGrapeCor(string grape, string hex, OnEndAddDataCallback onEndAddDataCallback)
    {
        var data = CreateDataFromDict(new Dictionary<string, string>(){{"Grape", grape},{"HEX", hex}});
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
        
        onEndGetDataCallback(new CommonData(www.downloadHandler.text, www.responseCode));
    }

    IEnumerator GetVineCor(OnEndGetDataCallback onEndGetDataCallback)
    {
        var www = CreateRequestAndSetHeaders("https://parseapi.back4app.com/classes/" + VINE);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        
        onEndGetDataCallback(new CommonData(www.downloadHandler.text, www.responseCode));
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
        System.IO.File.WriteAllBytes( Application.dataPath + "/UnityImage.jpg", tex.EncodeToJPG());
        print(Application.dataPath);
    }

    IEnumerator UploadImageToServerCor(Texture2D image, string color, string grape, string country, string region, OnEndAddDataCallback onEndAddDataCallback)
    {
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        var imageByteData = File.ReadAllBytes(Application.dataPath + "/Sprites/Backgrounds/мОрдынка.jpg");
        data.Add(new MultipartFormFileSection(imageByteData));

        var www = new UnityWebRequest("https://parseapi.back4app.com/files/UnityImage.jpg");
        www.SetRequestHeader("X-Parse-Application-Id", parseApplicationId);
        www.SetRequestHeader("X-Parse-REST-API-Key", restApiKey);
        www.SetRequestHeader("Content-Type", "image/jpg");
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
            StartCoroutine(AddVineCor(imageData.url, imageData.name, color, grape, country, region,
                onEndAddDataCallback));
        }
    }

    private string CreateDataFromDict(Dictionary<string, string> query)
    {
        return JsonConvert.SerializeObject(query);
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

public class CommonData
{
    public long ResponseCode { get; private set; }
    public string Json { get; private set; }
    public List<JToken> Results { get; private set; }

    public CommonData(string json, long responseCode)
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
