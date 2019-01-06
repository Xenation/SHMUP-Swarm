using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ADD NAMESPACE



namespace Swarm
{
    public class CameraController : MonoBehaviour
    {

        public PlayerSwarm player;

        public float distanceRatio = 10.0f;

        private void Awake()
        {
            
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, (player.cursor.position / distanceRatio ) + new Vector3(0, 0, -10f), 3f* Time.deltaTime);
        }

    }
}
