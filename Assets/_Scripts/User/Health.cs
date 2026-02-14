
using System;
using UnityEngine;

namespace _Project
{
    public class Health : MonoBehaviour
    {
        public event Action<int> OnHealth;
        public event Action OnDie;

        [SerializeField] private int _maxHealth = 100;

        public int health {  get; private set; }

        private void Awake()
        {
            health = _maxHealth;
            OnHealth?.Invoke(health);
        }

        public void TakeDamage(int countDamage)
        {
            health -= countDamage;

            Math.Clamp(health, 0, _maxHealth);
            OnHealth?.Invoke(health);

            if (health <= 0)
                OnDie?.Invoke();
        }
    }
}