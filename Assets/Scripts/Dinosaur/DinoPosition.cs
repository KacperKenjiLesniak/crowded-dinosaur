using MutableObjects.Vector3;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class DinoPosition : MonoBehaviourPunCallbacks
    {
        [SerializeField] private MutableVector3 dinoPosition;
        
        private void Awake()
        {
            dinoPosition.Value = transform.position;
        }
        
        private void Update()
        {
            photonView.RPC(nameof(UpdateDinosaurPosition), RpcTarget.All);
        }
        
        [PunRPC]
        private void UpdateDinosaurPosition()
        {
            dinoPosition.Value = transform.position;
        }
    }
}