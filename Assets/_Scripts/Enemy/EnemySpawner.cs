using _Project.UnityServiceLocator;
using UnityEngine;
using UnityEngine.AI;

namespace _Project
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private float _minDistanceFromPlayer = 15f;
        [SerializeField] private GameObject _enemyPrefab;

        private Timer _spawnTimer;

        [Inject] private Car _car; 

        private void Start()
        {
            _spawnTimer = new Timer(_spawnInterval);
        }

        private void Update()
        {
            if(!_spawnTimer.IsTimerEnd) return;

            Vector3 spawnPosition = GetRandomSpawnPos();
            if(spawnPosition != Vector3.zero)
            {
                GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

                if(enemy.TryGetComponent(out NavMeshAgent agent))
                {
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                }
            }

            _spawnTimer.StartTime();
        }

        private Vector3 GetRandomSpawnPos()
        {
            int maxAttempts = 20;
            for (int i = 0; i < maxAttempts; i++)
            {
                float radius = 30f;
                Vector3 randomDir = UnityEngine.Random.insideUnitCircle * radius;
                Vector3 spawnPoint = _car.transform.position + new Vector3(randomDir.x, 0, randomDir.y);

                if (!NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    continue;

                spawnPoint = hit.position;

                float distanceToPlayer = Vector3.Distance(spawnPoint, _car.transform.position);
                if (distanceToPlayer < _minDistanceFromPlayer)
                    continue;

                return spawnPoint;
            }

            return Vector3.zero;
        }
    }
}