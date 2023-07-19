using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] fireSprtes;
    private int spriteIndex = 0;
    public float fireSpawnPeriod = 5;
    public int maxNumberOfFires = 3;
    private int currenNumberOfFires = 0;
    public GameObject firePrefab;

    private bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(AnimateSprite), 1f / 2f, 1f / 2f);
        InvokeRepeating(nameof(SpawnFire), fireSpawnPeriod, fireSpawnPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFire()
    {
        if(disabled==false)
        {
            currenNumberOfFires++;

            Instantiate(firePrefab, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);

            if (currenNumberOfFires >= maxNumberOfFires)
            {
                CancelInvoke(nameof(SpawnFire));
            }
        }
        
    }

    private void AnimateSprite()
    {
        if(disabled == false)
        {
            spriteIndex++;

            if (spriteIndex >= fireSprtes.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = fireSprtes[spriteIndex];
        }
    }

    public void DisableFireSpawner()
    {
        disabled = true;
    }
}
