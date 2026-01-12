using UnityEngine;

namespace _Project
{
    public class CarEngine : MonoBehaviour
    {
        [Header("Настройки предвижения")]
        [SerializeField] private float _moveSpeed = 100;
        [SerializeField] private float _maxMoveSpeed = 25;
        [SerializeField] private float _rotateAngle = 10;
        [SerializeField, Range(0, 1)] private float _drag = 0.98f;
        [SerializeField, Range(0, 1)] private float _tractionGround = 1;
        [SerializeField] private Rigidbody _body;

        private float _inputX = 0;
        private float _inputY = 0;


        private void Update()
        {
            MoveInput();
        }

        private void FixedUpdate()
        {
            Movement();
            DragAndSpeedLimit();
            TractionGround();
        }

        private void TractionGround()
        {
            _body.linearVelocity = Vector3.Lerp(_body.linearVelocity.normalized, transform.forward, _tractionGround * Time.deltaTime) * _body.linearVelocity.magnitude;
        }

        private void DragAndSpeedLimit()
        {
            _body.linearVelocity *= _drag;
            _body.linearVelocity = Vector3.ClampMagnitude(_body.linearVelocity, _maxMoveSpeed);
        }

        private void Movement()
        {
            _body.linearVelocity += transform.forward * _moveSpeed * _inputY * Time.deltaTime;
            transform.position += _body.linearVelocity * Time.deltaTime;

            transform.Rotate(Vector3.up * _inputX * _body.linearVelocity.magnitude * _rotateAngle * Time.deltaTime);
        }

        private void MoveInput()
        {
            _inputY = Input.GetAxis("Vertical");
            _inputX = Input.GetAxis("Horizontal");
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            pos.y += 3;
            if (_body == null) return;

            Gizmos.DrawRay(pos, _body.linearVelocity.normalized * 5);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(pos, transform.forward * 5);
        }
    }
}