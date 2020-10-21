using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AddVineButton : MonoBehaviour, IInitable
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    
    public void Init(Color color, string name, bool isFavorite, string objectId)
    {
        image.color = color;
        nameText.text = name;
    }

    public void Choose()
    {
        GetFromListHelper.Current.ChooseObject(nameText);
    }
}
