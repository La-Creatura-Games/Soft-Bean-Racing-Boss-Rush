using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public bool gravityEnabled = true;
    public bool canRun = true;
    public bool canJump = true;
    [HideInInspector] public int direction = 1;

    [Header("Running")]
    [SerializeField] private float maxSpeed = 15;
    public float acceleration = 500;
    public float decceleration = 600;
    [SerializeField] private float duckMaxSpeed = 5;
    [SerializeField] private ParticleSystem dustParticles;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private Vector2 overGroundBox = new Vector2(0.9f, 1);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Optional<float> fallingGravityScaleMultiplier = new Optional<float>(2);
    
    float originalGravityScale;
    private bool isTouchingGround = false;
    private Collider2D isOverGround;
    
    [Header("Animation")]
    [SerializeField] private Transform graphics;
    [SerializeField] private Optional<float> tilt = new Optional<float>(15);
    [SerializeField] private float tiltSpeed = 1;
    [SerializeField] private float squashAndStretchSpeed = 5;
    [SerializeField] private Vector2 landSquash = new Vector2(0.8f, -0.8f);
    [SerializeField] private Vector2 jumpStretch = new Vector2(-0.8f, 0.8f);
    [SerializeField] private Vector2 duckSquash = new Vector2(0.5f, -0.5f);

    private float targetTilt = 0;
    private float speed;

    // Input values
    private int movementInput;
    private int jumpInput;
    private int duckInput;

    Rigidbody2D playerBody;

    #region Singleton
    
    static public PlayerMovement Instance = null;
    void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    
    #endregion

    void Start() {
        playerBody = GetComponent<Rigidbody2D>();

        speed = maxSpeed;
        originalGravityScale = playerBody.gravityScale;
    }
    
    void Update() {
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);
        graphics.rotation = Quaternion.RotateTowards(graphics.rotation, targetRotation, tiltSpeed);

        Vector2 scale = duckInput > 0 ? Vector2.one + duckSquash : Vector2.one;
        graphics.localScale = Vector2.MoveTowards(graphics.localScale, scale, squashAndStretchSpeed * Time.deltaTime);
    }

    void FixedUpdate() {
        // Checks();

        if (canRun) Run();

        Gravity();

        if (jumpInput > 0 && isTouchingGround && isOverGround != null && canJump)
        {
            Jump();
        }
    }

    private void Run() {
        float targetSpeed = movementInput * speed;
        float speedDiff = targetSpeed - playerBody.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float moveForce = Mathf.Pow(Mathf.Abs(speedDiff) * accelerationRate, 0.96f) * Mathf.Sign(speedDiff);

        playerBody.AddForce(moveForce * Vector2.right * Time.fixedDeltaTime);
    }

    private void Jump() {
        playerBody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        graphics.localScale = Vector2.one + jumpStretch;
    }

    private void Gravity() {
        if (gravityEnabled)
        {
            if (playerBody.velocity.y < 0)
            {
                if (fallingGravityScaleMultiplier.enabled)
                    playerBody.gravityScale = originalGravityScale * fallingGravityScaleMultiplier.value;
            } else
            {
                playerBody.gravityScale = originalGravityScale;
            }
        } else
        {
            playerBody.gravityScale = 0;
        }
    }

    private void Checks() {
        isOverGround = Physics2D.OverlapBox(transform.position, overGroundBox, 0, groundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Checks();
            
            if (isOverGround == collision.collider)
            {
                isTouchingGround = true;
                graphics.localScale = Vector2.one + landSquash;
                if (dustParticles != null) dustParticles.Play();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isOverGround == collision.collider)
            {
                isTouchingGround = false;
                if (dustParticles != null) dustParticles.Stop();
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, overGroundBox);
    }

    // Input
    void OnMovement(InputValue value) {
        movementInput = (int)value.Get<float>();
        if (movementInput != 0) direction = movementInput;

        if (tilt.enabled) targetTilt = -movementInput * tilt.value;
    }

    void OnJump(InputValue value) {
        jumpInput = (int)value.Get<float>();
    }   

    void OnDuck(InputValue value) {
        duckInput = (int)value.Get<float>();

        if (duckInput > 0)
        {
            canJump = false;
            speed = duckMaxSpeed;
        } else
        {
            canJump = true;
            speed = maxSpeed;
        }
    }

}
