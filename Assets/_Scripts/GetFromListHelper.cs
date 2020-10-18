using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class GetFromListHelper : MonoBehaviour
    {
        public static GetFromListHelper Current;

        private void OnEnable()
        {
            Current = this;
        }

        [SerializeField] private Back4appHelper _back4AppHelper;
        [SerializeField] private Transform scrollViewContent;
        [SerializeField] private EditListButton buttonPrefab;
        [SerializeField] private string type;

        public void GenerateList(List<CommonResult> commonResults)
        {
            DeleteChildren();
            
            foreach (var result in commonResults)
            {
                var newButton = Instantiate(buttonPrefab, scrollViewContent);
                Color buttonColor;
                if (ColorUtility.TryParseHtmlString("#" + result.hexColor, out buttonColor))
                {
                    newButton.Init(buttonColor, result.name, result.isFavorite, result.objectId);
                }
            }
            FilterListByFavorite(); //I'm too lazy to rewrite it
        }

        public void DeleteChildren()
        {
            for (int i = 0; i < scrollViewContent.childCount; i++)
            {
                Destroy(scrollViewContent.GetChild(i).gameObject);
            }
        }

        public void DeleteObject(string objectId)
        {
            _back4AppHelper.DeleteObject(objectId, type);
        }

        public void UpdateFavorite(bool isFavorite, string objectId)
        {
            _back4AppHelper.UpdateFavorite(isFavorite, objectId, type);
        }

        public void FilterListByFavorite()
        {
            List<EditListButton> buttons = scrollViewContent
                .GetComponentsInChildren<EditListButton>()
                .OrderBy((x) => x.IsFavorite ? 0 : 1)
                .ThenBy(x => x.Name)
                .ToList();

            for (int i = 0; i < scrollViewContent.childCount; i++)
            {
                buttons[i].transform.SetSiblingIndex(i);
            }
        }
    }
}