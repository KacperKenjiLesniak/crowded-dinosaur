﻿using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.AI;
using MutableObjects.Int;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Color> playerColors;

    [SerializeField] private Vector3 startingPosition = new Vector3(0f, -3f, 0f);
    [SerializeField] private MutableInt numberOfAis;

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
            var arrowSpawner = PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "ArrowSpawner"),
                new Vector3(0, 2f, 0),
                Quaternion.identity
            );
            arrowSpawner.transform.parent = Camera.main.transform;
        }
    }

    void CreateController()
    {
        var dino = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerDino"), startingPosition,
            Quaternion.identity);
        dino.GetComponent<SpriteRenderer>().color = playerColors[photonView.CreatorActorNr - 1];
    }

    public void CreateAIControllers(List<AiConfig> aiConfigs)
    {
        for (var i = 0; i < numberOfAis.Value; i++)
        {
            var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AIDino"), startingPosition,
                Quaternion.identity);
            dinoAI.GetComponent<AIDinoController>().Configure(i, aiConfigs[i].jumpNoise);
            dinoAI.GetComponent<SpriteRenderer>().color = playerColors[PhotonNetwork.CurrentRoom.PlayerCount + i];
        }
    }

    void CreateCrowdedController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CrowdedDino"), startingPosition, Quaternion.identity);
    }
}