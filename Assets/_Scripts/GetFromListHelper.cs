using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class GetFromListHelper : MonoBehaviour
    {
        public static GetFromListHelper Current;

        [SerializeField] private Back4appHelper _back4AppHelper;
        [SerializeField] private Transform scrollViewContent;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject loadingAnimation;
        [SerializeField] private GameObject errorMessage;
        public string type;
        
        private void OnEnable()
        {
            Current = this;
            errorMessage.SetActive(false);
            loadingAnimation.SetActive(false);
            GetData();
        }

        public void GetData()
        {
            loadingAnimation.SetActive(true);
            bool needUpdate = false;
            GetDataResult data = null;
            if (type == Back4appHelper.GRAPES_CLASS)
            {
                if (RepositoryManager._grapeNeedUpdate)
                {
                    needUpdate = true;
                    RepositoryManager._grapeNeedUpdate = false;
                }
                else
                    data = new GetDataResult(RepositoryManager.grapeData);
            }
            else if (type == Back4appHelper.COLORS_CLASS)
            {
                if (RepositoryManager._colorNeedUpdate)
                {
                    needUpdate = true;
                    RepositoryManager._colorNeedUpdate = false;
                }
                else
                    data = new GetDataResult(RepositoryManager.colorData);
            }
            else if (type == Back4appHelper.COUNTRIES_CLASS)
            {
                if (RepositoryManager._countryNeedUpdate)
                {
                    needUpdate = true;
                    RepositoryManager._countryNeedUpdate = false;
                }
                else
                    data = new GetDataResult(RepositoryManager.countryData);
            }
            else if (type == Back4appHelper.REGIONS_CLASS)
            {
                if (RepositoryManager._regionNeedUpdate)
                {
                    needUpdate = true;
                    RepositoryManager._regionNeedUpdate = false;
                }
                else
                    data = new GetDataResult(RepositoryManager.regionData);
            }
            else
            {
                throw new Exception("Wrong type");
            }

            if (needUpdate)
            {
                _back4AppHelper.GetData(type, OnGetData);
                Debug.Log("Data getting from b4a");
            }
            else
            {
                if (scrollViewContent.childCount == 0)
                {
                    Debug.Log("Data getting from local storage");
                    OnGetData(data);
                }
                else
                {
                    loadingAnimation.SetActive(false);
                }
            }
        }

        private void OnGetData(GetDataResult result)
        {
            if (result.ResponseCode != 200)
            {
                errorMessage.SetActive(true);
            }
            else
            {
                loadingAnimation.SetActive(false);
                UpdateLocalData(result);
                var deserializedObject = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(result.resultsString);
                List<CommonResult> commonResults = new List<CommonResult>();
                foreach (var data in deserializedObject)
                {
                    bool isFavorite = false;
                    string hexColor = "3192EB";
                    if (data.ContainsKey("IsFavorite"))
                        isFavorite = (bool?) data["IsFavorite"] == true;
                    string objectId = data["objectId"].ToString();
                    if (data.ContainsKey(Back4appHelper.HEX))
                        hexColor = data[Back4appHelper.HEX].ToString();
                    string name = "";
                    if (type == Back4appHelper.GRAPES_CLASS)
                    {
                        name = data[Back4appHelper.GRAPES_FIELD].ToString();
                    }
                    else if (type == Back4appHelper.COLORS_CLASS)
                    {
                        name = data[Back4appHelper.COLOR_FIELD].ToString();
                    }
                    else if (type == Back4appHelper.COUNTRIES_CLASS)
                    {
                        name = data[Back4appHelper.COUNTRIES_FIELD].ToString();
                    }
                    else if (type == Back4appHelper.REGIONS_CLASS)
                    {
                        name = data[Back4appHelper.REGION_FIELD].ToString();
                    }
                    else
                    {
                        throw new Exception("Wrong type");
                    }

                    var newResult = new CommonResult(name, hexColor, isFavorite, objectId);
                    commonResults.Add(newResult);
                }
                GenerateList(commonResults);
            }
        }

        private void UpdateLocalData(GetDataResult result)
        {
            if (type == Back4appHelper.GRAPES_CLASS)
            {
                RepositoryManager.grapeData = result.resultsString;
            }
            else if (type == Back4appHelper.COLORS_CLASS)
            {
                RepositoryManager.colorData = result.resultsString;
            }
            else if (type == Back4appHelper.COUNTRIES_CLASS)
            {
                RepositoryManager.countryData = result.resultsString;
            }
            else if (type == Back4appHelper.REGIONS_CLASS)
            {
                RepositoryManager.regionData = result.resultsString;
            }
            else
            {
                throw new Exception("Wrong type");
            }
        }

        public void GenerateList(List<CommonResult> commonResults)
        {
            DeleteChildren();

            var orderedEnumerable = commonResults
                .OrderBy(x => x.isFavorite)
                .ThenBy(x => x.name);

            foreach (var result in orderedEnumerable)
            {
                var newButtonGo = Instantiate(buttonPrefab, scrollViewContent);
                IInitable newButton = newButtonGo.GetComponent<IInitable>();
                if (ColorUtility.TryParseHtmlString("#" + result.hexColor, out var buttonColor))
                {
                    newButton.Init(buttonColor, result.name, result.isFavorite, result.objectId);
                }
                else
                {
                    throw new Exception("Error while parsing color");
                }
            }
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

        public void ChooseObject(Text text)
        {
            if (type == Back4appHelper.GRAPES_CLASS)
            {
                AddVineManager.Main.grape = text.text;
            }
            else if (type == Back4appHelper.COLORS_CLASS)
            {
                AddVineManager.Main.color = text.text;
            }
            else if (type == Back4appHelper.COUNTRIES_CLASS)
            {
                AddVineManager.Main.country = text.text;
            }
            else if (type == Back4appHelper.REGIONS_CLASS)
            {
                AddVineManager.Main.region = text.text;
            }
            else
            {
                throw new Exception("Wrong type");
            }
            UIManager.Main.ShowWindow(UIManager.Main.addVineMain);
            AddVineManager.Main.UpdateFields();
        }

        public void UpdateFavorite(bool isFavorite, string objectId)
        {
            _back4AppHelper.UpdateFavorite(isFavorite, objectId, type);
        }

        public void AddToFilter(bool isAddedToFilter, Color color, string text)
        {
            GetVineManager.main.AddToFilter(type, isAddedToFilter, color, text);
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