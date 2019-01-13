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
        public int maxPickups = 3;
        [HideInInspector]public static List<Pickups> currentPickups;

        private void Start()
        {
            currentPickups = new List<Pickups>();
        }

        private void FixedUpdate()
        {
            timeSpent += Time.deltaTime;

            if (timeSpent > SpawnRate && currentPickups.Count < maxPickups)
            {
                GameObject tmp = Instantiate(Pickup);
                Pickups currentPickup = tmp.GetComponent<Pickups>();
                currentPickup.setPlayer(Player);
                timeSpent = 0;
                currentPickups.Add(currentPickup);
            }       
        }
    }
}