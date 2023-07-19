using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float fireSpeed = 1;
    public float fireMoveDistance = 1;
    public float fireStayTime = 1;
    public float distanceToMoveLocationAcceptanceRadius = 1;
    private float distanceToMoveLocation;

    private GameManager gameManagerRef;
    private Player playerRef;

    private SpriteRenderer spriteRenderer;
    public Sprite[] fireSprtes;
    private int spriteIndex = 0;

    private bool disabled=false;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = FindObjectOfType<Player>();
        gameManagerRef = FindObjectOfType<GameManager>();
        StartCoroutine(moveToLocation(getNewPoint()));

        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f);


    }
    
    private void AnimateSprite()
    {
        if(disabled==false)
        {
            spriteIndex++;

            if (spriteIndex >= fireSprtes.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = fireSprtes[spriteIndex];
        }  
    }
    

    private IEnumerator moveToLocation(Vector3 moveLocation)
    {
        yield return new WaitForSeconds(fireStayTime);
        if(gameManagerRef.levelStarted==true)
        {
            distanceToMoveLocation = (transform.position - moveLocation).magnitude;
            Vector3 movementDirection = (moveLocation - transform.position).normalized;
            while (distanceToMoveLocation > distanceToMoveLocationAcceptanceRadius)
            {
                if (disabled == true)
                {
                    break;
                }
                transform.position = transform.position + movementDirection * fireSpeed * Time.deltaTime;

                distanceToMoveLocation = (transform.position - moveLocation).magnitude;
                yield return new WaitForEndOfFrame();
            }
            if (disabled == true)
            {
                yield return null;
            }
        }
        

        StartCoroutine(moveToLocation(getNewPoint()));

        yield return null;
    }
    
    private Vector3 getNewPoint()
    {

        Vector3 point = transform.position + ((Vector3)(Random.insideUnitCircle.normalized) + (playerRef.transform.position - transform.position).normalized).normalized * fireMoveDistance;
        
        return point;
    }

    public void DisableFire()
    {
        disabled = true;
    }
}
