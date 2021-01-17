using DefaultNamespace;
using GameEvents.Game;
using Photon.Pun;
using UnityEngine;

public class DinoController : MonoBehaviourPunCallbacks
{

    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private GameEvent lostGameEvent;
    
    private Rigidbody2D rb;
    private bool grounded = true;
    
    private void Awake()
    {
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
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<DeadDinoMovement>().enabled = true;
        enabled = false;
    }
}
