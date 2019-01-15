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
        public int nbOfPickups = 0;
        [HideInInspector]public static List<Pickups> currentPickups;



        private void Start()
        {
            currentPickups = new List<Pickups>();
        }


        private int randomSign()
        {
            return Random.value < .5 ? 1 : -1;
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
                currentPickup.transform.position = new Vector2((float)randomSign() * Random.Range(2.5f, 5), (float)randomSign() * Random.Range(2.5f, 5));
            }
            nbOfPickups = currentPickups.Count;
        }

        public void spawnAt(Vector3 pos)
        {
            GameObject tmp = Instantiate(Pickup, pos, Quaternion.identity);
            Pickups currentPickup = tmp.GetComponent<Pickups>();
            currentPickup.setPlayer(Player);
            currentPickups.Add(currentPickup);
            PlayerSwarm swarm = Player.GetComponent<PlayerSwarm>();
            //tmp.transform.position = new Vector3(swarm.transform.position.x + swarm.cursor.position.x + 0.2f, swarm.transform.position.y + swarm.cursor.position.y + 0.2f, tmp.transform.position.z);
        }
    }
}