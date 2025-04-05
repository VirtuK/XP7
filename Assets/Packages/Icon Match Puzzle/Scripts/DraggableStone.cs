using UnityEngine;

public class DraggableStone : MonoBehaviour
{
    public Camera mainCamera;

    private Transform selectedPiece;
    private Transform originalSlot;
    private bool isDragging = false;
    private Vector3 pieceOffset;

    public int side1;
    public int side2;
    public int actualSide;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        actualSide = side1;
    }

    void Update()
    {
        // Right-click: flip the piece on a slot
        if (Input.GetMouseButtonDown(1))
        {
            if (RaycastSlot(out Transform slot))
            {
                Transform piece = slot.Find("Piece");
                if (piece != null)
                {
                    FlipStone(piece.gameObject);
                }
            }
        }

        // Left-click down: begin dragging a piece from a slot
        if (Input.GetMouseButtonDown(0))
        {
            if (RaycastSlot(out Transform slot))
            {
                Transform piece = slot.Find("Piece");
                if (piece != null)
                {
                    selectedPiece = piece;
                    originalSlot = slot;

                    // Move piece to world space so we can drag freely
                    selectedPiece.SetParent(null);
                    pieceOffset = selectedPiece.position - GetMouseWorldPoint();
                    isDragging = true;
                }
            }
        }

        // While dragging: move the piece with the mouse
        if (isDragging && selectedPiece != null)
        {
            Vector3 targetPos = GetMouseWorldPoint() + pieceOffset;
            selectedPiece.position = targetPos;
        }

        // Left-click up: drop the piece
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (RaycastSlot(out Transform targetSlot))
            {
                Transform targetPiece = targetSlot.Find("Piece");

                if (targetSlot == originalSlot)
                {
                    ResetPieceToSlot(selectedPiece, originalSlot);
                }
                else if (targetPiece != null)
                {
                    // Swap the two pieces
                    SwapPieces(originalSlot, selectedPiece, targetSlot, targetPiece);
                }
                else
                {
                    // Drop into empty slot
                    MovePieceToSlot(selectedPiece, targetSlot);
                }
            }
            else
            {
                // Dropped outside → reset
                ResetPieceToSlot(selectedPiece, originalSlot);
            }

            selectedPiece = null;
            originalSlot = null;
            isDragging = false;
        }
    }

    private bool RaycastSlot(out Transform slot)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Slot"))
        {
            slot = hit.collider.transform;
            return true;
        }

        slot = null;
        return false;
    }

    private Vector3 GetMouseWorldPoint()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void FlipStone(GameObject piece)
    {
        piece.GetComponent<StoneRotation>().FlipStone();

        actualSide = (actualSide == side1) ? side2 : side1;

        GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().CheckSolution();
    }

    private void ResetPieceToSlot(Transform piece, Transform slot)
    {
        piece.SetParent(slot);
        piece.localPosition = Vector3.zero; // Properly center the piece in slot
    }

    private void MovePieceToSlot(Transform piece, Transform slot)
    {
        piece.SetParent(slot);
        piece.localPosition = Vector3.zero;
    }

    private void SwapPieces(Transform slotA, Transform pieceA, Transform slotB, Transform pieceB)
    {
        // Store world positions
        Vector3 posA = pieceA.position;
        Vector3 posB = pieceB.position;

        // Swap parents
        pieceA.SetParent(slotB);
        pieceB.SetParent(slotA);

        // Set world positions so they visually swap places
        pieceA.position = posB;
        pieceB.position = posA;
    }
}
