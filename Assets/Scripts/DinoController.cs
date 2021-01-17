using System;
using DefaultNamespace;
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
    
    private Rigidbody2D rb;
    public bool grounded = true;

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
                Debug.Log("Jumping");
                rb.AddForce(new Vector2(0f, 10f) * jumpPower);
                grounded = false;
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
}