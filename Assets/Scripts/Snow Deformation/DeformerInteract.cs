using UnityEngine;

public class DeformerInteract : MonoBehaviour
{
    public Camera mainCamera;
    public Transform sphereTransform;       // Assign your Sphere GameObject here
    public float sphereYPosition = 1.5f;    // The fixed height the sphere will hover at
    public LayerMask groundPlaneLayer;      // Optional: Layer for a ground plane to raycast against

    private Plane interactionPlane;

    void Start()
    {
        if (sphereTransform == null)
        {
            Debug.LogError("Sphere Transform not assigned!");
            enabled = false;
            return;
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Try to find main camera if not assigned
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found or assigned!");
                enabled = false;
                return;
            }
        }

        // Create a horizontal plane at the sphere's desired height for raycasting
        interactionPlane = new Plane(Vector3.up, new Vector3(0, sphereYPosition, 0));
    }

    void Update()
    {
        // Create a ray from the camera through the mouse cursor
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Perform a raycast against the invisible horizontal plane
        if (interactionPlane.Raycast(ray, out float enterDistance))
        {
            // Get the point where the ray intersects the plane
            Vector3 targetWorldPosition = ray.GetPoint(enterDistance);

            // Update the sphere's position (keeping Y constant)
            sphereTransform.position = new Vector3(targetWorldPosition.x, sphereYPosition, targetWorldPosition.z);
        }
    }
}