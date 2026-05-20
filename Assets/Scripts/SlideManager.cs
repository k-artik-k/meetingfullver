using UnityEngine;
using TMPro;
using SFB;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;

public class SlideManager : MonoBehaviourPun
{
    public Renderer screenRenderer;
    public TMP_Text slideCountText;

    public GameObject menuPanel;
    public Button prevButton;
    public Button nextButton;

    private Texture2D[] slides;
    private int currentIndex = 0;

    void Start()
    {
        menuPanel.SetActive(false);
        prevButton.interactable = false;
        nextButton.interactable = false;
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void OpenFiles()
    {
        Camera.main.enabled = true;

        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Select Images",
            "",
            new[] { new ExtensionFilter("Images", "png", "jpg", "jpeg") },
            true
        );

        if (paths.Length == 0)
            return;

        slides = new Texture2D[paths.Length];

        for (int i = 0; i < paths.Length; i++)
        {
            byte[] bytes = File.ReadAllBytes(paths[i]);

            Texture2D tex = new Texture2D(2, 2);

            tex.LoadImage(bytes);

            slides[i] = tex;
        }

        currentIndex = 0;

        ShowSlide(currentIndex);
        SendSlideToAll(currentIndex);

        prevButton.interactable = true;
        nextButton.interactable = true;

        menuPanel.SetActive(false);

        StartCoroutine(RegainFocus());
        StartCoroutine(RefocusGame());
    }

    IEnumerator RegainFocus()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        #if UNITY_EDITOR
        UnityEditor.EditorWindow[] windows =
            Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>();

        foreach (var w in windows)
        {
            if (w.GetType().Name == "GameView")
            {
                w.Focus();
                break;
            }
        }
        #endif
    }

    IEnumerator RefocusGame()
    {
        yield return new WaitForSeconds(0.1f);

        UnityEngine.EventSystems.EventSystem.current
            .SetSelectedGameObject(null);
    }

    public void NextSlide()
    {
        if (slides == null)
            return;

        currentIndex = (currentIndex + 1) % slides.Length;

        ShowSlide(currentIndex);
        SendSlideToAll(currentIndex);
    }

    public void PrevSlide()
    {
        if (slides == null)
            return;

        currentIndex = (currentIndex - 1 + slides.Length) % slides.Length;

        ShowSlide(currentIndex);
        SendSlideToAll(currentIndex);
    }

    void SendSlideToAll(int index)
    {
        byte[] bytes = slides[index].EncodeToPNG();

        photonView.RPC("ReceiveSlide", RpcTarget.Others, bytes);
    }

    [PunRPC]
    void ReceiveSlide(byte[] bytes)
    {
        Texture2D tex = new Texture2D(2, 2);

        tex.LoadImage(bytes);

        slides = slides ?? new Texture2D[1];
        slides[0] = tex;

        ShowSlide(0);
    }

    void ShowSlide(int index)
    {
        Texture2D tex = slides[index];

        screenRenderer.material.mainTexture = tex;

        if (slideCountText != null)
        {
            slideCountText.text =
                (index + 1) + " / " + slides.Length;
        }
    }
}