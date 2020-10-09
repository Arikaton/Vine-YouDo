using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Back4appHelper _back4AppHelper;

    public void AddColor(string color)
    {
        _back4AppHelper.AddColor(color, DebugAddDataCallback);
    }

    public void GetColors()
    {
        _back4AppHelper.GetColors(DebugGetDataCallback);
    }

    public void UploadImage(Texture2D image)
    {
        _back4AppHelper.AddVine(image, DebugAddDataCallback);
    }

    void DebugGetDataCallback(GetDataCallback data)
    {
     print(data.ResponseCode);
     data.PrintResults();
    }

    private void DebugAddDataCallback(AddDataCallback data)
    {
        print(data.Json);
        print(data.ResponseCode);
        print(data.ObjectId);
        print(data.CreatedAt);
    }
}
