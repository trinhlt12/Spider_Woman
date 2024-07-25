using System;
using System.Security.Cryptography;
using UnityEngine;

namespace SFRemastered.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHittable
    {
        [SerializeField] public float maxHealth = 100f;
        private float currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeHit(float damage, Vector3 hitPoint, Vector3 impactDirection)
        {
            currentHealth -= damage;
            
            Debug.Log("Enemy hit! Current Health:" + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                ApplyKnockBack(impactDirection);
            }
        }

        private void ApplyKnockBack(Vector3 impactDirection)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(impactDirection * 10f, ForceMode.Impulse);
            }
        }

        private void Die()
        {
            Debug.Log("Enemey died!");
            
            //Destroy(gameObject);
        }
    }
}