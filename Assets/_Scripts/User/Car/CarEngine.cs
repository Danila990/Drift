using UnityEngine;

namespace _Project
{
    public class CarEngine : MonoBehaviour
    {
        [Header("Настройки Передвижение")]
        [SerializeField] private float _moveSpeed = 50;
        [SerializeField] private float _maxSpeed = 25;
        [SerializeField] private float _steerAngle = 10;
        [SerializeField, Range(1, 5)] private float _driftMultiplier = 1.3f;
        [SerializeField] private float _driftMinSpeed = 12;

        [Header("Настройки ручника")]
        [SerializeField,Range(0.98f, 5)] private float _handbrakeDrag = 1.5f;
        [SerializeField, Range(0, 5)] private float _handbrakeAngularDrag = 0.01f;
        [SerializeField] private float _handbrakeDragChangeSpeed = 5f;

        [Header("Настройки эффектов")]
        [SerializeField] private ParticleSystem _leftTireSmoke;
        [SerializeField] private TrailRenderer _leftTireSkid;
        [SerializeField] private ParticleSystem _rightTireSmoke;
        [SerializeField] private TrailRenderer _rightTireSkid;

        [Header("Настройки Звука")]
        [SerializeField] private AudioSource _carEngineSound;
        [SerializeField] private AudioSource _tireScreechSound;

        private Rigidbody _rigidbody;

        private float _startCarEngineSoundPitch;

        private float _normalHandbrakeDrag;
        private float _normalHandbrakeAngularDrag;

        private float _xInput;
        private float _yInput;
        private bool _isHandbrake;
        private bool _isDrifting;

        public int Speed => (int)_rigidbody.linearVelocity.magnitude;
        public bool IsDrigtin => _isDrifting;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _normalHandbrakeDrag = _rigidbody.linearDamping;
            _normalHandbrakeAngularDrag = _rigidbody.angularDamping;
            DriftingEffect();
            EngineSound();
        }

        private void Update()
        {
            InputHandler();
            Drifting();
            DriftingEffect();
            EngineSound();
        }

        private void FixedUpdate()
        {
            Movement();
            Rotation();
            ForwardInertia();
            MoveLimit();
        }

        private void EngineSound()
        {
            float engineSoundPitch = _startCarEngineSoundPitch + Mathf.Abs(_rigidbody.linearVelocity.magnitude) / 25f;
            _carEngineSound.pitch = engineSoundPitch;

            if (_isDrifting)
            {
                if (!_tireScreechSound.isPlaying)
                    _tireScreechSound.Play();
            }
            else
                _tireScreechSound.Stop();
        }

        private void Drifting()
        {
            if(_isDrifting)
                _rigidbody.angularDamping = _handbrakeAngularDrag;
            else
                _rigidbody.angularDamping = _normalHandbrakeAngularDrag;
        }

        private void DriftingEffect()
        {
            if (_isDrifting)
            {
                _leftTireSkid.emitting = true;
                _rightTireSkid.emitting = true;
                if(!_leftTireSmoke.isPlaying && !_rightTireSmoke.isPlaying)
                {
                    _leftTireSmoke.Play();
                    _rightTireSmoke.Play();
                }

                return;
            }


            _leftTireSkid.emitting = false;
            _rightTireSkid.emitting = false;
            _leftTireSmoke.Stop();
            _rightTireSmoke.Stop();
        }

        private void Movement()
        {
            _rigidbody.linearVelocity += transform.forward * _moveSpeed * _yInput * Time.fixedDeltaTime;

            if(_isHandbrake)
                _rigidbody.linearDamping = Mathf.Lerp(_rigidbody.linearDamping, _handbrakeDrag, _handbrakeDragChangeSpeed * Time.fixedDeltaTime);
            else
                _rigidbody.linearDamping = Mathf.Lerp(_rigidbody.linearDamping, _normalHandbrakeDrag, _handbrakeDragChangeSpeed * Time.fixedDeltaTime);

        }

        private void Rotation()
        {
            float yAngle = _xInput * _rigidbody.linearVelocity.magnitude * _steerAngle;
            if (_isHandbrake)
                yAngle *= _driftMultiplier;

            Quaternion rotate = Quaternion.Euler(0, yAngle * Time.fixedDeltaTime, 0);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, _rigidbody.rotation * rotate, 10 * Time.fixedDeltaTime);
        }

        private void ForwardInertia()
        {
            _rigidbody.linearVelocity = Vector3.Lerp(_rigidbody.linearVelocity.normalized, transform.forward, Time.fixedDeltaTime) * _rigidbody.linearVelocity.magnitude;
        }

        private void MoveLimit()
        {
            _rigidbody.linearVelocity = Vector3.ClampMagnitude(_rigidbody.linearVelocity, _maxSpeed + 1);
        }

        private void InputHandler()
        {
            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");
            _isHandbrake = Input.GetKey(KeyCode.Space);
            _isDrifting = _isHandbrake && _rigidbody.linearVelocity.magnitude > _driftMinSpeed;
        }
    }
}