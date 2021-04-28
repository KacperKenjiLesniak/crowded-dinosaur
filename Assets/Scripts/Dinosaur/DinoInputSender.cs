using DefaultNamespace;
using DefaultNamespace.Events;
using MutableObjects.Bool;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement))]
public class DinoInputSender : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInputGameEvent playerInputGameEvent;
    [SerializeField] private MutableBool playerOnMasterServer;

    public void SendInput(int actorNumberOffset, int inputId, bool reference = false)
    {
        photonView.RPC(nameof(InputInfo), RpcTarget.MasterClient, actorNumberOffset, inputId, reference);
    }

    [PunRPC]
    private void InputInfo(int actorNumberOffset, int inputId, bool reference, PhotonMessageInfo info)
    {
        int playerNumber = info.Sender.ActorNumber - (playerOnMasterServer.Value ? 1 : 2) + actorNumberOffset;
        playerInputGameEvent.RaiseGameEvent(
            new PlayerInput(
                playerNumber,
                PhotonNetwork.NickName,
                inputId,
                Time.time
                ));
    }
}