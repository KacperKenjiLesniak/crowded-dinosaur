using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnvironmentMovement : MonoBehaviour
    {
        [SerializeField] private MutableVector3 dinoPosition;

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (transform.position.x - dinoPosition.Value.x <= -10f)
                {
                    transform.position = new Vector3(dinoPosition.Value.x + 30f, transform.position.y);
                }
            }
        }
    }
}