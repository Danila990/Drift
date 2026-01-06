using UnityEngine;

namespace Drift
{
    public class Bullet : MonoBehaviour
    {

        [SerializeField] private float _damage = 10f;
        [SerializeField] private GameObject _hitEffect;

        private Vector3 _velocity;
        private float _lifetime;
        private float _currentLifetime;

        public void Initialize(Vector3 initialVelocity, float maxLifetime)
        {
            _velocity = initialVelocity;
            _lifetime = maxLifetime;
            _currentLifetime = 0f;
        }

        private void Update()
        {
            transform.position += _velocity * Time.deltaTime;

            _currentLifetime += Time.deltaTime;
            if (_currentLifetime >= _lifetime)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                if (_hitEffect != null)
                    Instantiate(_hitEffect, transform.position, transform.rotation);

                enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}