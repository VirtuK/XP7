using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ClickToMove : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal
    private NavMeshAgent agent;
    private GameObject targetItem = null; // Stores the selected item
    private float interactionDistance = 2f; // Distance at which interaction happens
    private Vector3 clickPosition;



    void Start()
    {
       
        agent = GetComponent<NavMeshAgent>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Pega a câmera principal automaticamente
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground") && hit.distance < 15 && InteractionManagar.instance.interacting == false) // Certifique-se de marcar o chão com a tag "Ground"
                {
                    agent.SetDestination(hit.point);
                }
                else if (hit.collider.gameObject.CompareTag("Item") && InteractionManagar.instance.interacting == false)
                {
                    targetItem = hit.collider.gameObject;
                    agent.SetDestination(targetItem.transform.position);
                    clickPosition = Input.mousePosition;// Move to item
                }

                if (InteractionManagar.instance.interacting)
                {
                    InteractionManagar.instance.resetInteractions();
                    InteractionManagar.instance.interacting = false;
                }
            }
            
        }

        if (targetItem != null)
        {
            // Get the positions ignoring Y
            Vector3 playerPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 itemPos = new Vector3(targetItem.transform.position.x, 0, targetItem.transform.position.z);

            // Check distance only in X and Z
            if (Vector3.Distance(playerPos, itemPos) <= interactionDistance)
            {
                InteractionManagar.instance.CheckInteractions(targetItem.GetComponent<Item>(), clickPosition);
                targetItem = null; // Reset item after interaction
            }
        }
    }

}