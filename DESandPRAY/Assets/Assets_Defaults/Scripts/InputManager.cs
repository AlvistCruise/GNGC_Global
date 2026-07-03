using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject clickedObj;
    public bool newClick;
    void Start()
    {   
        newClick = false;
        clickedObj = null;
        mainCamera = Camera.main;
    }

    void Update()
    {
        newClick = false;
        if (Input.GetMouseButtonDown(0))
        {
            // Convert screen mouse position to 2D World coordinates
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Cast a ray at the mouse position
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            // Check if the ray hit a 2D collider
            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                clickedObj = hit.collider.gameObject;
                newClick = true;
            }
        }
    }
}
