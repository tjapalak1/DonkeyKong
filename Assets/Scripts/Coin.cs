using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinScoreValue = 100;
    private Vector3 startLocation;
    private Vector3 endLocation;

    public float moveSpeed = 1;

    private bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
        endLocation = transform.position + new Vector3(0, 0.05f, 0);
    }
    public float lerpValue = 0;
    private float lerpMultiplier = 1;
    // Update is called once per frame
    void Update()
    {
        if(disabled==false)
        {
            lerpValue += Time.deltaTime * lerpMultiplier * moveSpeed;
            if (lerpValue >= 1)
            {
                lerpMultiplier = -1;
            }
            if (lerpValue <= 0)
            {
                lerpMultiplier = 1;
            }
            transform.position = Vector3.Lerp(startLocation, endLocation, lerpValue);
        }
        
    }

    public void DisableCoin()
    {
        disabled = true;
    }
}
