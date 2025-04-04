using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableStone : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalTransform;
    private bool isDragging = false;
    private Transform pieceTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Slot"))
            {
                Transform child = eventData.pointerEnter.transform.Find("Piece");
                FlipStone(child.gameObject);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Ensure the clicked object is a Slot
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Slot"))
        {
            // Find the child Piece within the Slot
            pieceTransform = eventData.pointerEnter.transform.Find("Piece");
            if (pieceTransform != null)
            {
                // Set up the piece for dragging
                originalPosition = pieceTransform.position;
                originalTransform = pieceTransform;
                isDragging = true;
                pieceTransform.SetParent(originalTransform); // Move to top level to avoid being masked by other UI elements
                canvasGroup.alpha = 0.6f;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            originalTransform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Slot"))
            {
                Transform targetSlot = eventData.pointerEnter.transform;
                Transform existingPiece = targetSlot.childCount > 0 ? targetSlot.GetChild(0) : null;

                if (existingPiece != null)
                {
                    // Swap the current piece with the existing piece
                    SwapPieces(existingPiece);
                }
                else
                {
                    pieceTransform.position = eventData.pointerEnter.transform.position;
                    pieceTransform.SetParent(eventData.pointerEnter.transform);
                }
            }
            else
            {
                pieceTransform.position = originalPosition;
                pieceTransform.SetParent(originalTransform);
            }

            isDragging = false; // Reset the dragging state
        }
    }

    private void FlipStone(GameObject piece)
    {
        piece.GetComponent<StoneRotation>().FlipStone();
        GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().CheckSolution();
    }

    private void SwapPieces(Transform otherPiece)
    {
        // Store the original parent and position of the other piece
        Transform otherParent = otherPiece.parent;
        Vector3 otherPosition = otherPiece.position;

        // Move the current piece to the other piece's slot
        pieceTransform.SetParent(otherParent);
        pieceTransform.position = otherPosition;

        // Move the other piece to the original slot of the current piece
        otherPiece.SetParent(transform);
        otherPiece.position = originalPosition;
    }

    
}
