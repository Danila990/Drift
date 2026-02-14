using UnityEngine;
using UnityEngine.AI;

namespace _Project
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        [SerializeField] private NavMeshAgent _agent;

        private Car _car;

        [field: SerializeField] public Health healthEnemy { get; private set; }


        private void Start()
        {
            _car = GameScope.Get<Car>();
            healthEnemy.OnDie += Death;
        }

        public void Damage(int damage)
        {
            healthEnemy.TakeDamage(damage);
        }

        private void Update()
        {
            _agent.SetDestination(_car.transform.position);
        }

        private void Death()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            healthEnemy.OnDie -= Death;
        }
    }
}