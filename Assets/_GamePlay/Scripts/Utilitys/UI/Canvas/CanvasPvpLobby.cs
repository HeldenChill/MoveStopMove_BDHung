using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoveStopMove.Manager;
using Photon.Pun;

public class CanvasPvpLobby : UICanvas
{
    [SerializeField]
    protected InputField createInput;
    [SerializeField]
    protected InputField joinInput;
    [SerializeField]
    protected Button createButton;
    [SerializeField]
    protected Button joinButton;
    [SerializeField]
    protected Button startButton;
    [SerializeField]
    protected Text statusTxt;

    private void Awake()
    {
        statusTxt.text = "Status: Lobby";
        createButton.onClick.AddListener(OnClickCreateRoom);
        joinButton.onClick.AddListener(OnClickJoinRoom);
        startButton.onClick.AddListener(OnClickStartGame);
        NetworkManager.Inst.ClearEvent();
        //NetworkManager.Inst._OnJoinedRoom += () => SceneManager.Inst.LoadPhotonScene(GAMECONST.INIT_PVP_RESOUCRCES_SCENE);
        GameplayManager.Inst.GameMode = GAMECONST.GAMEPLAY_MODE.STANDARD_PVP;       
        NetworkManager.Inst._OnJoinedRoom += () =>
        {
            if (PhotonNetwork.IsMasterClient)
                PrefabManager.Inst.ChangeMode(GAMECONST.GAMEPLAY_MODE.STANDARD_PVP);
            statusTxt.text = "Status: Room";           
        };       
    }

    private void OnClickJoinRoom()
    {
        NetworkManager.Inst.JoinRoom(joinInput.text);
    }

    protected void OnClickCreateRoom()
    {
        NetworkManager.Inst.CreateRoom(createInput.text);
    }

    protected void OnClickStartGame()
    {
        SceneManager.Inst.LoadPhotonScene(GAMECONST.STANDARD_PVP_SCENE);
    }
}
