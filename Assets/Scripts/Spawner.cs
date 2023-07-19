using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;
    private GameManager gmRef;

    private bool disabled=false;
    private void Start()
    {
        gmRef = FindObjectOfType<GameManager>();
        Spawn();
    }

    private void Spawn()
    {
        if(disabled==false)
        {
            if (gmRef.levelStarted == true)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
        }
    }

    public void DisableBarrelSpawner()
    {
        disabled = true;
    }

}
