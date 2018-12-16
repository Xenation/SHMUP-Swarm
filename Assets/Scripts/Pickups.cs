using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Swarm
{
    public class Pickups : MonoBehaviour
    {

        public int unitsToCreate = 10;
        public GameObject unitPrefab;
        public GameObject playerSwarm;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Cursor"))
            {
                for (int i = 0; i < unitsToCreate; i++)
                {
                    GameObject tmp = Instantiate(unitPrefab, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity, playerSwarm.transform);
                    PlayerSwarm player = playerSwarm.GetComponent<PlayerSwarm>();
                    player.AddUnit(tmp);
                }
            }


        }

    }
}