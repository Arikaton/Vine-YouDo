using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

public class GetVineButton : MonoBehaviour, IInitable
{
    [SerializeField] private Text text;
    [SerializeField] private Image image;
    [SerializeField] private GameObject filterIcon;
    private string _objectId;
    private bool isAddedToFilter = false;
    
    public void Init(Color color, string name, bool isFavorite, string objectId)
    {
        _objectId = objectId;
        image.color = color;
        text.text = name;
        filterIcon.SetActive(false);
    }

    public void AddToFilter()
    {
        if (isAddedToFilter)
        {
            isAddedToFilter = false;
            filterIcon.SetActive(false);
        }
        else
        {
            isAddedToFilter = true;
            filterIcon.SetActive(true);
        }
        GetFromListHelper.Current.AddToFilter(isAddedToFilter, image.color, text.text);
    }
}
