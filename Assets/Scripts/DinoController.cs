using System;
using GameEvents.Game;
using Photon.Pun;
using UnityEngine;

public class DinoController : MonoBehaviour
{

    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private GameEvent lostGameEvent;
    
    private Rigidbody2D rb;
    private PhotonView photonView;
    private bool grounded = true;
    
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(rb);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Jump") && grounded)
            {
                rb.AddForce(new Vector2(0f, 10f) * jumpPower);
                grounded = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided with: " + other.collider.gameObject.name);
        if (other.collider.CompareTag("Obstacle"))
        {
            lostGameEvent.RaiseGameEvent();
            GetComponent<Animator>().enabled = false;
            enabled = false;
        }
        if (other.collider.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
