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
    private Animator animator;



    void Start()
    {
       
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Pega a câmera principal automaticamente
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground") && hit.distance < 15 && InteractionManagar.instance.interacting == false) 
                {
                    agent.SetDestination(hit.point);
                    animator.SetBool("Moving", true);
                }
                else if (hit.collider.gameObject.CompareTag("Item") && InteractionManagar.instance.interacting == false)
                {
                    targetItem = hit.collider.gameObject;
                    agent.SetDestination(targetItem.transform.position);
                    clickPosition = Input.mousePosition;
                    animator.SetBool("Moving", true);
                }

                if (InteractionManagar.instance.interacting)
                {
                    InteractionManagar.instance.resetInteractions();
                    InteractionManagar.instance.interacting = false;
                }
            }
            
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("Moving", false);
                }
            }
        }

        Vector3 movementDirection = agent.velocity;
        if (movementDirection.x > 0 && transform.localScale.x > 0)
        {
            Flip();
        }
        else if (movementDirection.x < 0 && transform.localScale.x < 0)
        {
            Flip();
        }

        if (targetItem != null)
        {
            
            Vector3 playerPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 itemPos = new Vector3(targetItem.transform.position.x, 0, targetItem.transform.position.z);

            
            if (Vector3.Distance(playerPos, itemPos) <= interactionDistance)
            {
                InteractionManagar.instance.CheckInteractions(targetItem.GetComponent<Item>(), clickPosition);
                targetItem = null; 
            }
        }
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

}