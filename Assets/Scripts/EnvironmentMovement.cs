using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnvironmentMovement : MonoBehaviour
    {
        public float speed = 1f;
        private void Update()
        {
            transform.Translate(new Vector2(-speed * Time.deltaTime, 0f));
            if (transform.position.x <= -10f)
            {
                transform.position = new Vector3(10f, transform.position.y);
            }
        }
    }
}