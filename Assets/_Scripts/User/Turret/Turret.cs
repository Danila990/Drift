using UnityEngine;

namespace _Project
{
    public class Turret : MonoBehaviour
    {
        [Header("Настройки вращения")]
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _maxVerticalAngle = 15f;
        [SerializeField] private float _minVerticalAngle = -15f;
        [SerializeField] private Transform _rotateModelX;
        [SerializeField] private Transform _rotateModelY;

        [Header("Найстройки обнаружения")]
        [SerializeField] private float _detectionRange = 15f;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private LayerMask _obstacleLayerMask;

        [Header("Найстройки стрельбы")]
        [SerializeField] private Transform[] _firePoints;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _minFireAngle = 10;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private int _bulletDamage = 25;
        [SerializeField] private float _bulletSpeed = 40f;
        [SerializeField] private float _bulletLifetime = 5f;

        [Header("Найстройки эффектов")]

        [Header("Найстройки Звуков")]
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _fireClip;

        [field: SerializeField, Header("Настройки турели")] public TurretType turretType { get; private set; }

        private float _startFireSoundPitch;
        private Transform _currentTarget;
        private Timer _fireTimer;
        private int _firePointIndex = 0;

        private void Awake()
        {
            _fireTimer = new Timer(_fireRate);
            _startFireSoundPitch = _source.pitch;
        }

        private void Update()
        {
            FindTarget();
            RotateToTarget();
            TryToShoot();
        }

        private void FindTarget()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, _detectionRange, _enemyLayerMask);

            Transform closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider enemy in enemies)
            {
                if (CheckRayObstacles(enemy.transform))
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy.transform;
                    }
                }
            }

            _currentTarget = closestEnemy;
        }

        private bool CheckRayObstacles(Transform target)
        {
            RaycastHit hit;
            Vector3 direction = target.position - _rotateModelY.position;

            if (Physics.Raycast(_rotateModelY.position, direction.normalized, out hit, _detectionRange, _obstacleLayerMask))
            {
                if (hit.transform == target)
                    return true;

                return false;
            }

            return true;
        }

        private void RotateToTarget()
        {
            if(_currentTarget == null)
            {
                _rotateModelX.localRotation = Quaternion.RotateTowards(_rotateModelX.localRotation, Quaternion.identity, _rotationSpeed * Time.deltaTime);
                _rotateModelY.localRotation = Quaternion.RotateTowards(_rotateModelY.localRotation, Quaternion.identity, _rotationSpeed * Time.deltaTime);
                return;
            }

            Vector3 direction = _currentTarget.position - _rotateModelX.position;

            Quaternion targetRotationY = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _rotateModelX.rotation = Quaternion.RotateTowards(_rotateModelX.rotation, targetRotationY, _rotationSpeed * Time.deltaTime);

            float angleX = Mathf.Atan2(direction.y, new Vector3(direction.x, 0, direction.z).magnitude) * Mathf.Rad2Deg;
            angleX = Mathf.Clamp(angleX, _minVerticalAngle, _maxVerticalAngle);

            Quaternion targetRotationX = Quaternion.Euler(-angleX, 0, 0);
            _rotateModelY.localRotation = Quaternion.RotateTowards(_rotateModelY.localRotation, targetRotationX, _rotationSpeed * Time.deltaTime);
        }

        private void TryToShoot()
        {
            if (_currentTarget != null && _fireTimer.IsTimerEnd)
            {
                Vector3 directionToTarget = _currentTarget.position - _rotateModelY.position;
                float angle = Vector3.Angle(_rotateModelY.forward, directionToTarget);
                if (angle < _minFireAngle)
                {
                    Shoot();
                    _fireTimer.StartTime();
                }
            }
        }

        private void Shoot()
        {
            Transform firePoint = GetNextPoint();
            Bullet projectileController = Instantiate(_bulletPrefab, firePoint.position, firePoint.rotation);
            projectileController.Setup(firePoint.forward * _bulletSpeed, _bulletLifetime, _bulletDamage);
            float pitchRandom = 0.5f;
            _source.pitch = _startFireSoundPitch + Random.Range(-pitchRandom, pitchRandom);
            _source.PlayOneShot(_fireClip);
        }

        private Transform GetNextPoint()
        {
            Transform point = null;

            if (_firePoints.Length - 1 < _firePointIndex)
                _firePointIndex = 0;

            point = _firePoints[_firePointIndex];
            _firePointIndex++;
            return point;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_rotateModelX.position, _rotateModelX.position + _rotateModelX.forward * _detectionRange);
        }
    }
}