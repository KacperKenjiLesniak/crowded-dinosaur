using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Evaluator;
using DefaultNamespace.Events;
using UnityEngine;

namespace DefaultNamespace
{
    public class InputBroker : MonoBehaviour
    {
        [SerializeField] private bool debug = false;

        private InputReceiver inputReceiver;
        private CrowdInputReliability crowdInputReliability;
        private float inputTimeToLive;
        private Queue<PlayerInput> inputsQueue;
        private EvaluatorData evaluatorData;

        private void Start()
        {
            inputsQueue = new Queue<PlayerInput>();
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
            }
            inputTimeToLive = config.inputTimeToLive;
            inputReceiver = receiver;
            evaluatorData = debug ? new EvaluatorData() : null;
        }

        public void PostInput(PlayerInput input)
        {
            inputsQueue.Enqueue(input);
        }

        private void Update()
        {
            if (crowdInputReliability != null)
            {
                if (inputsQueue.Count >= crowdInputReliability.numberOfPlayers / 2)
                {
                    var currentPlayerInputs = DequeueCurrentPlayerInputs();
                    inputReceiver.ApplyInput(
                        crowdInputReliability.IssueCommands(currentPlayerInputs) // TODO handle duplicated player inputs
                    );
                    if (debug)
                    {
                        evaluatorData.AppendReliabilities(crowdInputReliability.playerReliabilities);
                        evaluatorData.AppendInput(currentPlayerInputs);
                    }
                }

                if (inputsQueue.Count > 0 && Time.time - inputsQueue.Peek().timestamp > inputTimeToLive)
                {
                    inputsQueue.Dequeue();
                }
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

        private void OnApplicationQuit()
        {
            if (debug)
            {
                evaluatorData.SaveData();
            }
        }
    }
}