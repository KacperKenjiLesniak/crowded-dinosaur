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

        private CrowdInputReliability crowdInputReliability;
        private EvaluatorData evaluatorData;
        private InputReceiver inputReceiver;
        private List<int> inputList;
        private float inputTimeToLive;
        private float frameTimer;

        #region Public

        public override void SetUp(CrowdConfig config, int numberOfPlayers, InputReceiver receiver)
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
        }

        public override void PostInput(PlayerInput input)
        {
            inputList[input.playerId] = input.inputId;
        }

        #endregion

        #region Private

        private void Awake()
        {
            evaluatorData = GetComponent<EvaluatorData>();
        }

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
                evaluatorData.AppendInput(currentPlayerInputs, crowdedInput);
            }
        }

        #endregion
    }
}