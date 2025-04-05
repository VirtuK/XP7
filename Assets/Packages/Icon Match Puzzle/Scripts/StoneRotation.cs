using System.Collections;
using UnityEngine;

public class StoneRotation : MonoBehaviour
{
    private bool isFront = true;
    private bool isRotating = false;
    public float rotationSpeed = 180f; // Degrees per second

    public void FlipStone()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateStone());
        }
    }

    private IEnumerator RotateStone()
    {
        isRotating = true;
        isFront = !isFront;

        // Determine the rotation direction
        float targetAngle = isFront ? 0f : 180f;
        float currentAngle = transform.localEulerAngles.y;
        float angleToRotate = Mathf.DeltaAngle(currentAngle, targetAngle);

        float rotated = 0f;
        while (Mathf.Abs(rotated) < Mathf.Abs(angleToRotate))
        {
            float step = rotationSpeed * Time.deltaTime * Mathf.Sign(angleToRotate);
            transform.Rotate(Vector3.up, step, Space.Self);
            rotated += step;
            yield return null;
        }

        // Ensure the final rotation is exact
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetAngle, transform.localEulerAngles.z);

        isRotating = false;
    }
}