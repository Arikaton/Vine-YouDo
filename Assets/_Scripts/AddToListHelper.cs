using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToListHelper : MonoBehaviour
{
    [SerializeField] private GameObject downloadImage;
    [SerializeField] private GameObject successText;
    [SerializeField] private GameObject errorText;
    [SerializeField] private Text regionCountryText = null;
    [SerializeField] private Back4appHelper back4AppHelper;
    [SerializeField] private string type;
    [SerializeField] private InputField inputField;
    public Image color;

    private void OnEnable()
    {
        downloadImage.SetActive(false);
        successText.SetActive(false);
        errorText.SetActive(false);
    }

    public void StartAddingData(Text text)
    {
        if (string.IsNullOrEmpty(text.text))
            return;
        downloadImage.SetActive(true);
        successText.SetActive(false);
        errorText.SetActive(false);
        var data = new Dictionary<string, object>();
        if (type == Back4appHelper.GRAPES_CLASS)
        {
            data.Add(Back4appHelper.GRAPES_FIELD, text.text);
            RepositoryManager._grapeNeedUpdate = true;
        } else if (type == Back4appHelper.COLORS_CLASS)
        {
            data.Add(Back4appHelper.COLOR_FIELD, text.text);
            RepositoryManager._colorNeedUpdate = true;
        } else if (type == Back4appHelper.COUNTRIES_CLASS)
        {
            data.Add(Back4appHelper.COUNTRIES_FIELD, text.text);
            RepositoryManager._countryNeedUpdate = true;
        } else if (type == Back4appHelper.REGIONS_CLASS)
        {
            data.Add(Back4appHelper.REGION_FIELD, text.text);
            if (regionCountryText != null)
            {
                data.Add(Back4appHelper.COUNTRIES_FIELD, regionCountryText.text);
                RepositoryManager._regionNeedUpdate = true;
            }
        }
        else
        {
            throw new Exception("Wrong class name");
        }

        if (color != null)
        {
            var hexColor = ColorUtility.ToHtmlStringRGB(color.color);
            data.Add(Back4appHelper.HEX, hexColor);
        }
        back4AppHelper.AddData(type, data, OnAddData);
        
    }

    private void OnAddData(AddDataResult data)
    {
        if (data.ResponseCode == 201)
        {
            FinishedAddingData();
        }
        else
        {
            ErrorWhileAdding();
        }
    }

    public void FinishedAddingData()
    {
        downloadImage.SetActive(false);
        successText.SetActive(true);
        inputField.text = "";
    }

    public void ErrorWhileAdding()
    {
        downloadImage.SetActive(false);
        errorText.SetActive(true);
    }
}
