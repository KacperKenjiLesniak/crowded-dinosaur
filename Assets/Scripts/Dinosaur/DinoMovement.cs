using System;
using DefaultNamespace.Utils;
using GameEvents.Game;
using MutableObjects.Int;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DefaultNamespace
{
    public class DinoMovement : MonoBehaviourPunCallbacks, IPunObservable
    {
        private static readonly int Crouching = Animator.StringToHash("crouching");

        [SerializeField] private float jumpPower = 10f;
        [SerializeField] private float shortJumpPower = 7f;
        [SerializeField] public float initialSpeed = 10f;
        [SerializeField] private float speedModifier = 1.5f;
        [SerializeField] private MutableInt score;

        private Animator animator;
        private float speed = 10f;
        private Rigidbody2D rb;

        public bool grounded { get; private set; } = true;
        public bool isCrouching { get; private set; }

        #region Public

        public void SetColor(Color color)
        {
            photonView.RPC(nameof(SetColorRpc), RpcTarget.All, color.r, color.g, color.b, color.a);
        }

        public void Die()
        {
            this.Invoke(() => photonView.RPC(nameof(DieRpc), RpcTarget.All), 0.05f);
        }

        public void IssueJump(bool isShort)
        {
            grounded = false;
            photonView.RPC(nameof(Jump), RpcTarget.AllViaServer, isShort);
        }

        public void IssueCrouch()
        {
            isCrouching = true;
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
            speed = initialSpeed * (float) Math.Pow(speedModifier,
                Mathf.Floor((float) score.Value / Constants.CHECKPOINT_LENGHT));
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

        #endregion

        #region PUN

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                var networkPosition = (Vector3) stream.ReceiveNext();
                float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));

                var interpolatedNetworkPosition = new Vector3(networkPosition.x + rb.velocity.x * lag,
                    networkPosition.y, networkPosition.z);

                if (Math.Abs(interpolatedNetworkPosition.x - transform.position.x) >= 6f)
                {
                    transform.position = interpolatedNetworkPosition;
                }
            }
        }

        [PunRPC]
        private void SetColorRpc(float r, float g, float b, float a)
        {
            GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
        }

        [PunRPC]
        private void DieRpc()
        {
            GetComponent<Animator>().enabled = false;
            rb.velocity = Vector2.zero;
            enabled = false;
        }

        [PunRPC]
        private void Jump(bool isShort)
        {
            EndCrouch();
            rb.AddForce(new Vector2(0f, 10f) * (isShort ? shortJumpPower : jumpPower));
            grounded = false;
        }


        [PunRPC]
        private void Crouch()
        {
            isCrouching = true;
            animator.SetBool(Crouching, true);
            Invoke(nameof(EndCrouch), 1f);
        }

        private void EndCrouch()
        {
            isCrouching = false;
            animator.SetBool(Crouching, false);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!Equals(newPlayer, photonView.Owner))
            {
                var color = GetComponent<SpriteRenderer>().color;
                photonView.RPC(nameof(SetColorRpc), RpcTarget.All, color.r, color.g, color.b, color.a);
            }
        }

        #endregion
    }
}