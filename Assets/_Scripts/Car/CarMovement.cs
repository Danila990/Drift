using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private bool _isDebug = true;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed = 25f;
    [SerializeField] private float _rotateSpeed = 5f;

    private float _inputX;
    private float _inputY;
    private Vector3 _startCenter;

    private void Update()
    {
        _inputX = Input.GetAxis("Horizontal");
        _inputY = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity += transform.forward * _moveSpeed * _inputY * Time.fixedDeltaTime;

        float yAngle = _inputX * _rigidbody.linearVelocity.magnitude * _rotateSpeed;
        Quaternion rotate = Quaternion.Euler(0, yAngle * Time.fixedDeltaTime, 0);
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, _rigidbody.rotation * rotate, 10 * Time.fixedDeltaTime);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!_isDebug && enabled) return;

        
    }
#endif
}
