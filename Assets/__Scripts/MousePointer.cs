using UnityEngine;
using UnityEngine.InputSystem;

public class MousePointer : MonoBehaviour
{

    Camera cam;

    void Start() {
        cam = Camera.main;
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update() {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePosition;
    }

}
