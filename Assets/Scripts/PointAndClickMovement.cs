using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ClickToMove : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal
    private Rigidbody rb;
    private NavMeshAgent agent;

    private bool movement = false;
    private float speed;
    private float step;
    private float positionX;
    private float positionZ;
    private Vector3 lastPosition;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground") && hit.distance < 15 && InteractionManagar.instance.interacting == false) // Certifique-se de marcar o chão com a tag "Ground"
                {
                    agent.SetDestination(hit.point);
                }
                if (hit.collider.gameObject.CompareTag("Item") && hit.distance < 15) // Certifique-se de marcar o chão com a tag "Ground"
                {
                    InteractionManagar.instance.CheckInteractions(hit.collider.gameObject.GetComponent<Item>());

                }
            }
            }
        }

}