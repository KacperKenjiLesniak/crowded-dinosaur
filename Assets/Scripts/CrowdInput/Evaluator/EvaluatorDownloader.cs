using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace DefaultNamespace.Evaluator
{
    public class EvaluatorDownloader : MonoBehaviour
    {
        private EvaluatorData evaluatorData;
        
        [DllImport("__Internal")]
        private static extern void DownloadFile(byte[] array, int byteLength, string fileName);

        private void Start()
        {
            evaluatorData = GetComponent<EvaluatorData>();
        }

        public void SaveData()
        {
            Debug.Log("Saving data in: " + Application.dataPath + "/Evaluator");
            using var file1 = File.CreateText(Application.dataPath + "/Evaluator.txt");
            foreach (var row in evaluatorData.playerReliabilitiesData)
            {
                file1.WriteLine(row.Select(f => f + "").Aggregate((i, j) => i + "," + j) + ";");
            }     
            
            using var file2 = File.CreateText(Application.dataPath + "/EvaluatorInput.txt");
            foreach (var row in evaluatorData.playerInputData)
            {
                file2.WriteLine(row.Select(f => f + "").Aggregate((i, j) => i + "," + j) + ";");
            }
        }

        public void DownloadData()
        {
            var file = evaluatorData.playerReliabilitiesData
                .Aggregate("", (current, row)
                    => current + (row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n"));

            var file2 = evaluatorData.playerInputData
                .Aggregate("", (current, row)
                    => current + (row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n"));
            
            Debug.Log(file + "\n<SEPARATOR>\n" + file2);
            var fileBytes = Encoding.UTF8.GetBytes(file + "\n<SEPARATOR>\n" + file2);
            DownloadFile(fileBytes, fileBytes.Length, "evaluator.txt");
        }
    }
}