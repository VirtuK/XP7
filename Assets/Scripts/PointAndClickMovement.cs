using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
[System.Serializable]
public class ClickToMove : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal
    private NavMeshAgent agent;
    private GameObject targetItem = null; // Stores the selected item
    private float interactionDistance = 2f; // Distance at which interaction happens
    private Vector3 clickPosition;
    private Animator animator;

    [SerializeField] private GameObject playerDefault;
    private Vector3 clickedPosition;
    private int directionSide = -1;

    private Transform textTransform;
    private Vector3 originalTextLocalPosition;

    [SerializeField] private AudioSource audioSC;
    [SerializeField] private AudioClip footsteps;

    [SerializeField] public bool doingPuzzle = false;


    void Start()
    {
       
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Pega a câmera principal automaticamente
        }
        textTransform = MessageText.instance.getText().transform;
        originalTextLocalPosition = textTransform.localPosition;
        
    }

    void Update()
    {
        HandleMouseInput();
        HandleMovementAnimation();
        HandleItemInteraction();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !doingPuzzle)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedPosition.z = 0;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground") && hit.distance < 15 && !InteractionManagar.instance.interacting)
                {
                    SetDestination(hit.point);
                }
                else if (hit.collider.gameObject.CompareTag("Item"))
                {
                    SetItemDestination(hit.collider.gameObject);
                }

                if (InteractionManagar.instance.interacting)
                {
                    InteractionManagar.instance.resetInteractions();
                    InteractionManagar.instance.interacting = false;
                }
            }
        }
    }

    void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        animator.SetBool("Moving", true);
    }

    void SetItemDestination(GameObject item)
    {
        targetItem = item;
        agent.SetDestination(targetItem.transform.position);
        clickPosition = Input.mousePosition;
        animator.SetBool("Moving", true);
        Debug.Log("andou");
    }

    void HandleMovementAnimation()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("Moving", false);
                    VerifyMousePosition();
                    StopFootStepSound();

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

    }

    void VerifyMousePosition()
    {
        Vector3 directionToMouse = (clickedPosition - transform.position).normalized;

        if (directionToMouse.x > 0 && transform.localScale.x < 0)
        {
            Flip();  // Flip to the right
        }
        else if (directionToMouse.x < 0 && transform.localScale.x > 0)
        {
            Flip();  // Flip to the left
        }
    }

    void HandleItemInteraction()
    {
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
        Transform textTransform = MessageText.instance.getText().transform;

        Vector3 textScale = textTransform.localScale;
        textScale.x *= -1;
        textTransform.localScale = textScale;
        if(directionSide == 1) 
        {
            textTransform.localPosition = originalTextLocalPosition;
        }
        else
        {
            textTransform.localPosition = new Vector3(2.05f, 4.56f, 0);
        }


            directionSide *= -1;

    }

    public void FootStepSound()
    {
        audioSC.clip = footsteps;
        audioSC.Play();

    }

    private void StopFootStepSound()
    {
        audioSC.Stop();
    }
}