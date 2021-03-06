﻿using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using Reliability;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(EvaluatorData))]
    public class StaticFrameInputBroker : AbstractInputBroker
    {
        [SerializeField] private bool debug;

        private InputReceiver inputReceiver;
        private List<int> inputList;
        private float inputTimeToLive;
        private float frameTimer;

        #region Public

        public override void SetUp(CrowdConfig config, int numberOfPlayers, InputReceiver receiver)
        {
            if (crowdInputReliability == null || ConfigsDiffer(numberOfPlayers, config))
            {
                if (config.mockedCrowdConfig)
                {
                    crowdInputReliability = new MockCrowdInputReliability(numberOfPlayers, config.numberOfCommands);
                }
                else
                {
                    crowdInputReliability = new CrowdInputReliability(
                        numberOfPlayers,
                        config.numberOfCommands,
                        config.reliabilityCoefficient,
                        config.agreementThreshold
                    );
                }
                evaluatorData.ResetReliabilities();
            }

            this.numberOfPlayers = numberOfPlayers;
            inputList = Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();
            inputTimeToLive = config.inputTimeToLive;
            inputReceiver = receiver;
        }

        public override void PostInput(PlayerInput input)
        {
            inputList[input.playerId] = input.inputId;
        }


        #endregion

        #region Private

        private void Update()
        {
            frameTimer += Time.deltaTime;

            if (crowdInputReliability != null)
            {
                if (frameTimer >= inputTimeToLive)
                {
                    IssueInput();
                    frameTimer = 0f;
                }
            }
        }

        private void IssueInput()
        {
            int[] currentPlayerInputs = inputList.ToArray();
            int crowdedInput = crowdInputReliability.IssueCommands(currentPlayerInputs);

            inputReceiver.ApplyInput(crowdedInput);

            inputList = Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();

            if (debug)
            {
                evaluatorData.AppendReliabilities(crowdInputReliability.GetPlayerReliabilities());
                evaluatorData.AppendInput(currentPlayerInputs, crowdedInput);
            }
        }

        #endregion
    }
}