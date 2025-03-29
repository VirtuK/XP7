using UnityEngine;
[System.Serializable]
public class PlayerOutline : MonoBehaviour
{
    public GameObject outlineObject; // The duplicate player model with the outline material
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        outlineObject.SetActive(false); // Hide outline by default
    }

    void Update()
    {
        CheckIfBehindWall();
    }

    void CheckIfBehindWall()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 direction = transform.position - cameraPosition;

        RaycastHit hit;
        if (Physics.Raycast(cameraPosition, direction, out hit))
        {
            
            if (hit.collider.gameObject != gameObject)
            {
                outlineObject.SetActive(true);
                // Show the outline if behind an object
            }
            else
            {
                outlineObject.SetActive(false); // Hide if the player is visible
            }
        }
    }
}