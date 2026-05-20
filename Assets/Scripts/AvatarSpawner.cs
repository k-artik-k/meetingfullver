using Photon.Pun;
using UnityEngine;

public class AvatarSpawner : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (PhotonNetwork.InRoom)
            SpawnAvatar();
    }

    public override void OnJoinedRoom()
    {
        SpawnAvatar();
    }

    void SpawnAvatar()
    {
        Debug.Log("Prefab found: " + Resources.Load("Avatar"));
        PhotonNetwork.Instantiate("Avatar", new Vector3(0, 1, 0), Quaternion.identity);
    }
}