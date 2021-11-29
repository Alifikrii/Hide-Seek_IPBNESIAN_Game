using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public Text PingText;
    public Text NamaRoom;
    public Text pList;
    private string playerl;
    public Text JumlahPlayerdiLoby;
    public GameObject DisconnectUI;
    private bool off = false;
    public Button startButton;
    public PhotonView myPv;

    //ktext tunggu host memulai game
    public Text HostText;

    //mulai mencari dosen 
    // GameObject DosenRole;
    // public PlayerScript PemainScript;
    // public MasterClient masterClient;
    


    private void Update()
    {
        PingText.text = "ping : " + PhotonNetwork.GetPing() + "ms";
        // JumlahPlayerdiLoby.text = "Jumlah Player : "+ PhotonNetwork.CurrentRoom.PlayerCount;
        NamaRoom.text = "LOBBY : " + PhotonNetwork.CurrentRoom.Name;

        if(PhotonNetwork.GetPing()>90)
            PingText.color = Color.red;
        else
            PingText.color = Color.green;
        
        //jika ada 2 atau lebih player di dalam loby
        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
        // if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startButton.interactable = true;
            HostText.text = "Tekan Start Untuk Memulai Game";
            HostText.color = Color.green;

        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            startButton.interactable = false;
        }
        
        CheckInput();
        AddAllActivePlayers();
    }

//untuk list player
    public void AddAllActivePlayers()
     {
         var playerList = new StringBuilder();
         Dictionary<int, Photon.Realtime.Player> playerl = Photon.Pun.PhotonNetwork.CurrentRoom.Players;
         foreach (KeyValuePair<int, Photon.Realtime.Player> p in playerl)
         {
             
            playerList.Append(p.Value.NickName + "\n"); 
         }

         pList.text = playerList.ToString();
     }
    

    private void Awake()
    {
        GameCanvas.SetActive(true);
    }

    public void SpawnPlayer()
    {
        
        // Debug.Log("adakok");
        float randomValue = Random.Range(-1f, 1f);
        // PhotonNetwork.Instantiate(this.PlayerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
        Vector2 randomPos = new Vector2(this.transform.position.x *randomValue, this.transform.position.y *randomValue);
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomPos, Quaternion.identity,0);
        // PhotonNetwork.Instantiate(plr, randomPos, Quaternion.identity,0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);

        // masterClient.Initialize();


        // DosenRole = GameObject.Find("Player").
       // PemainScript = GetComponent<PlayerScript>();
        // DosenRole.PilihDosen();
        // PemainScript.PilihDosen();

    }
   
    public void CheckInput()
    {
        if(off && Input.GetKeyDown(KeyCode.Escape))
        {
            DisconnectUI.SetActive(false);
            off=false;
        }
        else if(!off && Input.GetKeyDown(KeyCode.Escape))
        {
            DisconnectUI.SetActive(true);
            off=true;
        }
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

   
    public void startBtn()
    {
        myPv = GetComponent<PhotonView>();
        myPv.RPC("RPC_spawnAll", RpcTarget.All);
        myPv.RPC("RPC_LobyMessage", RpcTarget.AllBufferedViaServer);

    }

    // spawn all player
    [PunRPC]
    public void RPC_spawnAll()
    {
        SpawnPlayer();
    }

    [PunRPC]
    public void RPC_LobyMessage()
    {
        HostText.text = "Tidak dapat Bergabung, Game Sedang Berlangsung";
        HostText.color = Color.red;
    }
}
