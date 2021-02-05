using System;
using DefaultNamespace;
using DefaultNamespace.Events;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
public class DinoController : MonoBehaviourPunCallbacks
{
    private DinoMovement dinoMovement;
    private DinoInputSender dinoInputSender;

    private void Awake()
    {
        dinoMovement = GetComponent<DinoMovement>();
        dinoInputSender = GetComponent<DinoInputSender>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Jump") && dinoMovement.grounded)
            {
                dinoMovement.IssueJump();
                dinoInputSender.SendJumpInput(0);
            }
        }
    }
}