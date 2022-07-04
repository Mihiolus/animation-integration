using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform targetBone;

    public void LateUpdate()
    {
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            var mousePos = hit.point;
            var lookDir = Vector3.ProjectOnPlane(mousePos - targetBone.position, Vector3.up);
            var angle = Vector3.SignedAngle(transform.forward,lookDir, Vector3.up);
            var lookRotation = Quaternion.LookRotation(lookDir, Vector3.up);
            targetBone.rotation = Quaternion.AngleAxis(angle,Vector3.up)*targetBone.rotation;
        }
    }
}
