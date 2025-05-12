using UnityEngine;
using UnityEngine.UI;

public class InteractionSegment : MonoBehaviour
{
    public float minAngle;
    public float maxAngle;
    public InteractionType type;
    public bool IsHighlighted { get; private set; }

    private Image segmentImage;
    public Sprite normalSprite;
    public Sprite highlightSprite;

    private Vector3 scale;

    public AudioSource canvasAudio;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private bool sound;

    private void Start()
    {
        canvasAudio = GameObject.Find("Canvas").GetComponent<AudioSource>();
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

        segmentImage.sprite = inside ? highlightSprite : normalSprite;
        IsHighlighted = inside;

        if (inside)
        {
            transform.localScale = scale * 1.2f;
            if (canvasAudio != null && hoverSound != null && !sound 
                && InteractionManagar.instance.highlightedItem.interactions.HasFlag(type))
            {
                canvasAudio.PlayOneShot(hoverSound);
                sound = true;
            }
        }
        else
        {
            if (sound)
            {
                sound = false;
            }
            transform.localScale = scale;
        }
    }

    public void Unhighlight()
    {
        IsHighlighted = false;
        sound = false;
        if (segmentImage != null)
        {
            segmentImage.sprite = normalSprite;
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
        canvasAudio.Stop();
        sound = false;
        canvasAudio.PlayOneShot(clickSound);
    }
}