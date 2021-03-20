using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
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
        private List<int> referenceAiInputs;

        #region Public

        public override void SetUp(CrowdConfig config, int numberOfPlayers, int numberOfReferenceAis, InputReceiver receiver)
        {
            if (crowdInputReliability == null || crowdInputReliability.numberOfPlayers != numberOfPlayers)
            {
                crowdInputReliability = new CrowdInputReliability(
                    numberOfPlayers,
                    config.numberOfCommands,
                    config.reliabilityCoefficient,
                    config.agreementThreshold
                );

                evaluatorData.ResetReliabilities();
            }

            inputList = Enumerable.Range(0, crowdInputReliability.numberOfPlayers).Select(i => 0).ToList();
            inputTimeToLive = config.inputTimeToLive;
            inputReceiver = receiver;
            referenceAiInputs = Enumerable.Range(0, numberOfReferenceAis).Select(i => 0).ToList();
        }

        public override void PostInput(PlayerInput input)
        {
            if (!input.reference)
            {
                inputList[input.playerId] = input.inputId;
            }
            else
            {
                referenceAiInputs[input.playerId] = input.inputId;
            }
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

            inputList = Enumerable.Range(0, crowdInputReliability.numberOfPlayers).Select(i => 0).ToList();

            if (debug)
            {
                evaluatorData.AppendReliabilities(crowdInputReliability.playerReliabilities);
                evaluatorData.AppendInput(currentPlayerInputs, referenceAiInputs, crowdedInput);
                referenceAiInputs = Enumerable.Range(0, referenceAiInputs.Count).Select(i => 0).ToList();
            }
        }

        #endregion
    }
}