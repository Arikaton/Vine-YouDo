using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddVineManager : MonoBehaviour
{
    public static AddVineManager Main;
    public Action OnReset;

    [SerializeField] private GameObject rootWindow;
    [SerializeField] private Back4appHelper _back4AppHelper;
    [SerializeField] private GameObject grapeObject;
    [SerializeField] private GameObject colorObject;
    [SerializeField] private GameObject countryObject;
    [SerializeField] private GameObject regionObject;
    [SerializeField] private GameObject grapeButtonPrefab;
    [SerializeField] private Transform grapeContentParent;

    [SerializeField] InputField nameInputField;
    [SerializeField] InputField yearInputField;
    [SerializeField] InputField countInputField;
    [SerializeField] InputField descriptionInputField;
    
    public string grape;
    public string color;
    public string country;
    public string region = "";
    public int year = 0;
    public string description = "";
    public int count = 1;
    public string cellar;
    public string name;

    private Texture2D _image;
    private string _imagePath;

    public void Reset()
    {
        nameInputField.text = "";
        yearInputField.text = "";
        countInputField.text = "";
        descriptionInputField.text = "";
        grape = "";
        foreach (Transform child in grapeContentParent)
        {
            Destroy(child.gameObject);
        }
        grapeObject.SetActive(false);
        color = "";
        colorObject.SetActive(false);
        count = 1;
        region = "";
        regionObject.SetActive(false);
        country = "";
        countryObject.SetActive(false);
        year = 0;
        description = "";
        name = "";
        OnReset?.Invoke();
    }

    public void GrapeChoosen(bool _isChoosen, string name)
    {
        if (_isChoosen)
        {
            if (string.IsNullOrEmpty(grape))
            {
                grape += name;
            }
            else
            {
                grape += $",{name}";
            }
        }
        else
        {
            if (grape.Contains(name))
            {
                print("Grape contains "+name);
                if (grape.IndexOf(name, StringComparison.Ordinal) == 0)
                {
                    print("try remove first grape");
                    if (grape.Length != name.Length)
                    {
                        print("Grape not only one");
                        grape = grape.Replace($"{name},", "");
                    }
                    else
                    {
                        print("Grape is only one");
                        grape = "";
                    }
                }
                else
                {
                    print("try remove middle or last grape");
                    grape = grape.Replace($",{name}", "");
                }
            }
        }
    }
    
    public void Save()
    {
        if (!String.IsNullOrEmpty(grape) && 
            !String.IsNullOrEmpty(color) && 
            !String.IsNullOrEmpty(country))
        {
            RepositoryManager.UpdateVineInfo(cellar, true);
            
            var vineData = new VineData(
                color,
                grape,
                region,
                country,
                description,
                count,
                year,
                cellar);

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
            foreach (Transform grapeChild in grapeContentParent)
            {    
                Destroy(grapeChild.gameObject);
            }
            foreach (var grapeName in grape.Split(','))
            {
                var grapeButtonGo = Instantiate(grapeButtonPrefab, grapeContentParent);
                grapeButtonGo.GetComponentInChildren<Text>().text = grapeName;
            }
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
