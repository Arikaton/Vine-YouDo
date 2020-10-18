using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class RepositoryManager : MonoBehaviour
{
    [SerializeField] private Back4appHelper _back4AppHelper;

    private bool _grapeNeedUpdate = true;
    private bool _colorNeedUpdate = true;
    private bool _countryNeedUpdate = true;

    public void AddGrape(Text text)
    {
        if (PreparingAddData(text)) return;
        string hexColor = ColorUtility.ToHtmlStringRGB(AddToListHelper.current.color.color);
        _back4AppHelper.AddGrape(text.text,  hexColor, AddToListRepository);
        _grapeNeedUpdate = true;
    }

    public void GetGrape()
    {
        if (_grapeNeedUpdate)
            _back4AppHelper.GetGrapes(OnGetGrape);
    }

    public void OnGetGrape(CommonData grapeData)
    {
        List<CommonResult> commonResults = grapeData.Results.ConvertAll<CommonResult>(result =>
            {
                bool isFavorite = (bool?) result["IsFavorite"] == true;
                return new CommonResult(
                    result["Grape"].ToString(),
                    result["HEX"].ToString(),
                    isFavorite,
                    result["objectId"].ToString()
                );
            }
            
        );
        GetFromListHelper.Current.GenerateList(commonResults);
        _grapeNeedUpdate = false;
    }
    
    public void AddColor(Text text)
    {
        if (PreparingAddData(text)) return;
        string hexColor = ColorUtility.ToHtmlStringRGB(AddToListHelper.current.color.color);
        _back4AppHelper.AddColor(text.text, hexColor, AddToListRepository);
        _colorNeedUpdate = true;
    }
    
    public void GetColor()
    {
        if (_colorNeedUpdate)
        {
            _back4AppHelper.GetColors(OnGetColor);
        }
    }

    public void OnGetColor(CommonData commonData)
    {
        List<CommonResult> commonResults = commonData.Results.ConvertAll<CommonResult>(result =>
            {
                bool isFavorite = (bool?) result["IsFavorite"] == true;
                return new CommonResult(
                    result["Color"].ToString(),
                    result["HEX"].ToString(),
                    isFavorite,
                    result["objectId"].ToString()
                );
            }
        );
        GetFromListHelper.Current.GenerateList(commonResults);
        _colorNeedUpdate = false;
    }

    public void AddRegion(Text regionText, Text countryText)
    {
        if (PreparingAddData(regionText)) return;
        _back4AppHelper.AddRegion(regionText.text, countryText.text, AddToListRepository);
    }

    public void AddCountry(Text text)
    {
        if (PreparingAddData(text)) return;
        string hexColor = ColorUtility.ToHtmlStringRGB(AddToListHelper.current.color.color);
        _back4AppHelper.AddCountry(text.text, hexColor, AddToListRepository);
        _countryNeedUpdate = true;
    }

    public void GetCountries()
    {
        if (_countryNeedUpdate)
            _back4AppHelper.GetCountries(OnGetCountries);
    }
    
    public void OnGetCountries(CommonData commonData)
    {
        List<CommonResult> commonResults = commonData.Results.ConvertAll<CommonResult>(result =>
            {
                bool isFavorite = (bool?) result["IsFavorite"] == true;
                return new CommonResult(
                    result["Country"].ToString(),
                    result["HEX"].ToString(),
                    isFavorite,
                    result["objectId"].ToString()
                );
            }
        );
        GetFromListHelper.Current.GenerateList(commonResults);
        _countryNeedUpdate = false;
    }

    private bool PreparingAddData(Text text)
    {
        if (string.IsNullOrEmpty(text.text)) return true;
        AddToListHelper.current.StartAddingData();
        return false;
    }

    public void GetAllVines()
    {
        _back4AppHelper.GetVine(GetVineDataCallback);
    }

    public void GetColors()
    {
        _back4AppHelper.GetColors(DebugGetDataCallback);
    }

    public void UploadImage(Texture2D image)
    {
        _back4AppHelper.AddVine(image, DebugAddDataCallback);
    }

    public void GetVineDataCallback(CommonData commonData)
    {
        commonData.PrintResults();
    }

    public void AddToListRepository(AddDataCallback callback)
    {
        if (callback.ResponseCode == 201)
        {
            AddToListHelper.current.FinishedAddingData();
        }
        else
        {
            AddToListHelper.current.ErrorWhileAdding();
        }
    }
    
    void DebugGetDataCallback(CommonData data)
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
