using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {
    public GameObject PlayerPrefab;
    // public GameObject[] nAbsen;
    // public GameObject[] nHadir;
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

    public MasterClient masterClient;

//Audio game
    public GameObject MusikIngame;
    public GameObject AudioDeploy;
    public GameObject AudioEND;


//ini untuk mengetahui jumlah oplayrt yang alpa dan hadir
    public Text nAlpa;
    public Text nHadir;
    public Text nPlayer;
    private float CountAlpa;
    private float CountHadir;
    private float CountPlayer;

    public GameObject Result;
    public Text ResultText;



    
    private void Update() {
        myPv = GetComponent<PhotonView>();
        PingText.text = "ping : " + PhotonNetwork.GetPing() + "ms";
        // JumlahPlayerdiLoby.text = "Jumlah Player : "+ PhotonNetwork.CurrentRoom.PlayerCount;
        NamaRoom.text = "LOBBY : " + PhotonNetwork.CurrentRoom.Name;

        if(PhotonNetwork.GetPing()>90)
            PingText.color = Color.red;
        else
            PingText.color = Color.green;
        
        //jika ada 2 atau lebih player di dalam loby
        if(PhotonNetwork.CurrentRoom.PlayerCount > 3 && PhotonNetwork.IsMasterClient) {
            startButton.interactable = true;
            HostText.text = "Tekan Start Untuk Memulai Game";
            HostText.color = Color.green;

        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount < 2) {
            startButton.interactable = false;
        }

        CountPlayer = (GameObject.FindGameObjectsWithTag("Alpa").Length) + (GameObject.FindGameObjectsWithTag("Hadir").Length) + (GameObject.FindGameObjectsWithTag("Teman").Length)+1;

        CountAlpa = GameObject.FindGameObjectsWithTag("Alpa").Length;
        CountHadir = GameObject.FindGameObjectsWithTag("Hadir").Length;
        
        nPlayer.text = "Player In Room : " +  CountPlayer;
        nHadir.text = "Hadir : " +  CountHadir;
        nAlpa.text = "Alpa : " + CountAlpa;
        
        CheckInput();
        AddAllActivePlayers();

        if(CountHadir > ((CountPlayer-1)/2.0)) {
            ClassSuccess();
        }
        else if((CountAlpa > ((CountPlayer-1)/2.0)) || ((CountAlpa+CountHadir == (CountPlayer-1)) && (CountAlpa == CountHadir) && (CountPlayer-1 > 0))) {
            ClassDismis();
        }
        

        
        
    }

//untuk list player
    public void AddAllActivePlayers() {
         var playerList = new StringBuilder();
         Dictionary<int, Photon.Realtime.Player> playerl = Photon.Pun.PhotonNetwork.CurrentRoom.Players;
         foreach (KeyValuePair<int, Photon.Realtime.Player> p in playerl) {
             
            playerList.Append(p.Value.NickName + "\n"); 
         }

         pList.text = playerList.ToString();
     }
     
    
    

    private void Awake() {
        GameCanvas.SetActive(true);
    }

    public void SpawnPlayer() {
        
        // Debug.Log("adakok");
        float randomValue = Random.Range(-1f, 1f);
        // PhotonNetwork.Instantiate(this.PlayerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
        Vector2 randomPos = new Vector2(this.transform.position.x *randomValue, this.transform.position.y *randomValue);
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomPos, Quaternion.identity,0);
        // PhotonNetwork.Instantiate(plr, randomPos, Quaternion.identity,0);
        GameCanvas.SetActive(false);
        MusikIngame.SetActive(true);
        AudioDeploy.SetActive(true);
        SceneCamera.SetActive(false);

        // ini untuk menginisialisasi dosen
        if(PhotonNetwork.IsMasterClient) {
            masterClient.Initialize();
        }
    }
   
    public void CheckInput() {

        // if(Input.GetKeyDown(KeyCode.Return))
        // {
        //     nHadir.text = "Hadir :" + masterClient.GetHadir();
        //     nAlpa.text = "Alpa :" + masterClient.GetAlpa();
        // }

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

    public void ClassSuccess(){
        Result.SetActive(true);
        MusikIngame.SetActive(false);
        AudioEND.SetActive(true);
        ResultText.color = Color.green;
        ResultText.text = "Class Success";


    }
    public void ClassDismis(){
        Result.SetActive(true);
        MusikIngame.SetActive(false);
        AudioEND.SetActive(true);
        ResultText.color = Color.red;
        ResultText.text = "Class Dismissed";

    }

    public void leaveRoom() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

   
    public void startBtn() {
        myPv = GetComponent<PhotonView>();
        myPv.RPC("RPC_spawnAll", RpcTarget.All);
        myPv.RPC("RPC_LobyMessage", RpcTarget.AllBufferedViaServer);
        
    }

    // spawn all player
    [PunRPC]
    public void RPC_spawnAll() {
        SpawnPlayer();
    }

    [PunRPC]
    public void RPC_LobyMessage() {
        HostText.text = "Tidak dapat Bergabung, Game Sedang Berlangsung";
        HostText.color = Color.red;
    }
}
