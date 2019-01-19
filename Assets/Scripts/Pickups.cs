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
        public PlayerSwarm playerSwarm;
        public Animator openAnim;
        private Animator pickAnim;
        private Collider2D coll;

        private void Awake()
        {
			coll = GetComponent<Collider2D>();
            pickAnim = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerUnits"))
            {
                //INSERER SON DE PICKUP
                // just a test
                pickAnim.SetTrigger("contact"); // TODO temporarly disabled to avoid console flood
                openAnim.SetTrigger("collision"); // TODO temporarly disabled to avoid console flood
                AkSoundEngine.PostEvent("Play_PickUp", gameObject);
                Destroy(coll);                
            }
        }

        

        public void stop()
        {
            //Debug.Log("before for");
            for (int i = 0; i < unitsToCreate; i++)
            {

                float playerX = playerSwarm.cursor.position.x;
                float playerY = playerSwarm.cursor.position.y;

                GameObject tmp = Instantiate(unitPrefab, new Vector2(Random.Range(playerX - 0.1f, playerX + 0.1f), Random.Range(playerY - 0.1f, playerY + 0.1f)), Quaternion.identity, playerSwarm.transform);
                playerSwarm.AddUnit(tmp);
                //Debug.Log("lalaala");
                //Debug.Log(tmp);

            }
            Destroy(gameObject);
            ScoreManager.totalPyus += unitsToCreate;
            
            Swarm.PickupSpawner.currentPickups.Remove(this);
        }

        public void setPlayer(GameObject player)
        {
            playerSwarm = player.GetComponent<PlayerSwarm>();
        }

    }
}