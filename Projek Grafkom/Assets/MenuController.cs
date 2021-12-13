using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;



public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField] private string VersioName = "0.1";
    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ControlInfo;
    [SerializeField] private GameObject Role;
    [SerializeField] private GameObject Rules;
    [SerializeField] private GameObject Credits;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;
    [SerializeField] private GameObject StartButton;

    private void Start()
    {
        UsernameMenu.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConectedToMaster()
    {
        //untuk sync semua player di satu scene yang sama
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

//Untuk mengecek apakah username yang dimasukkan lebih dari atau sama dengan 3 karater
    public void ChangeUsernameInput()
    {
        if (UsernameInput.text.Length >= 3)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void SetUserName()
    {
        UsernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.text;
    }

//untuk membuat room dengan masimal 10 player
    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() {MaxPlayers = 10}, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom (JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

//ketika join room ditekan maka akan masuk kedalam loby
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }


//Menampilkan info permainian dan credits
    public void RoleInfo(){
        ControlInfo.SetActive(false);
        Role.SetActive(true);
        
    }
    public void RulesInfo(){
        Role.SetActive(false);
        Rules.SetActive(true);
        
    }

    public void ManControl(){
        ControlInfo.SetActive(true);
    }
    public void CreditsDev(){

        Credits.SetActive(true);
    }

    public void keluarPanduCredit(){
        Credits.SetActive(false);
        Role.SetActive(false);
        Rules.SetActive(false);
    }
}
