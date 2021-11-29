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
    [SerializeField] private GameObject ConnectPanel;

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;

    [SerializeField] private GameObject StartButton;


    // private void Awake()
    // {
        
    // }

    private void Start()
    {
        UsernameMenu.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConectedToMaster()
    {
        // PhotonNetwork.automaticallySyncScene = true;

        //untuk sync semua player di satu scene yang sama
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected");
    }

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

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() {MaxPlayers = 7}, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom (JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGame");
        // PhotonNetwork.LoadLevel("Lobby");
    }



}
