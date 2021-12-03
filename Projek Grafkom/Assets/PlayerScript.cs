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

    // public MasterClient masterClient;

    //id yang jadi dosen
    public bool isDosen = false;
    // public int jadiDosen;

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
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Teman" && isDosen==true) {
            absenButton.interactable = true;
            Debug.Log("Alpa");
            Debug.Log(other.gameObject.tag);
        }
        else if (other.gameObject.tag == "Dosen" && isDosen == false) {
            absenButton.interactable = true;
            Debug.Log("Hadir");
            Debug.Log(other.gameObject.tag);
        }
        else {
            absenButton.interactable = false;
            Debug.Log(other.gameObject.tag);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
            absenButton.interactable = false;
            Debug.Log("jauh");
        
    }

    [PunRPC]
    public void RPC_SetDosen(int idDsn) {
        gameObject.tag = "Dosen";
        isDosen = true;
        Debug.Log("SAYA DOSENNN id = " + idDsn + TampilanPhoton.Owner.NickName);
    }
}
