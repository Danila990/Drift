using UnityEngine;

namespace _Project
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _hitEffect;

        private int _damage = 25;
        private Vector3 _velocity;
        private float _lifetime;
        private float _currentLifetime;

        public void Setup(Vector3 initialVelocity, float maxLifetime, int bulletDamage)
        {
            _velocity = initialVelocity;
            _lifetime = maxLifetime;
            _damage = bulletDamage;
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
            if (collision.gameObject.TryGetComponent(out IDamagable enemy))
            {
                if (_hitEffect != null)
                    Instantiate(_hitEffect, transform.position, transform.rotation);

                enemy.Damage(_damage);
            }

            Destroy(gameObject);
        }
    }
}