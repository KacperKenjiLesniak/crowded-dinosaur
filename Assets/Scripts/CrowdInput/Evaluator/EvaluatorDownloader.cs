using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DefaultNamespace.AI;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Evaluator
{
    public class EvaluatorDownloader : MonoBehaviour
    {
        [SerializeField] private AiList aiList;

        private EvaluatorData evaluatorData;

        private void Start()
        {
            evaluatorData = GetComponent<EvaluatorData>();
        }

        [DllImport("__Internal")]
        private static extern void DownloadFile(byte[] array, int byteLength, string fileName);

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
            string names = PhotonNetwork.PlayerList.Select(p => p.NickName)
                .Union(aiList.aiConfigs.Select((_, index) => "AI" + index))
                .Aggregate((i, j) => i + "," + j) + ";\n";

            string playerReliabilitiesFile = evaluatorData.playerReliabilitiesData
                .Aggregate("", (current, row)
                    => current + row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n");

            string inputDataFile = evaluatorData.playerInputData
                .Aggregate("", (current, row)
                    => current + row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n");
            
            string issuedInputDataFile = evaluatorData.issuedInputData
                .Select(i => i.ToString())
                .Aggregate((i, j) => i + ";\n" + j);

            string referenceAisDataFile = evaluatorData.referenceAiData
                .Aggregate("", (current, row)
                    => current + row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n");

            string scoresDataFile = evaluatorData.scores
                .Select(i => i.ToString())
                .Aggregate((i, j) => i + ";\n" + j);
            
            Debug.Log(names + playerReliabilitiesFile + "\n<SEPARATOR>\n" + inputDataFile + "\n<SEPARATOR>\n" + issuedInputDataFile +"\n<SEPARATOR>\n" + referenceAisDataFile +"\n<SEPARATOR>\n" + scoresDataFile );
            byte[] fileBytes = Encoding.UTF8.GetBytes(names + playerReliabilitiesFile + "\n<SEPARATOR>\n" + inputDataFile + "\n<SEPARATOR>\n" + issuedInputDataFile  + "\n<SEPARATOR>\n" + referenceAisDataFile + "\n<SEPARATOR>\n" + scoresDataFile );
            DownloadFile(fileBytes, fileBytes.Length, "evaluator.txt");
        }
    }
}