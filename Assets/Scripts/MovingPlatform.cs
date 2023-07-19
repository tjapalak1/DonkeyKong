using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float platformSpeed = 1;
    public float TopYValue = 10;

    public Vector3 startPosition;
    private bool disabled;

    private GameManager gameManagerRef;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        gameManagerRef = FindObjectOfType<GameManager>();
        StartCoroutine(MovePlatform());
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MovePlatform()
    {
        while(gameManagerRef.levelStarted!=true)
        {
            yield return new WaitForEndOfFrame();
        }
        while(transform.position.y<TopYValue)
        {
            if(disabled==true)
            {
                yield break;
            }
            transform.position = new Vector3(transform.position.x,transform.position.y+platformSpeed*Time.deltaTime , transform.position.z);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    public void DisableMovingPlatform()
    {
        disabled = true;
    }

}
