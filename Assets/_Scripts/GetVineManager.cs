using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GetVineManager : MonoBehaviour
{
    public static GetVineManager main;
    public Action OnReset;
    
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Back4appHelper _back4AppHelper;
    [SerializeField] private GameObject filterWindow;
    [SerializeField] private VineScroll vineScollPrefab;
    [SerializeField] Text filterWindowName;
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private Transform grapeContentWindow;
    [SerializeField] private Transform colorContentWindow;
    [SerializeField] private Transform countryContentWindow;
    [SerializeField] private Transform regionContentWindow;
    [SerializeField] private Transform vineContentWindow;

    public string cellar;
    public Dictionary<string, Color> grapes = new Dictionary<string, Color>();
    public Dictionary<string, Color> colors = new Dictionary<string, Color>();
    public Dictionary<string, Color> countries = new Dictionary<string, Color>();
    public Dictionary<string, Color> regions = new Dictionary<string, Color>();

    private bool _useFilter;

    private void Awake()
    {
        main = this;
    }

    public void GetVineList(bool useFilter)
    {
        _useFilter = useFilter;
        _back4AppHelper.GetVine(cellar, OnGetVine);
    }

    void OnGetVine(List<VineData> vines)
    {
        var orderedVine = vines.OrderBy(x => x.Year).ToList();
        if (_useFilter)
        {
            orderedVine = orderedVine.Where(x =>
            {
                if (grapes.Count > 0 && !grapes.ContainsKey(x.Grape))
                    return false;
                if (colors.Count > 0 && !colors.ContainsKey(x.Color))
                    return false;
                if (countries.Count > 0 && !countries.ContainsKey(x.Country))
                    return false;
                if (regions.Count > 0 && !regions.ContainsKey(x.Region))
                    return false;
                return true;
            }).ToList();
        }
        int year = orderedVine[0].Year;
        List<VineData> vineHandler = new List<VineData>();
        for (int i = 0; i < orderedVine.Count; i++)
        {
            var vineData = orderedVine[i];
            if (vineData.Year != year)
            {
                var vineScroll = Instantiate(vineScollPrefab, vineContentWindow);
                vineScroll.Init(year.ToString(), vineHandler);
                vineHandler = new List<VineData>();
                year = vineData.Year;
                vineHandler.Add(vineData);
            }
            else
            {
                vineHandler.Add(vineData);
            }
            if (i == orderedVine.Count - 1)
            {
                var lastVineScroll = Instantiate(vineScollPrefab, vineContentWindow);
                lastVineScroll.Init(year.ToString(), vineHandler);
            }
        }
    }

    public void ShowFilterWindow(string cellarName)
    {
        cellar = cellarName;
        _uiManager.ShowWindow(filterWindow);
        filterWindowName.text = cellarName;
    }

    public void UpdateFields(string type)
    {
        switch (type)
        {
            case "Grapes":
                DestroyChilds(grapeContentWindow);
                UpdateChips(grapes, grapeContentWindow);
                break;
            case "Colors":
                DestroyChilds(colorContentWindow);
                UpdateChips(colors, colorContentWindow);
                break;
            case "Countries":
                DestroyChilds(countryContentWindow);
                UpdateChips(countries, countryContentWindow);
                break;
            case "Regions":
                DestroyChilds(regionContentWindow);
                UpdateChips(regions, regionContentWindow);
                break;
        }
    }

    public void DestroyChilds(Transform otherTransform)
    {
        foreach (Transform child in otherTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateChips(Dictionary<string, Color> dictionary, Transform contentWindow)
    {
        if (dictionary.Count == 0)
            return;
        foreach (var key in dictionary.Keys)
        {
            var chipGo = Instantiate(chipPrefab, contentWindow);
            chipGo.GetComponent<Image>().color = dictionary[key];
            chipGo.GetComponentInChildren<Text>().text = key;
        }
    }

    public void Reset()
    {
        grapes = new Dictionary<string, Color>();
        colors = new Dictionary<string, Color>();
        countries = new Dictionary<string, Color>();
        regions = new Dictionary<string, Color>();
        DestroyChilds(grapeContentWindow);
        DestroyChilds(colorContentWindow);
        DestroyChilds(countryContentWindow);
        DestroyChilds(regionContentWindow);
        OnReset?.Invoke();
    }

    public void AddToFilter(string type, bool isAddedToFilter, Color color, string text)
    {
        if (type == Back4appHelper.GRAPES_CLASS)
        {
            AddToFilterHelper(isAddedToFilter, color, text, grapes);
        } else if (type == Back4appHelper.COLORS_CLASS)
        {
            AddToFilterHelper(isAddedToFilter, color, text, colors);
        } else if (type == Back4appHelper.COUNTRIES_CLASS)
        {
            AddToFilterHelper(isAddedToFilter, color, text, countries);
        } else if (type == Back4appHelper.REGIONS_CLASS)
        {
            AddToFilterHelper(isAddedToFilter, color, text, regions);
        }
        else
        {
            throw new Exception("Wrong type");
        }
    }

    private void AddToFilterHelper(bool isAddedToFilter, Color color, string text, Dictionary<string, Color> dictionary)
    {
        if (isAddedToFilter)
        {
            dictionary.Add(text, color);
        }
        else
        {
            dictionary.Remove(text);
        }
    }
}
