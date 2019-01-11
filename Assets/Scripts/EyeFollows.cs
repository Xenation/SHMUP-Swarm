using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    public class EyeFollows : MonoBehaviour
    {

        public GameObject eye;
        public PlayerSwarm swarm;
        private bossLife bl;
        public float maxEyeDistance;


        // Start is called before the first frame update
        void Start()
        {
            bl = gameObject.GetComponentInParent<bossLife>();
        }

        // Update is called once per frame
        void Update()
        {
            if (bl.isPart)
            {
                Vector3 eyeDirection = Vector3.zero;
                Vector3 playerLocal = transform.InverseTransformPoint(swarm.cursor.transform.position);

                eyeDirection.x = playerLocal.x;
                eyeDirection.y = playerLocal.y;

                eyeDirection.Normalize();
                eyeDirection *= maxEyeDistance;
                eye.transform.localPosition = eyeDirection;

            }
            else
            {
                Vector3 eyeDirection = Vector3.zero;
                eyeDirection.x = Random.Range(-maxEyeDistance, maxEyeDistance);
                eyeDirection.y = Random.Range(-maxEyeDistance, maxEyeDistance);

                eye.transform.localPosition = eyeDirection;
            }
        }
    }
}

