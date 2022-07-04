using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ObjectToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = ObjectToFollow.position;
    }
}
