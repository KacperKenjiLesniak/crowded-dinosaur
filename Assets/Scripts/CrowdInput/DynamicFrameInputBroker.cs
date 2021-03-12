using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(EvaluatorData))]
    public class DynamicFrameInputBroker : AbstractInputBroker
    {
        [SerializeField] private bool debug;

        private CrowdInputReliability crowdInputReliability;
        private EvaluatorData evaluatorData;
        private InputReceiver inputReceiver;
        private Queue<PlayerInput> inputsQueue;
        private float inputTimeToLive;
        private bool scheduledInputIssue;
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

            inputTimeToLive = config.inputTimeToLive;
            inputReceiver = receiver;
            referenceAiInputs = Enumerable.Range(0, numberOfReferenceAis).Select(i => 0).ToList();
        }

        public override void PostInput(PlayerInput input)
        {
            if (!input.reference)
            {
                inputsQueue.Enqueue(input);
            }
            else
            {
                referenceAiInputs[input.playerId] = input.inputId;
            }
        }

        #endregion

        #region Private

        private void Awake()
        {
            evaluatorData = GetComponent<EvaluatorData>();
        }

        private void Start()
        {
            inputsQueue = new Queue<PlayerInput>();
        }

        private void Update()
        {
            if (crowdInputReliability != null)
            {
                var reliabilities = crowdInputReliability.playerReliabilities;
                if (!scheduledInputIssue
                    && inputsQueue.Select(input => reliabilities[input.playerId]).Sum() > reliabilities.Sum() / 2)
                {
                    Debug.Log("Invoking issuing input");
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
            Debug.Log("Issuing input");
            int[] currentPlayerInputs = DequeueCurrentPlayerInputs();
            int crowdedInput = crowdInputReliability.IssueCommands(currentPlayerInputs);

            inputReceiver.ApplyInput(
                crowdedInput // TODO handle duplicated player inputs
            );
            scheduledInputIssue = false;
            if (debug)
            {
                evaluatorData.AppendReliabilities(crowdInputReliability.playerReliabilities);
                evaluatorData.AppendInput(currentPlayerInputs, referenceAiInputs, crowdedInput);
                referenceAiInputs = Enumerable.Range(0, referenceAiInputs.Count).Select(i => 0).ToList();
            }
        }

        private int[] DequeueCurrentPlayerInputs()
        {
            var inputsList = Enumerable.Range(0, crowdInputReliability.numberOfPlayers).Select(i => 0).ToList();
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