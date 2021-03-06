﻿using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using GameEvents.String;
using MutableObjects.Bool;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Color> playerColors;

    [SerializeField] private Vector3 startingPosition = new Vector3(1f, -3f, 0f);
    [SerializeField] private Vector3 crowdedStartingPosition = new Vector3(-1f, -3f, 0f);
    [SerializeField] private StringGameEvent playerColorEvent;
    [SerializeField] private MutableBool playerOnMasterServer;
    
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(ToggleRPC), RpcTarget.All, playerOnMasterServer.Value);
                CreateArrowSpawner();
                CreateCrowdedController();
            }
            if (!PhotonNetwork.IsMasterClient || playerOnMasterServer.Value)
            {
                CreateController();
            }
        }
    }

    private static void CreateArrowSpawner()
    {
        var arrowSpawner = PhotonNetwork.Instantiate(
            Path.Combine("PhotonPrefabs", "ArrowSpawner"),
            new Vector3(0, 2f, 0),
            Quaternion.identity
        );
        arrowSpawner.transform.parent = Camera.main.transform;
    }

    private void CreateController()
    {
        var dino = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerDino"), startingPosition,
            Quaternion.identity);
        var color = playerColors[(photonView.CreatorActorNr - 1) % playerColors.Count];
        dino.GetComponent<DinoMovement>().SetColor(color);
        playerColorEvent.RaiseGameEvent("#" + ColorUtility.ToHtmlStringRGBA(color));
    }

    private void CreateCrowdedController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CrowdedDino"), crowdedStartingPosition,
            Quaternion.identity);
    }
    
    [PunRPC]
    void ToggleRPC(bool value)
    {
        playerOnMasterServer.Value = value;
    }
}