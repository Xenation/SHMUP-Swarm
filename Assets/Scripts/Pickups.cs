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

        private void Start()
        {
            transform.position = new Vector2((Random.Range(0,2)*2-1)*Random.Range(1, 8) ,(Random.Range(0,2)*2-1)*Random.Range(1, 4.5f));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerUnits"))
            {
                for (int i = 0; i < unitsToCreate; i++)
                {
                    Transform pos = playerSwarm.transform.GetChild(0);
                    PlayerSwarm player = playerSwarm.GetComponent<PlayerSwarm>();

                    float playerX = pos.position.x;
                    float playerY = pos.position.y;

                    GameObject tmp = Instantiate(unitPrefab, new Vector2(Random.Range(playerX-0.1f, playerX+0.1f), Random.Range(playerY-0.1f, playerY+0.1f)), Quaternion.identity, playerSwarm.transform);
                    player.AddUnit(tmp);

                    //INSERER SON DE PICKUP
                }

                Destroy(gameObject);
            }
        }

        public void setPlayer(GameObject player)
        {
            playerSwarm = player;
        }
    }
}