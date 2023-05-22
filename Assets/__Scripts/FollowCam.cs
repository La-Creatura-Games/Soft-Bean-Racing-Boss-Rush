using UnityEngine;
using Utils.ProceduralAnimation;
using Utils;

public class FollowCam : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 1;

    [SerializeField] private bool X = true;
    [SerializeField] private bool Y = true;

    Vector3 velocity;

    void LateUpdate() {
        Vector3 targetPos = new Vector3(X ? target.position.x : 0, Y ? target.position.y : 0, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, followSpeed);
    }

}
