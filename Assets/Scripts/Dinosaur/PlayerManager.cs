using System;
using System.IO;
using DefaultNamespace.AI;
using MutableObjects.Int;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPosition = new Vector3(0f, -3f, 0f);
    [SerializeField] private MutableInt numberOfAIs;

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

        if (PhotonNetwork.IsMasterClient)
        {
            CreateAIControllers();
        }
    }

    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Dino"), startingPosition, Quaternion.identity);
    }

    void CreateAIControllers()
    {
        for (var i = 0; i < numberOfAIs.Value; i++)
        {
            var dinoAI = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AIDino"), startingPosition, Quaternion.identity);
            dinoAI.GetComponent<AIDinoController>().aiIndex = i;
        }
    }

    void CreateCrowdedController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CrowdedDino"), startingPosition, Quaternion.identity);
    }
}