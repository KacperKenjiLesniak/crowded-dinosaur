using System;
using DefaultNamespace;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement))]
public class DinoInputSender : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

    public void SendJumpInput(int actorNumberOffset)
    {
        photonView.RPC(nameof(JumpInfo), RpcTarget.MasterClient, actorNumberOffset);
    }
    
    [PunRPC]
    private void JumpInfo(int actorNumberOffset, PhotonMessageInfo info)
    {
        var playerNumber = info.Sender.ActorNumber - 1 + actorNumberOffset;
        Debug.Log("Player " + playerNumber + " has jumped!");
        playerInputGameEvent.RaiseGameEvent(
            new PlayerInput(
                playerNumber,
                PhotonNetwork.NickName,
                Constants.INPUT_JUMP_ID));
    }
}