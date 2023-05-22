using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    Vector2 originalPosition;

    void Start() {
        originalPosition = transform.position;
    }

    private void Die() {
        transform.position = originalPosition;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            Die();
        }
    }

}
