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

        public int mortarDmg = 15;



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
                velocity *= (cursorDistance / 2) / swarm.cursorRadius;
            }
            velocity *= swarm.unitSpeed;
        }

        private void FixedUpdate()
        {
            rb.velocity = velocity;
            if(velocity != Vector2.zero)
                rb.rotation = Vector2.SignedAngle(Vector2.up, velocity.normalized);
        }

        public void Die(float vibDuration = 1.0f, float vibStrength = 2.0f)
        {
            AkSoundEngine.PostEvent("Play_Death", gameObject);
            VibrationManager.AddVibrateRight(vibStrength, vibDuration);

            if((swarm.ShrinkUnits + swarm.units.Count) > 1)
            {
                if(swarm.ShrinkUnits > 0)
                {
                    swarm.ShrinkUnits--;
                }
                else
                {
                    swarm.units[0].Die();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void kill()
        {
            if((swarm.ShrinkUnits + swarm.units.Count) <= mortarDmg)
            {
                swarm.ShrinkUnits = 0;

                foreach (PlayerUnit unit in swarm.units)
                {
                    swarm.units.Remove(unit);
                    unit.Die();
                }

                Destroy(gameObject);
            }
            else
            {
                int dmg = mortarDmg;

                if (dmg < swarm.ShrinkUnits)
                    swarm.ShrinkUnits -= dmg;
                else
                {
                    dmg -= swarm.ShrinkUnits;
                    swarm.ShrinkUnits = 0;

                    for(int i = 0; i < dmg; i++)
                    {
                        swarm.units[i].Die();
                        swarm.units.RemoveAt(i);
                    }
                }
            }
            

        }
    }
}