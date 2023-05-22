using UnityEngine;
using Utils.ProceduralAnimation;
using Utils;

public class FollowCam : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 1;

    Vector3 velocity;

    void LateUpdate() {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, followSpeed);
    }

}
