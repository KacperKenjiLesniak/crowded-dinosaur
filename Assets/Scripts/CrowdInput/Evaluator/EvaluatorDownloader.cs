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

            string file = evaluatorData.playerReliabilitiesData
                .Aggregate("", (current, row)
                    => current + row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n");

            string file2 = evaluatorData.playerInputData
                .Aggregate("", (current, row)
                    => current + row.Select(f => f + "")
                        .Aggregate((i, j) => i + "," + j) + ";\n");

            Debug.Log(names + file + "\n<SEPARATOR>\n" + file2);
            byte[] fileBytes = Encoding.UTF8.GetBytes(names + file + "\n<SEPARATOR>\n" + file2);
            DownloadFile(fileBytes, fileBytes.Length, "evaluator.txt");
        }
    }
}