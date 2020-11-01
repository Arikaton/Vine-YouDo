using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class VineView : MonoBehaviour
{
    public static VineView main;

    [SerializeField] private GameObject whichOneWindow;
    [SerializeField] private Back4appHelper _back4AppHelper;
    [SerializeField] private RawImage image;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text countText;
    [SerializeField] private Text yearText;
    [SerializeField] private Text regionText;
    [SerializeField] private Text grapeText;
    [SerializeField] private Text colorText;
    [SerializeField] private Text countDrinkText;
    
    private VineData _vineData;
    private Texture2D _texture2D;
    private GameObject _cardObj;

    private int _count = 1;

    private void Awake()
    {
        main = this;
    }

    public void SetData(VineData data, Texture2D texture2D, GameObject cardObj)
    {
        _vineData = data;
        _texture2D = texture2D;
        _cardObj = cardObj;
        ResetCount();
        ShowData();
    }

    private void ShowData()
    {
        image.texture = _texture2D;
        nameText.text = _vineData.Name;
        descriptionText.text = _vineData.Description;
        countText.text = "Количество: " + _vineData.Count;
        yearText.text = _vineData.Year == 0 ? "Non Vintage" : _vineData.Year.ToString();
        regionText.text = _vineData.Region == "" ? "Регион не указан" : _vineData.Region;
        grapeText.text = _vineData.Grape;
        colorText.text = _vineData.Color;
    }

    public void ChangePlace(string cellar)
    {
        Destroy(_cardObj);
        _back4AppHelper.UpdateData(
            _vineData.objectId, 
            Back4appHelper.VINE_CLASS, 
            JsonConvert.SerializeObject(new Dictionary<string, string>(){{"Cellar", cellar}}));
        UIManager.Main.ShowWindow(whichOneWindow);
    }

    public void Drink()
    {
        if (_vineData.Count == _count)
        {
            _back4AppHelper.DeleteObject(_vineData.objectId, Back4appHelper.VINE_CLASS);
            Destroy(_cardObj);
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

    private void ResetCount() => _count = 1;

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
}
