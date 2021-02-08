using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Evaluator
{
    public class EvaluatorData
    {
        private List<List<float>> playerReliabilitiesData;
        private List<List<int>> playerInputData;

        public EvaluatorData()
        {
            playerReliabilitiesData = new List<List<float>>();
            playerInputData = new List<List<int>>();
        }

        public void AppendReliabilities(List<float> playerReliabilities)
        {
            var reliabilitiesString = playerReliabilities
                .Select(f => f + "")
                .Aggregate((i, j) => i + "," + j);

            Debug.Log("Current reliabilities: " + reliabilitiesString);

            playerReliabilitiesData.Add(playerReliabilities);
        }
        
        public void AppendInput(List<int> playerInput)
        {
            playerInputData.Add(playerInput);
        }

        public void SaveData()
        {
            Debug.Log("Saving data in: " + Application.dataPath + "/Evaluator");
            using var file1 = File.CreateText(Application.dataPath + "/Evaluator.txt");
            foreach (var row in playerReliabilitiesData)
            {
                file1.WriteLine(row.Select(f => f + "").Aggregate((i, j) => i + "," + j) + ";");
            }     
            
            using var file2 = File.CreateText(Application.dataPath + "/EvaluatorInput.txt");
            foreach (var row in playerInputData)
            {
                file2.WriteLine(row.Select(f => f + "").Aggregate((i, j) => i + "," + j) + ";");
            }
        }
    }
}