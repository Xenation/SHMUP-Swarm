using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerShrink : MonoBehaviour
    {

        private Rigidbody2D rb;
        private PlayerSwarm swarm;

        private Vector2 velocity;



        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            swarm = GetComponentInParent<PlayerSwarm>();
        }

        private void Update()
        {
            Vector2 toCursor = swarm.cursor.position - transform.position;
            float cursorDistance = toCursor.magnitude;
            velocity = toCursor.normalized;
            if (cursorDistance < swarm.cursorRadius)
            {
                velocity *= cursorDistance / swarm.cursorRadius;
            }
            velocity *= swarm.unitSpeed;
        }

        private void FixedUpdate()
        {
            rb.velocity = velocity;
            rb.rotation = Vector2.SignedAngle(Vector2.up, velocity.normalized);
        }

        public void Die(float vibDuration = 1.0f, float vibStrength = 2.0f)
        {
            AkSoundEngine.PostEvent("Play_Death", gameObject);
            VibrationManager.AddVibrateRight(vibStrength, vibDuration);

            if(swarm.ShrinkUnits > 0)
            {
                swarm.ShrinkUnits--;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

    }
}