using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;  


public class PlayerScript : MonoBehaviourPunCallbacks {
    public PhotonView TampilanPhoton;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public Text PlayerNameText;
    public GameObject lampu;

    public bool IsGrounded = false;
    public float MoveSpeed;

    public Button absenButton;
    public Text absenText;
    public GameObject absen;
    private static Collider2D Lain;
    // public GameObject Kiilled;


    // public MasterClient masterClient;

    //id yang jadi dosen
    public bool isDosen = false;
    public bool isAlpa = false;
    // public int jadiDosen;
    public GameObject status;
    public Text StatusText;


    private void Awake() {
        TampilanPhoton = GetComponent<PhotonView>();
        
        if (TampilanPhoton.IsMine) {
            PlayerCamera.SetActive(true);
            lampu.SetActive(true);
            PlayerNameText.text = PhotonNetwork.NickName;
            Debug.Log(PlayerNameText.text);
            Debug.Log(PhotonNetwork.LocalPlayer);
        }
        else {
            PlayerNameText.text = TampilanPhoton.Owner.NickName;
            PlayerNameText.color = Color.cyan;
            Debug.Log(PlayerNameText.text);
            Debug.Log(PhotonNetwork.LocalPlayer);
        }
    }
   

    private void Update() {
        
        if(TampilanPhoton.IsMine) {
            absen.SetActive(true);
            
            CheckInput();
            // Kalau Dosen warnanya merah
            if(isDosen==true) {
                absenText.text = "ALPA";
                absenText.color = Color.red;
            }
            else {
                absenText.text = "HADIR";
                absenText.color = Color.green;
            }          
        }
    }

// ini untuk mengetahui pergerkaan player
    private void CheckInput() {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        transform.position += move * MoveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A)) {
            TampilanPhoton.RPC("FlipTrue", RpcTarget.All);
            Debug.Log("ADA kiri");
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            TampilanPhoton.RPC("FlipFalse", RpcTarget.All);
            Debug.Log("ADA kanan");
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ) {
            anim.SetBool("isRunning",true);
        }
        else {
            anim.SetBool("isRunning",false);
        } 
    }

    [PunRPC]
    private void FlipTrue() {
         sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse() {
         sr.flipX = false;
    }

    //masih mau diperbaiki
    public void OnTriggerEnter2D(Collider2D other) {
        // Session["Lain"] = other;
        Lain = other ;
        // SetDekat(Dkt);
        // Debug.Log(Dkt.PlayerNameText);
        if(other.gameObject.tag=="Teman" && isDosen==true) {
            absenButton.interactable = true;
            Debug.Log("Alpa");
            Debug.Log(other.gameObject.tag);
        }
        else if (other.gameObject.tag == "Dosen" && isDosen == false) {
            absenButton.interactable = true;
            Debug.Log("Hadir");
            // StatusText.text = "Hadir";
            Debug.Log(other.gameObject.tag);
        }
        else {
            absenButton.interactable = false;
            Debug.Log(other.gameObject.tag);
        }

        
    }

    public void OnTriggerExit2D(Collider2D other) {
            absenButton.interactable = false;
            Debug.Log("jauh");
       
    }

    // public void SetDekat(PlayerScript close){
    //     Dekat = close;
    //     Debug.Log("close : " + Dekat.TampilanPhoton.Owner.NickName);
    // }

    public void tombolAbsen() {
        // Debug.Log("MAti");
        // PlayerScript Other = Session["Lain"];
        Debug.Log(Lain);
        PlayerScript Dkt = Lain.GetComponent<PlayerScript>();
        Debug.Log(Dkt);
        Debug.Log(Dkt.TampilanPhoton.Owner.NickName);
            Dkt.TampilanPhoton.RPC("RPC_AlpaButtonClicked",RpcTarget.All);
        
        // if(TampilanPhoton.IsMine && isDosen==true) {
            // Dkt.TampilanPhoton.RPC("RPC_AlpaButtonClicked",RpcTarget.All);
        // }
        // else if(TampilanPhoton.IsMine && isDosen!=true) {
        //     PhotonView MyPview = TampilanPhoton.GetComponent<PhotonView>();
        //     MyPview.RPC("RPC_HadirButtonClicked",RpcTarget.All);
        //     Debug.Log(TampilanPhoton.Owner.NickName);
        // }
    }

    [PunRPC]
    public void RPC_AlpaButtonClicked() {
        // Debug.Log("Player :" + Dekat.PlayerNameText + "Terbunuh");
        // PlayerScript Ini = GetComponent<PlayerScript>();
        // Debug.Log(Ini.status);
        // Ini.status.SetActive(true);
        status.SetActive(true);
        gameObject.tag = "Selesai";
    }

    [PunRPC]
    public void RPC_HadirButtonClicked() {
        StatusText.text = "Hadir";
        StatusText.color = Color.green;
        status.SetActive(true);
        gameObject.tag = "Selesai";
    }




    [PunRPC]
    public void RPC_SetDosen(int idDsn) {
        gameObject.tag = "Dosen";
        isDosen = true;
        Debug.Log("SAYA DOSENNN id = " + idDsn + TampilanPhoton.Owner.NickName);
    }
}
