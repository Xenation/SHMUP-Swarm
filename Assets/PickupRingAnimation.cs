using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    public class PickupRingAnimation : MonoBehaviour
    {

        private Pickups parent;
        // Start is called before the first frame update
        void Start()
        {
            parent = GetComponentInParent<Pickups>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void stopInPArent()
        {
            parent.stop();
        }
    }
}