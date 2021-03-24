using DefaultNamespace;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement))]
public class DinoInputSender : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

    public void SendInput(int actorNumberOffset, int inputId, bool reference = false)
    {
        photonView.RPC(nameof(InputInfo), RpcTarget.MasterClient, actorNumberOffset, inputId, reference);
    }

    [PunRPC]
    private void InputInfo(int actorNumberOffset, int inputId, bool reference, PhotonMessageInfo info)
    {
        int playerNumber = info.Sender.ActorNumber - 1 + actorNumberOffset;
        playerInputGameEvent.RaiseGameEvent(
            new PlayerInput(
                playerNumber,
                PhotonNetwork.NickName,
                inputId,
                Time.time
                ));
    }
}