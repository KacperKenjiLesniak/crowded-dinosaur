using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class DeadDinoMovement : MonoBehaviourPunCallbacks
    {
        public float speed = 5f;

        private void Update()
        {
            if (photonView.IsMine)
            {
                transform.Translate(new Vector2(-speed * Time.deltaTime, 0f));
            }
        }
    }
}