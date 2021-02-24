using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class DinoPosition : MonoBehaviourPunCallbacks
    {
        [SerializeField] private MutableVector3 dinoPosition;
        [SerializeField] private float maxPositionDifference;

        private void Awake()
        {
            dinoPosition.Value = transform.position;
        }
        
        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                dinoPosition.Value = transform.position;
                photonView.RPC(nameof(UpdateDinosaurPosition), RpcTarget.Others, transform.position);
            }
        }
        
        [PunRPC]
        private void UpdateDinosaurPosition(Vector3 position)
        {
            dinoPosition.Value = position;
            if (Vector3.Distance(position, transform.position) > maxPositionDifference)
            {
                transform.position = position;
            }
        }
    }
}