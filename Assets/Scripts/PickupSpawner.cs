using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    public class PickupSpawner : MonoBehaviour
    {

        public GameObject Pickup;
        public GameObject Player;
        public int SpawnRate;
        private float timeSpent = 0;

        private void FixedUpdate()
        {
            timeSpent += Time.deltaTime;

            if (timeSpent > SpawnRate)
            {
                GameObject tmp = Instantiate(Pickup);
                Pickups currentPickup = tmp.GetComponent<Pickups>();
                currentPickup.setPlayer(Player);
                timeSpent = 0;
            }       
        }
    }
}