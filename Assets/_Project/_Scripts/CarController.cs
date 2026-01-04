using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 100;
    [SerializeField] private float _maxMoveSpeed = 25;
    [SerializeField] private float _rotateAngle = 10;
    [SerializeField, Range(0, 1)] private float _drag = 0.98f;
    [SerializeField, Range(0, 1)] private float _tractionGround = 1;

    private Vector3 _moveForce;
    private float _inputX = 0;
    private float _inputY = 0;

    private void Update()
    {
        MoveInput();
        Movement();
        DragAndSpeedLimit();
        TractionGround();
    }

    private void TractionGround()
    {
        _moveForce = Vector3.Lerp(_moveForce.normalized, transform.forward, _tractionGround * Time.deltaTime) * _moveForce.magnitude;
    }

    private void DragAndSpeedLimit()
    {
        _moveForce *= _drag;
        _moveForce = Vector3.ClampMagnitude(_moveForce, _maxMoveSpeed);
    }

    private void Movement()
    {
        _moveForce += transform.forward * _moveSpeed * _inputY * Time.deltaTime;
        transform.position += _moveForce * Time.deltaTime;

        transform.Rotate(Vector3.up * _inputX * _moveForce.magnitude * _rotateAngle * Time.deltaTime);
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
        Gizmos.DrawRay(pos, _moveForce.normalized * 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(pos, transform.forward * 5);
    }
}