using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomCodeInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;
    public Button createButton;
    public Button joinButton;
    public GameObject canvas;
    public GameObject meetingCanvas;

    void Start()
    {
        createButton.interactable = false;
        joinButton.interactable = false;
        if (meetingCanvas != null)
            meetingCanvas.SetActive(false);
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
        string storedPass = (string)PhotonNetwork.CurrentRoom.CustomProperties["password"];
        string enteredPass = passwordInput.text;

        if (storedPass != enteredPass)
        {
            statusText.text = "Wrong password!";
            PhotonNetwork.LeaveRoom();
            return;
        }

        statusText.text = "Joined: " + PhotonNetwork.CurrentRoom.Name;
        canvas.SetActive(false);
        // PhotonNetwork.LoadLevel("MeetingRoom");

        if (meetingCanvas != null)
            meetingCanvas.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        createButton.interactable = true;
        joinButton.interactable = true;
        statusText.text = "Ready!";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "Room not found!";
    }
}