using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddVineManager : MonoBehaviour
{
    public static AddVineManager Main;

    [SerializeField] private GameObject rootWindow;
    [SerializeField] private Back4appHelper _back4AppHelper;
    [SerializeField] private GameObject grapeObject;
    [SerializeField] private GameObject colorObject;
    [SerializeField] private GameObject countryObject;
    [SerializeField] private GameObject regionObject;
    
    public string grape;
    public string color;
    public string country;
    public string region = "";
    public int year = 0;
    public string description;
    public int count;
    public string cellar;
    public string name;

    private Texture2D _image;
    private string _imagePath;
    
    public void Save()
    {
        if (!String.IsNullOrEmpty(grape) && 
            !String.IsNullOrEmpty(color) && 
            !String.IsNullOrEmpty(country) && 
            !String.IsNullOrEmpty(name) &&
            !String.IsNullOrEmpty(count.ToString()))
        {
            var vineData = new VineData(
                color,
                grape,
                region,
                country,
                description,
                count,
                year,
                cellar, 
                name);
            _back4AppHelper.UploadImage(_imagePath, vineData);
            UIManager.Main.ShowWindow(rootWindow);
        }
    }

    public void SetCellar(string cellar)
    {
        this.cellar = cellar;
    }

    private void Awake()
    {
        Main = this;
    }

    public void SetImage(Texture2D image, string imagePath)
    {
        _image = image;
        _imagePath = imagePath;
    }

    public void UpdateFields()
    {
        if (!string.IsNullOrEmpty(grape))
        {
            grapeObject.SetActive(true);
            grapeObject.GetComponentInChildren<Text>().text = grape;
        }
        if (!string.IsNullOrEmpty(color))
        {
            colorObject.SetActive(true);
            colorObject.GetComponentInChildren<Text>().text = color;
        }
        if (!string.IsNullOrEmpty(country))
        {
            countryObject.SetActive(true);
            countryObject.GetComponentInChildren<Text>().text = country;
        }
        if (!string.IsNullOrEmpty(region))
        {
            regionObject.SetActive(true);
            regionObject.GetComponentInChildren<Text>().text = region;
        }
    }

    public void OnChangeName(string name)
    {
        this.name = name;
    }

    public void OnChangeYear(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        if (int.TryParse(value, out var _year))
        {
            year = _year;
        }
    }
    
    public void OnChangeCount(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            count = 1;
            return;
        }
        if (!int.TryParse(value, out count))
        {
            throw new Exception("Can't convert count to int. Given value is " + count);
        }
    }

    public void OnChangeDescription(string value)
    {
        description = value;
    }

    
    
}
