using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.AI
{
    public class AiManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private AiList aiList;
        [SerializeField] private Vector3 startingPosition = new Vector3(0f, -3f, 0f);

        private List<Color> playerColors;

        private void Start()
        {
            playerColors = FindObjectOfType<PlayerManager>().playerColors;
        }

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
            if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                Debug.Log("Creating AI");
                for (var i = 0; i < aiList.aiConfigs.Count; i++)
                {
                    var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AIDino"), startingPosition,
                        Quaternion.identity);
                    dinoAI.GetComponent<AIDinoController>().Configure(i, aiList.aiConfigs[i].jumpNoise);
                    dinoAI.GetComponent<AIDinoController>().SetSeed(Random.Range(0, 1000000));
                    dinoAI.GetComponent<DinoMovement>().SetColor(playerColors[PhotonNetwork.CurrentRoom.PlayerCount + i]);
                }
            }
        }
    }
}