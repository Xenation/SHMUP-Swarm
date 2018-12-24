using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ADD NAMESPACE



namespace Swarm
{
    public class CameraController : MonoBehaviour
    {

        public PlayerSwarm player;

        private void Awake()
        {
            
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, (player.cursor.position / 10 ) + new Vector3(0, 0, -10f), 3f* Time.deltaTime);
        }

    }
}
