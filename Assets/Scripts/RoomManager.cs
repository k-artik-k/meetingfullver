using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomCodeInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;
    public Button createButton;
    public Button joinButton;
    public GameObject canvas;
void Awake()
{
    DontDestroyOnLoad(gameObject);
}
    void Start()
    {
        createButton.interactable = false;
        joinButton.interactable = false;
    }

    public void CreateRoom()
    {
        string code = roomCodeInput.text;
        string pass = passwordInput.text;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 6;
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "password", pass }
        };
        options.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(code, options);
        statusText.text = "Creating room...";
    }

    public void JoinRoom()
    {
        string code = roomCodeInput.text;
        string pass = passwordInput.text;

        PhotonNetwork.JoinRoom(code);
        statusText.text = "Joining...";
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom! IsMaster: " + PhotonNetwork.IsMasterClient);
        canvas.SetActive(false);
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedLobby()
    {
        createButton.interactable = true;
        joinButton.interactable = true;
        statusText.text = "Ready!";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = "Create failed: " + message;
        Debug.LogError("Create failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "Room not found!";
        Debug.LogError("Join failed: " + message);
    }
}