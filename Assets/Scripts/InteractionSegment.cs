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
        if (canvasAudio == null)
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            if (canvasObject != null)
            {
                canvasAudio = canvasObject.GetComponent<AudioSource>();
            }
        }
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

            var manager = InteractionManagar.instance;

            bool canPlaySound =
                canvasAudio != null &&
                hoverSound != null &&
                !sound &&
                manager != null &&
                manager.highlightedItem != null;

            if (canPlaySound)
            {
                // TEMPORARILY REMOVE HasFlag TO TEST SOUND
                // If this plays now, the issue is HasFlag(type)
                Debug.Log("Attempting to play hover sound...");

                if (manager.highlightedItem.interactions.HasFlag(type))
                {
                    Debug.Log("Hover sound conditions met. Playing sound.");
                    canvasAudio.PlayOneShot(hoverSound);
                    sound = true;
                }
                else
                {
                    Debug.Log($"interaction does not have flag {type}");
                }
            }
        }
        else
        {
            // Reset sound so it can be played again next time
            if (sound)
            {
                Debug.Log("Exiting highlight - sound reset.");
            }
            sound = false;
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

        if (canvasAudio != null && clickSound != null)
        {
            canvasAudio.Stop();
            canvasAudio.PlayOneShot(clickSound);
        }

        sound = false;
    }
}
