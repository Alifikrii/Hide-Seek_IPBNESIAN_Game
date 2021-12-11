using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MasterClient : MonoBehaviourPunCallbacks {

//Semua fungsi dibawah ini bertujuan untuk menetapkan role pada setiap player
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
        
        // Initialize the player index list
        for(int i =0; i< players.Length; i++){
            playerIndex.Add(i);
        }

        // Menetapkan siapa yang jadi dosen
        int DosenIndexTerpilih = playerIndex[Random.Range(0,playerIndex.Count)];
        PhotonView pv = players[DosenIndexTerpilih].GetComponent<PhotonView>();
        pv.RPC("RPC_SetDosen", RpcTarget.All, DosenIndexTerpilih);
    }
}
