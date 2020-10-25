using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VineCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RawImage _image;
    [SerializeField] private GameObject downloadAnim;

    private VineData _vineData;
    private Texture2D _texture2D;

    public void Init(VineData vineData)
    {
        _vineData = vineData;
        StartCoroutine(DownloadImage());
    }

    IEnumerator DownloadImage()
    {
        var www = UnityWebRequestTexture.GetTexture(_vineData.Image["url"]);
        yield return www.SendWebRequest();
        _texture2D = DownloadHandlerTexture.GetContent(www);
        _image.texture = _texture2D;
        downloadAnim.SetActive(false);
    }

    public void ShowCardView()
    {
        VineView.main.SetData(_vineData, _texture2D, gameObject);
        UIManager.Main.ShowVineView();
    }

    public void UpdateCount(int count)
    {
        _vineData.Count = count;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowCardView();
    }
}
