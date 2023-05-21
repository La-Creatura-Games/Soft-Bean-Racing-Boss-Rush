using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float gravity = 5;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 5;

    Rigidbody2D playerBody;

    [Header("Animation")]
    [SerializeField] private Transform graphics;

    Quaternion graphicsOriginalRotation;

    private float moveInput;
    private float turnInput;

    void Start() {
        playerBody = GetComponent<Rigidbody2D>();
        graphicsOriginalRotation = graphics.rotation;
    }

    void FixedUpdate() {
        playerBody.AddRelativeForce(Vector2.up * speed * moveInput);
        playerBody.AddTorque(-turnInput * turnSpeed);
    }

    // Input
    void OnTurn(InputValue value) {
        turnInput = value.Get<float>();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<float>();
    }
}
