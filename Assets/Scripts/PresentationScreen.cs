using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class PresentationScreen : MonoBehaviourPun
{
    public Renderer screenRenderer;
    private int currentSlide = 0;
    private string[] slideURLs;

    public void LoadImage(string url)
    {
        StartCoroutine(DownloadImage(url));
        photonView.RPC("SyncImage", RpcTarget.Others, url);
    }

    [PunRPC]
    void SyncImage(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            screenRenderer.material.mainTexture = texture;
        }
        else
        {
            Debug.LogError("Image load failed: " + request.error);
        }
    }
}