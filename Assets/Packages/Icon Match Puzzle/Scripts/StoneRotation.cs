using UnityEngine;
using UnityEngine.UI;

public class StoneRotation : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;
    private Image stoneImage;
    private bool isFront = true;

    private void Awake()
    {
        stoneImage = GetComponent<Image>();
        stoneImage.sprite = frontSprite;
    }
    public void FlipStone()
    {
        isFront = !isFront;
        stoneImage.sprite = isFront ? frontSprite : backSprite;
    }
}