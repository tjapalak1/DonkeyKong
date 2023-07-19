using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    public GameObject[] objectsToDestroy;
    public GameObject endPart;

    public void DestroyAllObjects()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }
    public void InstantiateAllObjects()
    {
        endPart.SetActive(true);
    }
}
