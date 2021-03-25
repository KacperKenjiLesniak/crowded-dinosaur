using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ColorSetter : MonoBehaviour
    {
        public void SetColor(string htmlColor)
        {
            bool success = ColorUtility.TryParseHtmlString(htmlColor, out var color);
            Debug.Log("Trying to set color " + color);
            if (success) GetComponent<Image>().color = color;
        }
    }
}