
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ClickToMove : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal
    private NavMeshAgent agent;
    private GameObject targetItem = null; // Stores the selected item
    public float interactionDistance = 1f; // Distance at which interaction happens
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

    public List<ParticleSystem> particles = new List<ParticleSystem>();


    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Pega a câmera principal automaticamente
        }
        textTransform = MessageText.instance.getText().transform;
        originalTextLocalPosition = textTransform.localPosition;
        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                Debug.Log("[ClickToMove] Agente realocado para NavMesh.");
            }
        }
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

            /*if (MessageText.instance.timerActive)
            {
                MessageText.instance.CloseText();
            }*/

            clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedPosition.z = 0;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer != 6)
                {


                    if (hit.collider.gameObject.CompareTag("Ground") && hit.distance < 15 && !InteractionManagar.instance.interacting)
                    {
                        SetDestination(hit.point);
                        InteractionManagar.instance.highlightedItem = null;
                    }
                    else if (hit.collider.gameObject.CompareTag("Item"))
                    {
                        SetItemDestination(hit.collider.gameObject);
                        InteractionManagar.instance.SaveInteractions(hit.collider.gameObject.GetComponent<Item>(), clickPosition);
                    }
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
            Debug.Log("anda vai");
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("anda caralho");
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("Moving", false);


                    VerifyMousePosition();
                    StopFootStepSound();
                }
            }
            else
            {
                animator.SetBool("Moving", true);
                transform.position = agent.nextPosition;
                
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
        if (targetItem != null && !agent.pathPending)
        {
            print(agent.remainingDistance);
            InteractionManagar.instance.isNear = false;
            if (agent.remainingDistance <= interactionDistance + 0.1f)
            {
                InteractionManagar.instance.isNear = true;
                if (targetItem.TryGetComponent(out Door door))
                {
                    door.Use();
                }
                else if (targetItem.TryGetComponent(out Item item))
                {
                    
                }

                targetItem = null;
            }
        }
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        foreach(ParticleSystem p in particles)
        {
            Vector3 currentRotation = p.transform.eulerAngles;
            currentRotation.y += 180f;
            p.transform.eulerAngles = currentRotation;
        }
        

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