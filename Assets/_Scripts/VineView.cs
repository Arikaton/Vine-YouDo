using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class VineView : MonoBehaviour
{
    public static VineView main;

    [SerializeField] private GameObject whichOneWindow;
    [SerializeField] private Back4appHelper _back4AppHelper;
    [SerializeField] private RawImage image;
    //[SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text countText;
    [SerializeField] private Text yearText;
    [SerializeField] private Text countryText;
    [SerializeField] private Text regionText;
    [SerializeField] private Text grapeText;
    [SerializeField] private Text colorText;
    [SerializeField] private Text countDrinkText;
    [SerializeField] private Text countCellarText;
    
    private VineData _vineData;
    private Texture2D _texture2D;
    private GameObject _cardObj;
    private string currentCellar;
    private VineCard _vineCard;

    private bool _isDownloading = false;

    private int _count = 1;
    private int _cellarCount = -1;

    private void Awake()
    {
        main = this;
    }

    public void SetData(VineData data, Texture2D texture2D, GameObject cardObj, VineCard vineCard)
    {
        currentCellar = data.Cellar;
        _vineCard = vineCard;
        _vineData = data;
        _texture2D = texture2D;
        _cardObj = cardObj;
        ResetCount();
        ShowData();
    }

    public void RotateImage()
    {
        var rotatedTexture = ImageDownloader.RotateTexture(_texture2D, true);
        _texture2D = rotatedTexture;
        ImageDownloader.SaveImageLocal(rotatedTexture, _vineData.Image["url"]);
        image.texture = rotatedTexture;
        _vineCard.Texture = rotatedTexture;
    }

    private void ShowData()
    {
        image.texture = _texture2D;
        descriptionText.text = _vineData.Description;
        countText.text = "Количество: " + _vineData.Count;
        yearText.text = _vineData.Year == 0 ? "Non Vintage" : _vineData.Year.ToString();
        regionText.text = _vineData.Region == "" ? "Регион не указан" : _vineData.Region;
        grapeText.text = _vineData.Grape.Replace(',','\n');
        colorText.text = _vineData.Color;
        countryText.text = _vineData.Country;
    }

    public void ChangePlace(string cellar)
    {
        if (_isDownloading) return;
        UpdateCellarInfo(currentCellar);
        UpdateCellarInfo(cellar);
        _isDownloading = true;
        _back4AppHelper.SameVineExist(cellar, _vineData, data =>
        {
            if (data != null)
            {
                if (_cellarCount == -1 || _cellarCount == _vineData.Count)
                {
                    var countData = JsonConvert.SerializeObject(new Dictionary<string, int>()
                    {
                        {"Count", data.Count + _vineData.Count}
                    });
                    _back4AppHelper.UpdateData(data.objectId, Back4appHelper.VINE_CLASS, countData);
                    _back4AppHelper.DeleteObject(_vineData.objectId, Back4appHelper.VINE_CLASS);
                    Destroy(_cardObj);
                }
                else
                {
                    var countHandler = new Dictionary<string, int>(1)
                    {
                        {"Count", data.Count + _cellarCount}
                    };
                    _vineData.Count -= _cellarCount;
                    _back4AppHelper.UpdateData(data.objectId, Back4appHelper.VINE_CLASS, JsonConvert.SerializeObject(countHandler));
                    countHandler["Count"] = _vineData.Count;
                    _back4AppHelper.UpdateData(_vineData.objectId, Back4appHelper.VINE_CLASS, JsonConvert.SerializeObject(countHandler));
                }
            }
            else
            {
                if (_cellarCount == -1 || _cellarCount == _vineData.Count)
                {
                    _back4AppHelper.UpdateData(
                        _vineData.objectId, 
                        Back4appHelper.VINE_CLASS, 
                        JsonConvert.SerializeObject(new Dictionary<string, string>(){{"Cellar", cellar}}));
                    Destroy(_cardObj);
                }
                else
                {
                    var countHandler = new Dictionary<string, int>(1)
                    {
                        {"Count", _vineData.Count - _cellarCount}
                    };
                    _back4AppHelper.UpdateData(_vineData.objectId, Back4appHelper.VINE_CLASS, JsonConvert.SerializeObject(countHandler));
                    var newVineData = _vineData.Copy();
                    _vineData.Count -= _cellarCount;
                    newVineData.Count = _cellarCount;
                    newVineData.objectId = "";
                    newVineData.Cellar = cellar;
                    _back4AppHelper.AddData(Back4appHelper.VINE_CLASS, JsonConvert.SerializeObject(newVineData));
                }
            }

            _isDownloading = false;
            UIManager.Main.ShowWindow(whichOneWindow);
        });
        
    }

    private void UpdateCellarInfo(string cellar)
    {
        switch (cellar)
        {
            case "Москва":
                RepositoryManager.MoscowVineNeedUpdate = true;
                break;
            case "Зеленый город":
                RepositoryManager.GreenVineNeedUpdate = true;
                break;
            case "Нижний":
                RepositoryManager.NizniyVineNeedUpdate = true;
                break;
            default:
                RepositoryManager.OtherVineNeedUpdate = true;
                break;
        }
    }

    public void Drink()
    {
        UpdateCellarInfo(currentCellar);
        if (_vineData.Count == _count)
        {
            _back4AppHelper.DeleteObject(_vineData.objectId, Back4appHelper.VINE_CLASS);
            var path = ImageDownloader.GetPathFromImageUrl(_vineData.Image["url"]);
            Destroy(_cardObj);
            File.Delete(path);
        }
        else
        {
            _vineData.Count -= _count;
            _back4AppHelper.UpdateData(
                _vineData.objectId, 
                Back4appHelper.VINE_CLASS, 
                JsonConvert.SerializeObject(new Dictionary<string, int>() 
                    {{"Count", _count}}
                ));
        }
        UIManager.Main.ShowWindow(whichOneWindow);
    }

    private void ResetCount()
    {
        _count = 1;
        _cellarCount = -1;
        UpdateCellarCountText();
        UpdateCountDrinkText();
    } 

    private void UpdateCountDrinkText() => countDrinkText.text = _count.ToString();

    public void Add()
    {
        if (_count < _vineData.Count)
        {
            _count++;
            UpdateCountDrinkText();
        }
    }

    public void Subtract()
    {
        if (_count > 1)
        {
            _count--;
            UpdateCountDrinkText();
        }
    }

    private void UpdateCellarCountText() => countCellarText.text = _cellarCount == -1 ? "Все" : _cellarCount.ToString();

    public void AddCellarCount()
    {
        if (_cellarCount == -1)
        {
            _cellarCount = 1;
        }
        else
        {
            if (_cellarCount < _vineData.Count)
                _cellarCount++;
        }
        UpdateCellarCountText();
    }

    public void SubtractCellarCount()
    {
        if (_cellarCount <= 1)
        {
            _cellarCount = -1;
        }
        else
        {
            _cellarCount--;
        }

        UpdateCellarCountText();
    }
}
