using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class DeadDinoMovement : MonoBehaviour
    {
        public float speed = 5f;

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                transform.Translate(new Vector2(-speed * Time.deltaTime, 0f));
            }
        }
    }
}