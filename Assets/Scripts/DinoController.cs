using System;
using DefaultNamespace;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement))]
public class DinoController : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

    private DinoMovement dinoMovement;

    private void Awake()
    {
        dinoMovement = GetComponent<DinoMovement>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Jump") && dinoMovement.grounded)
            {
                dinoMovement.IssueJump();
                photonView.RPC(nameof(JumpInfo), RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    private void JumpInfo(PhotonMessageInfo info)
    {
        Debug.Log("Player " + info.Sender.ActorNumber + " has jumped!");
        playerInputGameEvent.RaiseGameEvent(
            new PlayerInput(
                info.Sender.ActorNumber - 1,
                PhotonNetwork.NickName,
                Constants.INPUT_JUMP_ID));
    }
}