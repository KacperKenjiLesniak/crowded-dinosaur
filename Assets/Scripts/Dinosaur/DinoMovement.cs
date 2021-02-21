using MutableObjects.Int;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class DinoMovement : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float jumpPower = 10f;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float speedModifier = 1.5f;
        [SerializeField] private MutableInt score;

        private int lastCheckpoint;
        private Rigidbody2D rb;
        private Animator animator;
        public bool grounded = true;
        private static readonly int Crouching = Animator.StringToHash("crouching");

        #region Public

        public void IssueJump()
        {
            grounded = false;
            photonView.RPC(nameof(Jump), RpcTarget.AllViaServer);
        }
        
        public void IssueCrouch()
        {
            photonView.RPC(nameof(Crouch), RpcTarget.AllViaServer);
        }

        #endregion

        #region Private

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (score.Value > lastCheckpoint + Constants.CHECKPOINT_LENGHT)
            {
                speed *= speedModifier;
                lastCheckpoint = score.Value;
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (photonView.IsMine)
            {
                if (other.collider.CompareTag("Ground"))
                {
                    grounded = true;
                }
            }
        }


        [PunRPC]
        private void Jump()
        {
            EndCrouch();
            rb.AddForce(new Vector2(0f, 10f) * jumpPower);
            grounded = false;
        }


        [PunRPC]
        private void Crouch()
        {
            animator.SetBool(Crouching, true); 
            Invoke(nameof(EndCrouch), 1f);
        }

        private void EndCrouch()
        {
            animator.SetBool(Crouching, false);
        }
        #endregion
    }
}