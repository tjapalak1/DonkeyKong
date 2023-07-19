using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSpawner : MonoBehaviour
{
    public float spawnPeriod = 4;
    public GameObject movingPlatformPrefab;
    public float spawnDelay = 1.3f;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    public void ActivateSpawner()
    {
        InvokeRepeating(nameof(SpawnPlatform), spawnDelay, spawnPeriod);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPlatform()
    {
        Instantiate(movingPlatformPrefab,transform.position,Quaternion.identity);
    }

    public void DisalbePlatformSpawner()
    {
        CancelInvoke();
    }

}
