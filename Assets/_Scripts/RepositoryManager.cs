using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class RepositoryManager : MonoBehaviour
{
    public static bool GrapeNeedUpdate = true;
    public static bool ColorNeedUpdate = true;
    public static bool CountryNeedUpdate = true;
    public static bool RegionNeedUpdate = true;
    public static bool MoscowVineNeedUpdate = true;
    public static bool GreenVineNeedUpdate = true;
    public static bool NizniyVineNeedUpdate = true;
    public static bool OtherVineNeedUpdate = true;
    
    public static string GrapeData;
    public static string ColorData;
    public static string CountryData;
    public static string RegionData;
    public static string MoscowVineData;
    public static string GreenVineData;
    public static string NizniyVineData;
    public static string OtherVineData;

    public static bool GetVineInfo(string cellar)
    {
        switch (cellar)
        {
            case "Москва":
                return MoscowVineNeedUpdate;
            case "Зеленый город":
                return GreenVineNeedUpdate;
            case "Нижний":
                return NizniyVineNeedUpdate;
            default:
                return OtherVineNeedUpdate;
        }
    }
    
    public static void UpdateVineInfo(string cellar, bool needUpdate)
    {
        switch (cellar)
        {
            case "Москва":
                MoscowVineNeedUpdate = needUpdate;
                break;
            case "Зеленый город":
                GreenVineNeedUpdate = needUpdate;
                break;
            case "Нижний":
                NizniyVineNeedUpdate = needUpdate;
                break;
            default:
                OtherVineNeedUpdate = needUpdate;
                break;
        }
    }
    public static void UpdateVineData(string cellar, string data)
    {
        switch (cellar)
        {
            case "Москва":
                MoscowVineData = data;
                break;
            case "Зеленый город":
                GreenVineData = data;
                break;
            case "Нижний":
                NizniyVineData = data;
                break;
            default:
                OtherVineData = data;
                break;
        }
    }
    
    public static List<VineData> GetVineData(string cellar)
    {
        switch (cellar)
        {
            case "Москва":
                return JsonConvert.DeserializeObject<List<VineData>>(MoscowVineData);
            case "Зеленый город":
                return JsonConvert.DeserializeObject<List<VineData>>(GreenVineData);
            case "Нижний":
                return JsonConvert.DeserializeObject<List<VineData>>(NizniyVineData);
            default:
                return JsonConvert.DeserializeObject<List<VineData>>(OtherVineData);
        }
    }
}
