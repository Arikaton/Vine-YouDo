using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepositoryManager : MonoBehaviour
{
    [SerializeField] private Back4appHelper _back4AppHelper;

    private bool isDownloading = false;

    public void AddGrape(Text text)
    {
        if (PreparingAddData(text)) return;
        _back4AppHelper.AddGrape(text.text, AddToListRepository);

    }
    
    public void AddColor(Text text)
    {
        if (PreparingAddData(text)) return;
        _back4AppHelper.AddColor(text.text, AddToListRepository);
    }

    public void AddRegion(Text regionText, Text countryText)
    {
        if (PreparingAddData(regionText)) return;
        _back4AppHelper.AddRegion(regionText.text, countryText.text, AddToListRepository);
    }

    public void AddCountry(Text text)
    {
        if (PreparingAddData(text)) return;
        _back4AppHelper.AddCountry(text.text, AddToListRepository);
    }

    private bool PreparingAddData(Text text)
    {
        if (isDownloading) return true;
        if (string.IsNullOrEmpty(text.text)) return true;
        AddToListHelper.current.StartAddingData();
        isDownloading = true;
        return false;
    }

    public void GetColors()
    {
        _back4AppHelper.GetColors(DebugGetDataCallback);
    }

    public void UploadImage(Texture2D image)
    {
        _back4AppHelper.AddVine(image, DebugAddDataCallback);
    }

    public void AddToListRepository(AddDataCallback callback)
    {
        isDownloading = false;
        if (callback.ResponseCode == 201)
        {
            AddToListHelper.current.FinishedAddingData();
        }
        else
        {
            AddToListHelper.current.ErrorWhileAdding();
        }
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
