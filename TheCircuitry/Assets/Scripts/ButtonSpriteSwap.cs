using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonSpriteSwap : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;

    private Sprite currentSprite;

    // Use this for initialization
    void Start()
    {
        currentSprite = sprite1;
    }

    public void swapSprites()
    {
        if(sprite1 == currentSprite)
        {
            GetComponent<Image>().overrideSprite = sprite1;
            currentSprite = sprite1;
        }

        else
        {
            GetComponent<Image>().overrideSprite = sprite2;
            currentSprite = sprite2;
        }
    }
}
