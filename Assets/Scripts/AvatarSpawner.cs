using Photon.Pun;
using UnityEngine;

public class AvatarSpawner : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("AvatarSpawner Start - InRoom: " + PhotonNetwork.InRoom);
        if (PhotonNetwork.InRoom)
        {
            SpawnAvatar();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("AvatarSpawner OnJoinedRoom called!");
        SpawnAvatar();
    }

    void SpawnAvatar()
    {
        var test = Resources.Load("Avatar");
        Debug.Log("Prefab found: " + test);
        GameObject avatar = PhotonNetwork.Instantiate("Avatar", new Vector3(0, 1, 0), Quaternion.identity);

        if (avatar.GetComponent<PhotonView>().IsMine)
        {
            Camera.main.GetComponent<CameraFollow>().SetTarget(avatar.transform);
        }
    }
}