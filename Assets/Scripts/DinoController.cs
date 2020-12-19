using GameEvents.Game;
using UnityEngine;

public class DinoController : MonoBehaviour
{

    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private GameEvent lostGameEvent;
    
    private Rigidbody2D rb;

    private bool grounded = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(new Vector2(0f, 10f) * jumpPower);
            grounded = false;
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
