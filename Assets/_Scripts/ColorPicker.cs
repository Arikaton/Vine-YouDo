using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Slider red;
    [SerializeField] private Slider green;
    [SerializeField] private Slider blue;
    [SerializeField] private Image exampleSquare;

    public Color CurrentColor
    {
        get
        {
            Color color = new Color(red.value, green.value, blue.value);
            return color;
        }
    }

    private void OnEnable()
    {
        OnChangeValue(1);
    }

    public void OnChangeValue(float value)
    {
        exampleSquare.color = CurrentColor;
    }
}
