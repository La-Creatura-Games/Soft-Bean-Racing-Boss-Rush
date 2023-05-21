using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class PlayerShooting : MonoBehaviour
{

    public bool canShoot = true;

    [SerializeField] private Transform gunAnchor;
    [SerializeField] private Transform gunTransform;

    [SerializeField] private GunObject gun;

    private float nextTimeToFire = 0;

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
    private bool fireInput;

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
        RotateGun();

        if (fireInput && Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    private void RotateGun() {
        gunAnchor.rotation = gunRotation;
    }

    private void Fire() {
        Rigidbody2D bulletBody = Instantiate(gun.bulletPrefab, gunTransform.position, gunRotation);
        bulletBody.AddRelativeForce(Vector2.up * gun.bulletSpeed, ForceMode2D.Impulse);

        nextTimeToFire = Time.time + 1 / gun.fireRate;
    }

    void OnFire(InputValue value) {
        fireInput = value.Get<float>() > 0;
    }

}
