using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class SlidingPuzzle : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private SceneAsset returnToScene;

    private List<Transform> pieces;
    private int emptyLocation;
    private int size;
    private bool shuffling = false;
    private bool first = true;
    private float moveDuration = 0.3f;

    private void Start()
    {
        pieces = new List<Transform>();
        size = 3;
        CreateGamePieces(0.01f);
    }

    private void Update()
    {
        if (!shuffling && CheckCompletion())
        {
            shuffling = true;
            if (first)
            {
                StartCoroutine(WaitShuffle(0.5f));
                first = false;
            }
            else
            {
                StartCoroutine(SceneChanger.instance.changeScene(returnToScene));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (SwapIfValid(i, -size, size)) break;
                        if (SwapIfValid(i, +size, size)) break;
                        if (SwapIfValid(i, -1, 0)) break;
                        if (SwapIfValid(i, +1, size - 1)) break;
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(WaitShuffle(0.5f));
        }
    }

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";
                piece.GetComponentInChildren<TMP_Text>().text = piece.name;
                

                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4]
                    {
                        new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap)),
                        new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap)),
                        new Vector2((width * col) + gap, 1 - ((width * row) + gap)),
                        new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap))
                    };
                    mesh.uv = uv;
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if ((i % size) != colCheck && (i + offset) == emptyLocation)
        {
            StartCoroutine(MovePiece(pieces[i], pieces[i + offset].localPosition, i, offset));
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) = (pieces[i + offset].localPosition, pieces[i].localPosition);
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}") return false;
        }
        return true;
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size))
        {
            int rnd = Random.Range(0, size * size);
            if (rnd == last) continue;
            last = emptyLocation;
            if (SwapIfValid(rnd, -size, size) || SwapIfValid(rnd, +size, size) || SwapIfValid(rnd, -1, 0) || SwapIfValid(rnd, +1, size - 1))
            {
                count++;
            }
        }
    }

    private IEnumerator MovePiece(Transform piece, Vector3 targetPosition, int i, int offset)
    {
        Vector3 startPosition = piece.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            piece.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        piece.localPosition = targetPosition;
    }
}
