using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerShooting : MonoBehaviour
{

    public bool canShoot = true;

    [SerializeField] private Transform gunAnchor;
    [SerializeField] private Transform gunTransform;

    [SerializeField] private GunObject gun;
    [SerializeField] private float chargeSpeed = 3;
    
    private bool isCharging;
    private bool isMax;
    private float chargeAmount = 0;

    Camera cam;

    private Vector2 gunDirection {
        get {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return mousePosition - gunAnchor.position;
        }
    }

    private Quaternion gunRotation {
        get {
            Math.RotationToDirection(gunDirection, out Quaternion output);
            return output;
        }
    }

    // Input
    private bool chargeInput;

    #region Singleton
    
    static public PlayerShooting Instance = null;
    void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    
    #endregion

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if (isCharging && !isMax)
        {
            chargeAmount += chargeSpeed * Time.deltaTime;
            chargeAmount = Mathf.Clamp01(chargeAmount);
            if (chargeAmount == 1)
            {
                MaxCharge();
            }
        }

        RotateGun();
    }

    private void RotateGun() {
        gunAnchor.rotation = gunRotation;
    }

    private void StartCharging() {
        isCharging = true;
    }

    private void Fire() {
        Rigidbody2D bulletBody = Instantiate(gun.bulletPrefab, gunTransform.position, gunRotation);
        bulletBody.AddRelativeForce(Vector2.up * gun.bulletSpeedScale * chargeAmount, ForceMode2D.Impulse);

        isCharging = false;
        isMax = false;
        chargeAmount = 0;
    }

    private void MaxCharge() {
        isMax = true;
        print("Max");
    }

    void OnFire(InputValue value) {
        chargeInput = value.Get<float>() > 0;

        if (chargeInput)
        {
            StartCharging();
        } else
        {
            Fire();
        }
    }

}
