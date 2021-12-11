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

// Menentukan kecepatan gerak player
    public float MoveSpeed;

//Tombol assign presensi
    public Button alpaButton;
    public Button hadirButton;
    public GameObject alpaBtn;
    public GameObject hadirBtn;

//Colider dengan other player
    private static Collider2D Lain;

//Audio Game
    public GameObject AudioAIM;
    public GameObject AudioHadir;
    public GameObject AudioALPA;

//id yang jadi dosen
    public bool isDosen = false;
    public bool isAlpa = false;

//Status dan role player
    public GameObject status;
    public GameObject Roles;
    public Text StatusText;
    public Text role;


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
            Roles.SetActive(true);
            CheckInput();

            //Unutk menampilkan tulisan role pada layar player
            if(gameObject.tag == "Dosen")
            {
                role.color = Color.red;
                role.text = "Role Dosen";
            }
            else if(gameObject.tag != "Dosen")
            {
                role.color = Color.green;
                role.text = "Role Mahasiswa";
            }

            //Mengecek apakah dia dosen atau bukan, ketika dia doosen maka yang aktif adalah tombol alpa 
            // ketika dia mahasisawa maka yang aktif adalah tombol hadir
            if (gameObject.tag == "Dosen") {
                hadirBtn.SetActive(false);
                alpaBtn.SetActive(true);
            }
            else if (gameObject.tag == "Teman") {
                hadirBtn.SetActive(true);
            }          
        }
    }

// ini untuk mengetahui pergerkaan player
    private void CheckInput() {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        transform.position += move * MoveSpeed * Time.deltaTime;

        //Ketika Role mahasiswa dan role dosen saling berdekatan maka tombol akan dapat ditekan 
         if (Input.GetKeyDown(KeyCode.Return) && (alpaButton.interactable==true || hadirButton.interactable==true)) {
            // Debug.Log("ADA Enter");
            if(isDosen==true) {
                // Debug.Log("Lain = " + Lain);
                PlayerScript Dkt = Lain.GetComponent<PlayerScript>();
                // Debug.Log(Dkt);
                // Debug.Log(Dkt.TampilanPhoton.Owner.NickName);
                Dkt.TampilanPhoton.RPC("RPC_AlpaButtonClicked",RpcTarget.All);
                AudioHadir.SetActive(true);
            }
            else {
                // Debug.Log("hadirji");
                TampilanPhoton.RPC("RPC_HadirButtonClicked", RpcTarget.All);
                AudioHadir.SetActive(true);
            }
        }
        //membunyikan suaar aim ketika enter ditekan
        if (Input.GetKeyDown(KeyCode.Return)) {
            AudioAIM.SetActive(true);
        }

        //Mengecek tombol WASD untuk mengetahui pergerakan player
        if (Input.GetKeyDown(KeyCode.A)) {
            TampilanPhoton.RPC("FlipTrue", RpcTarget.All);
            // Debug.Log("ADA kiri");
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            TampilanPhoton.RPC("FlipFalse", RpcTarget.All);
            // Debug.Log("ADA kanan");
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ) {
            anim.SetBool("isRunning",true);
        }
        else {
            anim.SetBool("isRunning",false);
        } 
    }



//Saat player saling berdekatan maka tombol akan aktif
    public void OnTriggerEnter2D(Collider2D other) {
        Lain = other ;
        if(other.gameObject.tag=="Teman" && isDosen==true) {
            alpaButton.interactable = true;
            // Debug.Log("Alpa");
            // Debug.Log(other.gameObject.tag);
        }
        else if (other.gameObject.tag == "Dosen" && isDosen == false && gameObject.tag == "Teman") {
            hadirButton.interactable = true;
            Debug.Log("Hadir");
            // StatusText.text = "Hadir";
            // Debug.Log(other.gameObject.tag);
        }
        else {
            alpaButton.interactable = false;
            hadirButton.interactable = false;
            // Debug.Log(other.gameObject.tag);
        }

        
    }
// Ketika sal;ing berjauhan maka tombol akan mati
    public void OnTriggerExit2D(Collider2D other) {
            alpaButton.interactable = false;
            hadirButton.interactable = false;
            Debug.Log("jauh");
            AudioAIM.SetActive(false);
            AudioALPA.SetActive(false);
            AudioHadir.SetActive(false);
    }

//mengirimkan pergerakan player kita untuk diterima oleh player lain
        [PunRPC]
    private void FlipTrue() {
         sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse() {
         sr.flipX = false;
    }

//Mengirimkan ke seluruh player kalau player lain alpa atau hadir
    [PunRPC]
    public void RPC_AlpaButtonClicked() {
      
        status.SetActive(true);
        gameObject.tag = "Alpa";
        AudioALPA.SetActive(true);
    }

    [PunRPC]
    public void RPC_HadirButtonClicked() {
        // Debug.Log("Berrhasil");
        StatusText.text = "Hadir";
        StatusText.color = Color.green;
        status.SetActive(true);
        gameObject.tag = "Hadir";
        // gameObject.tag = "Selesai";
    }

// Mengseet Dosen
    [PunRPC]
    public void RPC_SetDosen(int idDsn) {
        gameObject.tag = "Dosen";
        isDosen = true;
        Debug.Log("SAYA DOSENNN id = " + idDsn + TampilanPhoton.Owner.NickName);
    }
}
