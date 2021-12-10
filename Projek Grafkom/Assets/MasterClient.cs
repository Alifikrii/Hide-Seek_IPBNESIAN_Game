using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MasterClient : MonoBehaviourPunCallbacks {
//    [SerializeField] private GameObject _dosenWindow;
//    [SerializeField] private GameObject _dosenText;
    public void Initialize() {
        StartCoroutine(PickDosen());
    }

    private IEnumerator PickDosen() {
        GameObject[] players;
        List<int> playerIndex = new List<int>();
        int tries =0;
        int dosenNumber = 1;
        // get alll the player in the room
        do {
            players = GameObject.FindGameObjectsWithTag("Teman");
            tries++;
            yield return new WaitForSeconds(0.25f);
        } while ((players.Length < PhotonNetwork.CurrentRoom.PlayerCount) && tries < 10);
        
        // Initialize the player index list\
        for(int i =0; i< players.Length; i++){
            playerIndex.Add(i);
        }

        // Assign The Dosen
        int DosenIndexTerpilih = playerIndex[Random.Range(0,playerIndex.Count)];
        // playerIndex.Remove(DosenIndexTerpilih);
        PhotonView pv = players[DosenIndexTerpilih].GetComponent<PhotonView>();
        pv.RPC("RPC_SetDosen", RpcTarget.All, DosenIndexTerpilih);
    }

    // public IEnumerator GetHadir() {
    //     GameObject[] hadir;
    //     int tries =0;
    //     // get alll the player in the room
    //     do {
    //         hadir = GameObject.FindGameObjectsWithTag("Hadir");
    //         tries++;
    //         yield return new WaitForSeconds(0.25f);
    //     } while ((hadir.Length < PhotonNetwork.CurrentRoom.PlayerCount) && tries < 10);
    //     yield return hadir.Length;
    // }

    // public IEnumerator GetAlpa() {
    //     GameObject[] alpa;
    //     int tries =0;
    //     // get alll the player in the room
    //     do {
    //         alpa = GameObject.FindGameObjectsWithTag("Alpa");
    //         tries++;
    //         yield return new WaitForSeconds(0.25f);
    //     } while ((alpa.Length < PhotonNetwork.CurrentRoom.PlayerCount) && tries < 10);
    //     yield return alpa.Length;
    // }
}
