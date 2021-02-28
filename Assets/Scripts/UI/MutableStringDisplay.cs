using MutableObjects.String;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MutableStringDisplay : MonoBehaviour
    {
        [SerializeField] private MutableString scoreText;

        private TMP_Text text;

        private void Start()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            text.text = scoreText.Value;
        }
    }
}