using UnityEngine;

namespace _Project
{
    public class Enemy : MonoBehaviour
    {
        public float maxHealth = 100f;

        private float _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
                Destroy(gameObject);
        }
    }
}