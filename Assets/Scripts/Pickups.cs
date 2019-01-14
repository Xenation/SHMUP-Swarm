using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


namespace Swarm
{
    public class Pickups : MonoBehaviour
    {

        public int unitsToCreate = 10;
        public GameObject unitPrefab;
        public GameObject playerSwarm;
        public Animator openAnim;
        public Animator pickAnim;
        private Collider2D coll;

        private void Start()
        {
            transform.position = new Vector2( Random.Range(2.5f, 5) ,Random.Range(2.5f, 5) );
            coll = GetComponent<Collider2D>();
            pickAnim = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerUnits"))
            {
                
                for (int i = 0; i < unitsToCreate; i++)
                {
                    Transform pos = playerSwarm.transform.GetChild(0);
                    PlayerSwarm player = playerSwarm.GetComponent<PlayerSwarm>();

                    float playerX = collision.gameObject.transform.position.x;
                    float playerY = collision.gameObject.transform.position.y;

                    GameObject tmp = Instantiate(unitPrefab, new Vector2(Random.Range(playerX-0.1f, playerX+0.1f), Random.Range(playerY-0.1f, playerY+0.1f)), Quaternion.identity, playerSwarm.transform);
                    player.AddUnit(tmp);
                    
                }
            
                //INSERER SON DE PICKUP
                pickAnim.SetTrigger("contact"); // TODO temporarly disabled to avoid console flood
                openAnim.SetTrigger("collision"); // TODO temporarly disabled to avoid console flood
                AkSoundEngine.PostEvent("Play_PickUp", gameObject);
                Destroy(coll);                
            }
        }

        

        public void stop()
        {
            Destroy(gameObject);
            Swarm.PickupSpawner.currentPickups.Remove(this);
        }

        public void setPlayer(GameObject player)
        {
            playerSwarm = player;
        }

    }
}