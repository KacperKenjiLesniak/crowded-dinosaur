using System;
using DefaultNamespace;
using DefaultNamespace.Events;
using GameEvents.Game;
using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

public class DinoController : MonoBehaviourPunCallbacks
{
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameEvent lostGameEvent;
    [SerializeField] private MutableVector3 dinoPosition;
    [SerializeField] private PlayerInputGameEvent playerInputGameEvent;

    private Rigidbody2D rb;
    private bool grounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Jump") && grounded)
            {
                photonView.RPC(nameof(Jump), RpcTarget.AllViaServer);
                photonView.RPC(nameof(JumpInfo), RpcTarget.MasterClient);

            }
        }
        
        dinoPosition.Value = transform.position;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (photonView.IsMine)
        {
            if (other.collider.CompareTag("Obstacle"))
            {
                lostGameEvent.RaiseGameEvent();
                photonView.RPC(nameof(Die), RpcTarget.AllViaServer);
            }

            if (other.collider.CompareTag("Ground"))
            {
                grounded = true;
            }
        }
    }

    [PunRPC]
    private void Die()
    {
        GetComponent<Animator>().enabled = false;
        enabled = false;
    }
    
    [PunRPC]
    private void Jump()
    {
        rb.AddForce(new Vector2(0f, 10f) * jumpPower);
        grounded = false;
    }
    
    [PunRPC]
    private void JumpInfo(PhotonMessageInfo info)
    {
        Debug.Log("Player " + info.Sender.ActorNumber + " has jumped!");
        playerInputGameEvent.RaiseGameEvent(new PlayerInput(info.Sender.ActorNumber - 1, Constants.INPUT_JUMP_ID));
    }
}