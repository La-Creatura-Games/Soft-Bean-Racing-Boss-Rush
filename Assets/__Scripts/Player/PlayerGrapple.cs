using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerGrapple : MonoBehaviour
{

    [SerializeField] private Transform grapplePoint;
    [SerializeField] private SpringJoint2D springJoint;
    [SerializeField] private LineRenderer grappleRope;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private Optional<float> grappleRange = new Optional<float>(10);

    private bool grappleInput;
    private Vector2 grapplePosition;

    private bool isGrappling;
    public bool IsGrappling {
        get => isGrappling;
        set {
            isGrappling = value;

            springJoint.enabled = value;
            grappleRope.enabled = value;
            movement.decceleration = value ? 0 : originalDecceleration;
        }
    }

    Camera cam;
    PlayerMovement movement;
    float originalDecceleration;

    void Start() {
        cam = Camera.main;
        movement = GetComponent<PlayerMovement>();
        originalDecceleration = movement.decceleration;

        IsGrappling = false;
    }

    void Update() {
        DuringGrapple();
    }

    private void StartGrapple() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePos - grapplePoint.position).normalized;
        float range = grappleRange.enabled ? grappleRange.value : Mathf.Infinity;
        RaycastHit2D ray = Physics2D.Raycast(grapplePoint.position, direction, range, grappleLayer);

        if (ray)
        {
            IsGrappling = true;
            grapplePosition = ray.point;
            springJoint.connectedAnchor = grapplePosition;
        }
    }

    private void DuringGrapple() {
        if (grappleInput)
        {
            grappleRope.SetPosition(0, grapplePoint.position);
            grappleRope.SetPosition(1, grapplePosition);
        }
    }

    private void EndGrapple() {
        IsGrappling = false;
    }

    void OnGrapple(InputValue value) {
        grappleInput = value.Get<float>() > 0;

        if (grappleInput) StartGrapple();
        else EndGrapple();
    }

    void OnDrawGizmosSelected() {
        if (grappleRange.enabled)
        {
            Gizmos.DrawWireSphere(grapplePoint.position, grappleRange.value);
        }
    }

}
