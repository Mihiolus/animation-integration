using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform _targetBone;
    [SerializeField]
    private float _rotationSpeed = 180f, _maxAngle = 90f;
    private float _previousAngle = 0f;

    public void LateUpdate()
    {
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            var mousePos = hit.point;
            var lookDir = Vector3.ProjectOnPlane(mousePos - _targetBone.position, Vector3.up);
            var angle = Vector3.SignedAngle(transform.forward, lookDir, Vector3.up);
            angle = Mathf.Clamp(angle,-_maxAngle,_maxAngle);
            angle = Mathf.MoveTowards(_previousAngle, angle,_rotationSpeed*Time.deltaTime);
            _targetBone.rotation = Quaternion.AngleAxis(angle, Vector3.up) * _targetBone.rotation;
            _previousAngle = angle;
        }
    }
}
