using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;  




public class PlayerScript : MonoBehaviourPunCallbacks
{
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
    public GameObject absen;

    // public MasterClient masterClient;

    //id yang jadi dosen
    // public bool isDosen;
    // public int jadiDosen;


    // private void Start()
    // {
        
    //     if(PhotonNetwork.IsMasterClient)
    //     {
    //         PilihDosen();
    //     }
    // }
     private void Awake() 
    {
        TampilanPhoton = GetComponent<PhotonView>();
        
        if (TampilanPhoton.IsMine)
        {
            PlayerCamera.SetActive(true);
            lampu.SetActive(true);
            PlayerNameText.text = PhotonNetwork.NickName;
            Debug.Log(PlayerNameText.text);
            Debug.Log(PhotonNetwork.LocalPlayer);
        }
        else
        {
            PlayerNameText.text = TampilanPhoton.Owner.NickName;
            PlayerNameText.color = Color.cyan;
            // if(gameObject.tag == "Dosen")
            // {
            //     PlayerNameText.color = Color.red;
            // }
            // if(gameObject.tag == "Teman")
            // {
            //     PlayerNameText.color = Color.cyan;
            // }

            Debug.Log(PlayerNameText.text);
            Debug.Log(PhotonNetwork.LocalPlayer);
        }

        

        // if(isDosen == true)
        // {
        //     PlayerNameText.color = Color.red;
        // }
        // if(gameObject.tag == "Dosen")
        // {
        //     PlayerNameText.color = Color.red;
        // }
    }
   

    private void Update()
    {
        
        if(TampilanPhoton.IsMine)
        {
            absen.SetActive(true);
            
            CheckInput();
            // Kalau Dosen warnanya merah
            
        }

        // if(PhotonNetwork.IsMasterClient && TampilanPhoton.IsMine)
        // {
        //     gameObject.tag = "Dosen";
        // }
        

    }

// ini untuk mengetahui pergerkaan player
    private void CheckInput()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        transform.position += move * MoveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            TampilanPhoton.RPC("FlipTrue", RpcTarget.All);
            Debug.Log("ADA kiri");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            TampilanPhoton.RPC("FlipFalse", RpcTarget.All);
            Debug.Log("ADA kanan");
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) )
        {
            anim.SetBool("isRunning",true);
        }
        else
        {
            anim.SetBool("isRunning",false);
        }
       
    }

    [PunRPC]
    private void FlipTrue()
    {
         sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse()
    {
         sr.flipX = false;
    }

    //masih mau diperbaiki
    void OnTriggerEnter2D(Collider2D other)
    {
        // if(gameObject.tag == "Dosen" && other.gameObject.tag=="Teman")
        if(other.gameObject.tag=="Teman")
        {
            absenButton.interactable = true;
            Debug.Log("Alpa");
            Debug.Log(gameObject.tag);
        }
        // else if (gameObject.tag == "Teman" && other.gameObject.tag=="Dosen")
        // {
        //     absenButton.interactable = true;
        //     Debug.Log("Hadir");
        //     Debug.Log(gameObject.tag);
        // }
        else
        {
            absenButton.interactable = false;
            Debug.Log(gameObject.tag);
        }

        

    }
    void OnTriggerExit2D(Collider2D other)
    {
            absenButton.interactable = false;
            Debug.Log("jauh");
        
    }

    // [PunRPC]
    // public void SetDosen()
    // {
    //     Debug.Log("SAYA DOSENNN");
    // }
    // public void PilihDosen()
    // {
    //     // jadiDosen = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
    //     // // TampilanPhoton.RPC("RPC_SyncDosen", RpcTarget.All, jadiDosen);
    //     // Debug.Log("Dosen" + jadiDosen);
    //     Debug.Log("ajdimamsimsdiiki");
        
    //     TampilanPhoton.RPC("RPC_setColorName" , RpcTarget.Others);
        
    // }

    // [PunRPC]
    // public void RPC_setColorName()
    // {

    //     gameObject.tag="Teman";

    //     if(gameObject.tag == "Dosen")
    //     {
    //         PlayerNameText.color = Color.red;
    //     }
    // }

    // void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if(stream.IsWriting)
    //     {
    //         // stream.SendText(direction);
    //         stream.SendNext(isDosen);
    //     }
    //     else 
    //     {
    //         // this.direction = (float)stream.ReceiveNext();
    //         this.isDosen = (bool)stream.ReceiveNext();
    //     }
    // }

    // public void menjadiDosen(int idDosen)
    // {
    //     if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[idDosen])
    //     {
    //         isDosen = true;
    //     }
        
    // }    

   

    // [PunRPC]
    // public void RPC_SyncDosen (int playerNumber)
    // {
    //     jadiDosen = playerNumber;
    //     menjadiDosen(jadiDosen);
    // }

}
