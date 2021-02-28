using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(EvaluatorData))]
    public class InputBroker : MonoBehaviour
    {
        [SerializeField] private bool debug;
        private CrowdInputReliability crowdInputReliability;
        private EvaluatorData evaluatorData;

        private InputReceiver inputReceiver;
        private Queue<PlayerInput> inputsQueue;
        private float inputTimeToLive;
        private bool scheduledInputIssue;

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
            if (crowdInputReliability != null && !scheduledInputIssue)
            {
                if (inputsQueue.Count >= crowdInputReliability.numberOfPlayers / 2 + 1)
                {
                    scheduledInputIssue = true;
                    Invoke(nameof(IssueInput), inputTimeToLive / 2);
                }

                if (inputsQueue.Count > 0 && Time.time - inputsQueue.Peek().timestamp > inputTimeToLive)
                {
                    inputsQueue.Dequeue();
                }
            }
        }

        public void SetUp(CrowdConfig config, int numberOfPlayers, InputReceiver receiver)
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
        }

        public void PostInput(PlayerInput input)
        {
            inputsQueue.Enqueue(input);
        }

        private void IssueInput()
        {
            int[] currentPlayerInputs = DequeueCurrentPlayerInputs();
            inputReceiver.ApplyInput(
                crowdInputReliability.IssueCommands(currentPlayerInputs) // TODO handle duplicated player inputs
            );
            scheduledInputIssue = false;
            if (debug)
            {
                evaluatorData.AppendReliabilities(crowdInputReliability.playerReliabilities);
                evaluatorData.AppendInput(currentPlayerInputs);
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
    }
}