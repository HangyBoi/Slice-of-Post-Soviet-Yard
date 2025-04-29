using UnityEngine;

/// <summary>
/// Rotates the GameObject this script is attached to around a specified axis
/// at a given speed. Designed for simulating sun movement with a Directional Light.
/// </summary>
public class SunRotator : MonoBehaviour
{
    [Header("Rotation Settings")]

    [Tooltip("The axis around which the object will rotate (in World Space). Normalize doesn't strictly matter but good practice for clarity.")]
    public Vector3 rotationAxis = Vector3.up; // Default to rotating around the world Y-axis

    [Tooltip("The speed of rotation in degrees per second.")]
    public float rotationSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation amount for this frame.
        // Time.deltaTime ensures the rotation is smooth and frame-rate independent.
        float angleThisFrame = rotationSpeed * Time.deltaTime;

        // Apply the rotation around the specified axis in World Space.
        // We use Space.World to ensure the axis of rotation is consistent
        // regardless of the object's own orientation.
        transform.Rotate(rotationAxis.normalized, angleThisFrame, Space.World);
    }

    // Optional: Draw a gizmo in the Scene view to visualize the rotation axis
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Draw a line representing the axis starting from the object's position
        // Make sure the axis is normalized for consistent gizmo length
        Gizmos.DrawLine(transform.position, transform.position + rotationAxis.normalized * 5f); // Draw a line 5 units long
        Gizmos.DrawWireSphere(transform.position + rotationAxis.normalized * 5f, 0.2f); // Draw a small sphere at the end
    }
}