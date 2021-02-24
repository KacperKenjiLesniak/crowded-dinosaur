using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using DefaultNamespace.AI;
using MutableObjects.Int;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Color> playerColors;

    [SerializeField] private Vector3 startingPosition = new Vector3(0f, -3f, 0f);

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            CreateController();
            CreateCrowdedController();
            if (PhotonNetwork.IsMasterClient)
            {
                var arrowSpawner = PhotonNetwork.Instantiate(
                    Path.Combine("PhotonPrefabs", "ArrowSpawner"),
                    new Vector3(0, 2f, 0),
                    Quaternion.identity
                );
                arrowSpawner.transform.parent = Camera.main.transform;
            }
        }
    }

    void CreateController()
    {
        var dino = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerDino"), startingPosition,
            Quaternion.identity);
        dino.GetComponent<DinoMovement>().SetColor(playerColors[photonView.CreatorActorNr - 1]);
    }

    public void CreateAIControllers(List<AiConfig> aiConfigs)
    {
        for (var i = 0; i < aiConfigs.Count; i++)
        {
            var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AIDino"), startingPosition,
                Quaternion.identity);
            dinoAI.GetComponent<AIDinoController>().Configure(i, aiConfigs[i].jumpNoise);
            dinoAI.GetComponent<DinoMovement>().SetColor(playerColors[PhotonNetwork.CurrentRoom.PlayerCount + i]);
        }
    }

    void CreateCrowdedController()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CrowdedDino"), startingPosition,
                Quaternion.identity);
        }
    }
}