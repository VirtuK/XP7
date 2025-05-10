using UnityEngine;
using UnityEngine.UI;

public class InteractionSegment : MonoBehaviour
{
    public float minAngle;
    public float maxAngle;
    public InteractionType type;
    public bool IsHighlighted { get; private set; }

    private Image segmentImage;
    private Color defaultColor = Color.white;
    private Color highlightColor = Color.yellow;

    private Vector3 scale;

    private void Start()
    {
        scale = transform.localScale;
    }
    public void Initialize()
    {
        segmentImage = GetComponent<Image>();
        Unhighlight();
    }

    public void HighlightIfInAngle(float angle)
    {
        bool inside = false;

        if (minAngle > maxAngle)
        {
            // Wrap-around case (e.g., 330°–30°)
            inside = angle >= minAngle || angle <= maxAngle;
        }
        else
        {
            inside = angle >= minAngle && angle <= maxAngle;
        }

        segmentImage.color = inside ? highlightColor : defaultColor;
        IsHighlighted = inside;

        // Adjust the size when highlighted
        if (inside)
        {
            // Increase size when highlighted
            transform.localScale = scale * 1.2f; // You can adjust the scaling factor (1.2f) as needed
        }
        else
        {
            // Reset size when unhighlighted
            transform.localScale = scale;
        }
    }

    public void Unhighlight()
    {
        IsHighlighted = false;
        if (segmentImage != null)
        {
            segmentImage.color = defaultColor;
        }

        // Reset the size when unhighlighted
        transform.localScale = scale;
    }

    public void Trigger(Item item)
    {
        switch (type)
        {
            case InteractionType.Pick:
                item.Pick();
                break;
            case InteractionType.See:
                item.See();
                break;
            case InteractionType.Use:
                item.Use();
                break;
        }
    }
}