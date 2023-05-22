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

    void OnTriggerEnter2D(Collider2D trigger) { 
        if (trigger.CompareTag("Deadly"))
        {
            Die();
        }
    }

}
