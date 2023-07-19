using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // Destroy(FindObjectOfType<GameManager>().gameObject);
        //Destroy(FindObjectOfType<UIManager>().gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
