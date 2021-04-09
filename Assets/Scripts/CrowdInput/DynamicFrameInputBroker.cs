using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using Reliability;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(EvaluatorData))]
    public class DynamicFrameInputBroker : AbstractInputBroker
    {
        private const float ACTIVATION_THRESHOLD = 0.5f;

        private InputReceiver inputReceiver;
        private Queue<PlayerInput> inputsQueue;
        private float inputTimeToLive;
        private bool scheduledInputIssue;

        #region Public

        public override void SetUp(CrowdConfig config, int numberOfPlayers, InputReceiver receiver)
        {
            if (crowdInputReliability == null ||
                ConfigsDiffer(numberOfPlayers, config))
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
            inputTimeToLive = config.inputTimeToLive;
            inputReceiver = receiver;
        }

        public override void PostInput(PlayerInput input)
        {
            inputsQueue.Enqueue(input);
        }

        #endregion

        #region Private

        private void Start()
        {
            inputsQueue = new Queue<PlayerInput>();
        }

        private void Update()
        {
            if (crowdInputReliability != null)
            {
                var reliabilities = crowdInputReliability.GetPlayerReliabilities();
                if (!scheduledInputIssue
                    && inputsQueue.Select(input => reliabilities[input.playerId]).Sum() >=
                    reliabilities.Sum() * ACTIVATION_THRESHOLD)
                {
                    Debug.Log("Invoking issuing input with queue of size: " + inputsQueue.Count);
                    scheduledInputIssue = true;
                    Invoke(nameof(IssueInput), inputTimeToLive / 2);
                }

                if (inputsQueue.Count > 0 && Time.time - inputsQueue.Peek().timestamp > inputTimeToLive)
                {
                    inputsQueue.Dequeue();
                }
            }
        }

        private void IssueInput()
        {
            Debug.Log("Issuing input with queue of size " + inputsQueue.Count);

            int[] currentPlayerInputs = DequeueCurrentPlayerInputs();
            Debug.Log("Inputs: " +
                      currentPlayerInputs.ToList().Select(i => i.ToString()).Aggregate((a, b) => a + ',' + b));

            int crowdedInput = crowdInputReliability.IssueCommands(currentPlayerInputs);
            Debug.Log("Issued input: " + crowdedInput);

            inputReceiver.ApplyInput(
                crowdedInput // TODO handle duplicated player inputs
            );
            scheduledInputIssue = false;

            evaluatorData.AppendReliabilities(crowdInputReliability.GetPlayerReliabilities());
            evaluatorData.AppendInput(currentPlayerInputs, crowdedInput);
        }

        private int[] DequeueCurrentPlayerInputs()
        {
            var inputsList = Enumerable.Range(0, numberOfPlayers).Select(i => 0).ToList();
            while (inputsQueue.Count > 0)
            {
                var input = inputsQueue.Dequeue();
                inputsList[input.playerId] = input.inputId;
            }

            return inputsList.ToArray();
        }

        #endregion
    }
}