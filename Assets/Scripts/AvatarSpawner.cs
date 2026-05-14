using Photon.Pun;
using UnityEngine;

public class AvatarSpawner : MonoBehaviourPunCallbacks
{
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            var test = Resources.Load("Avatar");
            Debug.Log("Prefab found: " + test);
           PhotonNetwork.Instantiate("Avatar", new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Not in room yet!");
        }
    }
}