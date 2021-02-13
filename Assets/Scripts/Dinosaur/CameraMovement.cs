using System;
using MutableObjects.Vector3;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private MutableVector3 dinoPosition;

        private void FixedUpdate()
        {
            Debug.Log(dinoPosition.Value);
            transform.position = new Vector3(dinoPosition.Value.x, transform.position.y, transform.position.z);
        }
    }
}