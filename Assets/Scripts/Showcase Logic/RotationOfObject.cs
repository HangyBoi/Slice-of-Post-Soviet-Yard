using UnityEngine;

public class RotationOfObject : MonoBehaviour
{
    [Tooltip("Degrees per second rotation speed.")]
    public float rotationSpeed = 50f;

    [Tooltip("Axis around which to rotate (e.g., (0, 1, 0) for Y axis).")]
    public Vector3 rotationAxisUp = Vector3.up;

    [Tooltip("Coordinate space for rotation (World or Self/Local).")]
    public Space rotationSpace = Space.Self;

    void Update()
    {
        float angle = rotationSpeed * Time.deltaTime;

        transform.Rotate(rotationAxisUp, angle, rotationSpace);
    }
}
