using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonRotator : MonoBehaviour
{
    public float rotationAngle = 180f;
    public float rotationSpeed = 5f;

    private Quaternion originalRotation;
    private Quaternion targetRotation;


    private void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = originalRotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void RotateOnEnter(BaseEventData data)
    {
        targetRotation = originalRotation * Quaternion.Euler(0, 0, rotationAngle);
    }

    public void RotateOnExit(BaseEventData data)
    {
        targetRotation = originalRotation;
    }
}