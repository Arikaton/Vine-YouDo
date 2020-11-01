using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGrapeButton : MonoBehaviour, IInitable
{
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private GameObject chooseImage;
    
    private bool _isChoosen = false;

    private string _objectId;

    public void Init(Color color, string name, bool isFavorite, string objectId)
    {
        image.color = color;
        text.text = name;
        _objectId = objectId;
    }

    public void Choose()
    {
        if (_isChoosen)
        {
            _isChoosen = false;
            chooseImage.SetActive(false);
        }
        else
        {
            _isChoosen = true;
            chooseImage.SetActive(true);
        }
        AddVineManager.Main.GrapeChoosen(_isChoosen, text.text);
    }
    
    public void Reset()
    {
        chooseImage.SetActive(false);
        _isChoosen = false;
    }
}
