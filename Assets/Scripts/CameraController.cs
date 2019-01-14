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
        public float distance = 5.0f;

        public bool newCamera = false;

        private void Awake()
        {
            
        }

        private void Update()
        {

            if (newCamera)
            {
                Vector3 dir = player.cursor.transform.position;
                dir.Normalize();

                Vector3 decal = -dir * 2;

                float scal = dir.x * 1 + dir.x * 0;
                float angle = Mathf.Acos(scal);

                dir *= distance;
                transform.position = new Vector3(dir.x + decal.x, dir.y + decal.y, -10);

                if (dir.y < 0)
                {
                    angle *= -1;
                }
                transform.rotation = Quaternion.EulerAngles(transform.rotation.x, transform.rotation.y, angle);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, (player.cursor.position / distanceRatio) + new Vector3(0, 0, -10f), 3f * Time.deltaTime);

            }


        }

    }
}
