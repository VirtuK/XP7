using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIDraw : MonoBehaviour
{
    public Color drawColor = Color.black;
    public float brushSize = 5.0f;
    private const float eraserSize = 5.0f; // Fixed size for the eraser

    private Texture2D drawTexture;
    private RawImage rawImage;

    public GameObject puzzle;
    public GameObject player;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        if (rawImage == null)
        {
            Debug.LogError("RawImage component not found.");
            return;
        }

        // Create a new texture for this RawImage
        int width = Mathf.RoundToInt(rawImage.rectTransform.rect.width);
        int height = Mathf.RoundToInt(rawImage.rectTransform.rect.height);
        drawTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        drawTexture.filterMode = FilterMode.Point;

        // Initialize the texture with a transparent background
        ClearTexture();

        // Assign the unique texture to the RawImage
        rawImage.texture = drawTexture;

        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button for drawing
        {
            DrawAtMousePosition(drawColor, brushSize);
        }
        else if (Input.GetMouseButton(1)) // Right mouse button for erasing
        {
            DrawAtMousePosition(Color.clear, eraserSize);
        }
    }

    void DrawAtMousePosition(Color color, float size)
    {
        // Check if mouse is over the RawImage
        if (!RectTransformUtility.RectangleContainsScreenPoint(rawImage.rectTransform, Input.mousePosition, null))
            return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localPoint);

        // Convert local point to texture coordinates
        int x = Mathf.RoundToInt(localPoint.x + rawImage.rectTransform.rect.width * 0.5f);
        int y = Mathf.RoundToInt(localPoint.y + rawImage.rectTransform.rect.height * 0.5f);

        DrawCircle(x, y, size, color);
    }
    void DrawCircle(int centerX, int centerY, float radius, Color color)
    {
        int x0 = Mathf.RoundToInt(centerX - radius);
        int x1 = Mathf.RoundToInt(centerX + radius);
        int y0 = Mathf.RoundToInt(centerY - radius);
        int y1 = Mathf.RoundToInt(centerY + radius);

        for (int y = y0; y <= y1; y++)
        {
            for (int x = x0; x <= x1; x++)
            {
                float dx = x - centerX;
                float dy = y - centerY;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);

                if (distance <= radius)
                {
                    float alpha = 1.0f;
                    if (distance > radius - 1.0f)
                    {
                       // alpha = radius - distance;
                    }

                    Color existingColor = drawTexture.GetPixel(x, y);
                    Color blendedColor = Color.Lerp(existingColor, color, alpha);
                    drawTexture.SetPixel(x, y, blendedColor);
                }
            }
        }
        drawTexture.Apply();
    }

    public void ClearTexture()
    {
        Color[] colors = new Color[drawTexture.width * drawTexture.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.clear;
        }
        drawTexture.SetPixels(colors);
        drawTexture.Apply();
    }

    public void closePuzzle()
    {
        Destroy(puzzle);
        player.GetComponent<ClickToMove>().doingPuzzle = false;
        CursorGame.instance.ResetDrawCursor();
        InteractionManagar.instance.selectedItem = null;
    }
}
