using DefaultNamespace.Events;
using DefaultNamespace.Visualization;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    [CustomEditor(typeof(ArrowSpawner))]
    public class ArrowSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Spawn"))
            {
                ((ArrowSpawner) target).SpawnArrow(new PlayerInput(1, "name", Constants.INPUT_JUMP_ID, Time.time));
            }
        }
    }
}