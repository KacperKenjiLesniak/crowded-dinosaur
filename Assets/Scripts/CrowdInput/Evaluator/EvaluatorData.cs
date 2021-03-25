using System.Collections.Generic;
using System.Linq;
using MutableObjects.Int;
using UnityEngine;

namespace DefaultNamespace.Evaluator
{
    public class EvaluatorData : MonoBehaviour
    {
        public List<LogRow<List<float>>> playerReliabilitiesData { get; private set; }
        public List<LogRow<List<int>>> playerInputData { get; private set; }

        public List<LogRow<int>> issuedInputData { get; private set; }
        
        public List<LogRow<int>> scores { get; private set; }

        public MutableInt score;
        
        private void Start()
        {
            ResetReliabilities();
        }

        public void ResetReliabilities()
        {
            playerReliabilitiesData = new List<LogRow<List<float>>>();
            playerInputData = new List<LogRow<List<int>>>();
            issuedInputData = new List<LogRow<int>>();
            scores = new List<LogRow<int>>();
        }

        public void AppendReliabilities(List<float> playerReliabilities)
        {
            string reliabilitiesString = playerReliabilities
                .Select(f => f + "")
                .Aggregate((i, j) => i + "," + j);

            Debug.Log("Current reliabilities: " + reliabilitiesString);

            playerReliabilitiesData.Add(new LogRow<List<float>>(new List<float>(playerReliabilities), Time.time));
        }

        public void AppendInput(IEnumerable<int> playerInput, int issuedInput)
        {
            playerInputData.Add(new LogRow<List<int>>(playerInput.ToList(), Time.time));
            issuedInputData.Add(new LogRow<int>(issuedInput, Time.time));
        }

        public void AddScore()
        {
            scores.Add(new LogRow<int>(score.Value, Time.time));
        }
    }
}