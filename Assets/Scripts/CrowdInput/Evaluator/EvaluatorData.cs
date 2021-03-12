using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Evaluator
{
    public class EvaluatorData : MonoBehaviour
    {
        public List<List<float>> playerReliabilitiesData { get; private set; }
        public List<List<int>> playerInputData { get; private set; }

        public List<int> issuedInputData { get; private set; }

        public List<List<int>> referenceAiData { get; private set; }
        
        private void Start()
        {
            ResetReliabilities();
        }

        public void ResetReliabilities()
        {
            playerReliabilitiesData = new List<List<float>>();
            playerInputData = new List<List<int>>();
            issuedInputData = new List<int>();
            referenceAiData = new List<List<int>>();
        }

        public void AppendReliabilities(List<float> playerReliabilities)
        {
            string reliabilitiesString = playerReliabilities
                .Select(f => f + "")
                .Aggregate((i, j) => i + "," + j);

            Debug.Log("Current reliabilities: " + reliabilitiesString);

            playerReliabilitiesData.Add(new List<float>(playerReliabilities));
        }

        public void AppendInput(IEnumerable<int> playerInput, IEnumerable<int> referenceAiInput, int issuedInput)
        {
            playerInputData.Add(playerInput.ToList());
            referenceAiData.Add(referenceAiInput.ToList());
            issuedInputData.Add(issuedInput);
        }
    }
}