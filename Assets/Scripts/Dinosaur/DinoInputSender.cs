using System;
using DefaultNamespace;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement))]
public class DinoInputSender : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

    public void SendInput(int actorNumberOffset, int inputId)
    {
        photonView.RPC(nameof(InputInfo), RpcTarget.MasterClient, actorNumberOffset, inputId);
    }
    
    [PunRPC]
    private void InputInfo(int actorNumberOffset, int inputId, PhotonMessageInfo info)
    {
        var playerNumber = info.Sender.ActorNumber - 1 + actorNumberOffset;
        playerInputGameEvent.RaiseGameEvent(
            new PlayerInput(
                playerNumber,
                PhotonNetwork.NickName,
                inputId,
                Time.time));
    }
}