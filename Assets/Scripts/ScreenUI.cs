using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    public TMP_InputField urlInput;
    public PresentationScreen presentationScreen;

    public void OnLoadPressed()
    {
        string url = urlInput.text;
        if (!string.IsNullOrEmpty(url))
        {
            presentationScreen.LoadImage(url);
        }
    }
}