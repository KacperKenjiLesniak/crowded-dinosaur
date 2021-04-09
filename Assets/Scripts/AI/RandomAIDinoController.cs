using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace.AI
{
    [RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
    public class RandomAIDinoController : MonoBehaviourPunCallbacks
    {
        private int aiIndex;
        private DinoInputSender dinoInputSender;
        private DinoMovement dinoMovement;
        private Random random = new Random();

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }

            dinoMovement = GetComponent<DinoMovement>();
            dinoInputSender = GetComponent<DinoInputSender>();
            Invoke(nameof(IssueRandomInput), Math.Abs(NextFloat(3)) + 2);
        }

        void IssueRandomInput()
        {
            switch (random.Next(0, 3))
            {
                case 0:
                    dinoMovement.IssueJump(false);
                    dinoInputSender.SendInput(GetAiIndex(), Constants.INPUT_JUMP_ID);
                    break;  
                case 1:
                    dinoMovement.IssueJump(true);
                    dinoInputSender.SendInput(GetAiIndex(),
                        Constants.INPUT_SHORT_JUMP_ID);
                    break;
                case 2:
                    dinoMovement.IssueCrouch();
                    dinoInputSender.SendInput(GetAiIndex(),
                        Constants.INPUT_CROUCH_ID);
                    break;
            }
            Invoke(nameof(IssueRandomInput), Math.Abs(NextFloat(3)) + 2);
        }

        public void Configure(int index)
        {
            aiIndex = index;
        }

        float NextFloat(float scale)
        {
            double f = random.NextDouble() * 2.0 - 1.0;
            return (float) f * scale;
        }

        public void SetSeed(int seed)
        {
            random = new Random(seed);
        }
        
        private int GetAiIndex()
        {
            return aiIndex + PhotonNetwork.CurrentRoom.PlayerCount - 1;
        }
    }
}