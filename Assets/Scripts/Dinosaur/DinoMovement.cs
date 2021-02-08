using DefaultNamespace.Events;
using GameEvents.Game;
using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class DinoMovement : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float jumpPower = 10f;
        [SerializeField] private float speed = 10f;

        private Rigidbody2D rb;
        public bool grounded = true;

        #region Public

        public void IssueJump()
        {
            grounded = false;
            photonView.RPC(nameof(Jump), RpcTarget.AllViaServer);
        }

        #endregion

        #region Private

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
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
            rb.AddForce(new Vector2(0f, 10f) * jumpPower);
            grounded = false;
        }

        #endregion
    }
}