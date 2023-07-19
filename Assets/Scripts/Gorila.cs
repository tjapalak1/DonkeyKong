using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorila : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] gorilaSprites;
    private int spriteIndex = 0;

    private bool disabled = false;

    private Sprite baseSprite;

    private void AnimateSprite()
    {
        if (disabled == false)
        {
            spriteIndex++;

            if (spriteIndex >= gorilaSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = gorilaSprites[spriteIndex];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSprite = spriteRenderer.sprite;
        StartCoroutine(AnimateSpriteCoroutine());
        startPosition = transform.position;
    }
    IEnumerator AnimateSpriteCoroutine()
    {
        if (disabled == false)
        {
            yield return new WaitForSeconds(3);
            if (disabled == false)
            {
                InvokeRepeating(nameof(AnimateSprite), 0f, 0.45f);
            }

            yield return new WaitForSeconds(3);
            CancelInvoke();
            spriteRenderer.sprite = baseSprite;
            if (disabled == false)
            {
                StartCoroutine(AnimateSpriteCoroutine());
            }
        }
    }

    public void DisableGorila()
    {
        disabled = true;
    }
    private float timeToMove=1.5f;
    private float timeTracker = 0;
    private Vector3 startPosition;
    public Vector3 endPosition;

    public void StartDeathSequence()
    {
        StartCoroutine(GorilaDeathSequence());
    }
    IEnumerator GorilaDeathSequence()
    {
        yield return new WaitForSeconds(2);
        transform.localScale = new Vector3(1,-1,1);
        
        while(timeTracker<timeToMove)
        {
            transform.position=Vector3.Lerp(startPosition,endPosition,timeTracker/timeToMove);
            timeTracker = timeTracker + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
