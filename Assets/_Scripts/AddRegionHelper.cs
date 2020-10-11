using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddRegionHelper : MonoBehaviour
{
    [SerializeField] private Text regionText;
    [SerializeField] private Text countryText;
    [SerializeField] private RepositoryManager _repositoryManager;

    public void AddRegion()
    {
        _repositoryManager.AddRegion(regionText, countryText);
    }
}
