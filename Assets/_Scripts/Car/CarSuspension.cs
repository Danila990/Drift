using UnityEditor;
using UnityEngine;

public class CarSuspension : MonoBehaviour
{
    private const int COMPRESSION_CLAMP = 1;

    [SerializeField] private bool _isDebug = true;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _springForce = 55.0f;
    [SerializeField] private float _suspensionLenght = 1.0f;
    [SerializeField] private Transform[] _suspensionsPoints;

    private void FixedUpdate()
    {
        foreach (Transform suspension in _suspensionsPoints)
            CalculateSuspension(suspension);
    }

    private void CalculateSuspension(Transform suspension)
    {
        Vector3 pos = suspension.position;

        if (Physics.Raycast(pos, -suspension.up, out RaycastHit hit, _suspensionLenght))
        {
            float compression = COMPRESSION_CLAMP - (hit.distance / _suspensionLenght);
            float springForce = _springForce * compression;

            _rigidbody.AddForceAtPosition(suspension.up * springForce, pos, ForceMode.Acceleration);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!_isDebug && enabled) return;

        foreach (Transform suspension in _suspensionsPoints)
        {
            Color hitCol = Color.green;
            Color missCol = Color.red;

            Vector3 pos = suspension.position;
            Vector3 downDir = -suspension.up;

            bool hit = Physics.Raycast(pos, downDir, out RaycastHit hitInfo, _suspensionLenght);

            Color rayColor = hit ? hitCol : missCol;
            Gizmos.color = rayColor;
            Gizmos.DrawRay(pos, downDir * (hit ? hitInfo.distance : _suspensionLenght));

            if (hit)
            {
                Gizmos.DrawWireSphere(hitInfo.point, 0.05f);

                Handles.Label(
                    hitInfo.point + Vector3.up * 0.12f,
                    (COMPRESSION_CLAMP - (hitInfo.distance / _suspensionLenght)).ToString("F2"),
                    new GUIStyle
                    {
                        fontSize = 12,
                        normal = { textColor = rayColor }
                    });
            }
        }
    }
#endif
}
