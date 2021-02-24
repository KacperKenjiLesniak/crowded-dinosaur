using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(PlayerManager))]
    public class AiManager : MonoBehaviour
    {
        [SerializeField] private AiList aiList;

        public void AddAi(float noise)
        {
            aiList.aiConfigs.Add(new AiConfig(noise));
            Debug.Log("Ai list size: " + aiList.aiConfigs.Count);
        }

        public void ClearAis()
        {
            aiList.aiConfigs.Clear();
            Debug.Log("Ai list size: " + aiList.aiConfigs.Count);
        }

        public void CreateAis()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Ai list size: " + aiList.aiConfigs.Count);
                GetComponent<PlayerManager>().CreateAIControllers(aiList.aiConfigs);
            }
        }
    }
}