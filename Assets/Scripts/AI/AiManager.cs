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
        private static bool firstTimeInitiated = true;

        private void Start()
        {
            if (firstTimeInitiated)
            {
                ClearAis();
                firstTimeInitiated = false;
            }

            playerColors = FindObjectOfType<PlayerManager>().playerColors;
        }

        public void AddAi(AiConfig aiConfig)
        {
            aiList.aiConfigs.Add(aiConfig);
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
                    if (aiList.aiConfigs[i].isRandom)
                    {
                        var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "RandomAIDino"), startingPosition,
                            Quaternion.identity);
                        dinoAI.GetComponent<RandomAIDinoController>().Configure(i);
                        dinoAI.GetComponent<RandomAIDinoController>().SetSeed(Random.Range(0, 1000000));
                        dinoAI.GetComponent<DinoMovement>()
                            .SetColor(playerColors[(PhotonNetwork.CurrentRoom.PlayerCount + i) % playerColors.Count]);
                    }
                    else
                    {
                        var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AIDino"), startingPosition,
                            Quaternion.identity);
                        dinoAI.GetComponent<AIDinoController>().Configure(i, aiList.aiConfigs[i]);
                        dinoAI.GetComponent<AIDinoController>().SetSeed(Random.Range(0, 1000000));
                        dinoAI.GetComponent<DinoMovement>()
                            .SetColor(playerColors[(PhotonNetwork.CurrentRoom.PlayerCount + i) % playerColors.Count]);
                    }
                }
            }
        }
    }
}