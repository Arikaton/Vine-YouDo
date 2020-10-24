using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VineView : MonoBehaviour
{
    public static VineView main;

    [SerializeField] private RawImage image;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text countText;
    [SerializeField] private Text yearText;
    [SerializeField] private Text regionText;
    
    private VineData _vineData;
    private Texture2D _texture2D;

    private void Awake()
    {
        main = this;
    }

    public void SetData(VineData data, Texture2D texture2D)
    {
        _vineData = _vineData;
        _texture2D = texture2D;
    }

    private void OnEnable()
    {
        image.texture = _texture2D;
        nameText.text = _vineData.Name;
        descriptionText.text = _vineData.Description;
        countText.text = _vineData.Count.ToString();
        yearText.text = _vineData.Year.ToString();
        regionText.text = _vineData.Region;
    }
}
