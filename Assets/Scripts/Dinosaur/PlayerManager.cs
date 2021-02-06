using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.AI;
using MutableObjects.Int;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPosition = new Vector3(0f, -3f, 0f);
    [SerializeField] private MutableInt numberOfAis;
    [SerializeField] private List<Color> playerColors;
    
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
        }
    }

    void CreateController()
    {
        var dino = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dino"), startingPosition, Quaternion.identity);
        dino.GetComponent<SpriteRenderer>().color = playerColors[photonView.CreatorActorNr];
    }

    public void CreateAIControllers(List<AiConfig> aiConfigs)
    {
        for (var i = 0; i < numberOfAis.Value; i++)
        {
            var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AIDino"), startingPosition, Quaternion.identity);
            dinoAI.GetComponent<AIDinoController>().Configure(i, aiConfigs[i].jumpNoise);
            dinoAI.GetComponent<SpriteRenderer>().color = playerColors[PhotonNetwork.CurrentRoom.PlayerCount + i];
        }
    }

    void CreateCrowdedController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CrowdedDino"), startingPosition, Quaternion.identity);
    }
}