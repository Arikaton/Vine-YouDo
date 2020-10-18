using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class RepositoryManager : MonoBehaviour
{
    public static bool _grapeNeedUpdate = true;
    public static bool _colorNeedUpdate = true;
    public static bool _countryNeedUpdate = true;
    public static bool _regionNeedUpdate = true;

    public static string grapeData;
    public static string colorData;
    public static string countryData;
    public static string regionData;

    /*public void GetGrape()
    {
        if (_grapeNeedUpdate)
            _back4AppHelper.GetData(Back4appHelper.GRAPES_CLASS, OnGetGrape);
    }

    private void OnGetGrape(GetDataResult grapeDataResult)
    {
        List<CommonResult> commonResults = grapeDataResult.Results.ConvertAll<CommonResult>(result =>
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
    
    public void GetColor()
    {
        if (_colorNeedUpdate)
        {
            _back4AppHelper.GetData(Back4appHelper.COLORS_CLASS, OnGetColor);
        }
    }

    private void OnGetColor(GetDataResult getDataResult)
    {
        List<CommonResult> commonResults = getDataResult.Results.ConvertAll<CommonResult>(result =>
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

    public void GetCountries()
    {
        if (_countryNeedUpdate)
            _back4AppHelper.GetData(Back4appHelper.COUNTRIES_CLASS, OnGetCountries);
    }
    
    private void OnGetCountries(GetDataResult getDataResult)
    {
        List<CommonResult> commonResults = getDataResult.Results.ConvertAll<CommonResult>(result =>
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
    }*/
}
