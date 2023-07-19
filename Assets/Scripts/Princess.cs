using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite[] princessSprites;
    private int spriteIndex = 0;

    private bool disabled=false;

    private Sprite baseSprite;

    private void AnimateSprite()
    {
        if (disabled == false)
        {
            spriteIndex++;

            if (spriteIndex >= princessSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = princessSprites[spriteIndex];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSprite = spriteRenderer.sprite;
        StartCoroutine(AnimateSpriteCoroutine());
    }
    IEnumerator AnimateSpriteCoroutine()
    {
        if(disabled == false)
        {
            yield return new WaitForSeconds(4);
            if(disabled==false)
            {
                InvokeRepeating(nameof(AnimateSprite), 0f, 0.5f);
            }

            yield return new WaitForSeconds(4);
            CancelInvoke();
            spriteRenderer.sprite = baseSprite;
            if(disabled==false)
            {
                StartCoroutine(AnimateSpriteCoroutine());
            }
        }
    }

    public void DisablePrincess()
    {
        disabled = true;
    }
}
