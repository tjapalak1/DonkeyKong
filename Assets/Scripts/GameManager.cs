using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level=1;
    private int lives=1;
    private int currentScore=0;
    public int bonusScoreBaseValue = 5000;
    private int bonusScoreCurrentValue=0;
    public int bonusScoreDecrementValue = 100;
    public float timeToDecrementBonusScore = 2.0f;
    private float nextActionTime;
    public float timeSinceGameStarted = 0;
    public bool levelStarted = false;

    public GameObject canvasReference;
    public UIManager uiManagerReference;

    public bool deletePlayerPrefs=false;

    public int numberOfConnectors = 8;

    public GameObject gameOverCanvasReference;
    public GameObject winCanvasReference;
    public Text winScoreText;

    private int startLevelScore=0;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            lives = 3;
            level = 1; 
            currentScore = 0;
            uiManagerReference.UpdateCurrentScoreText(0);

            winCanvasReference.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {

        }
        if (scene.buildIndex != 0)
        {
            Invoke(nameof(StartLevel), 3f);
            startLevelScore = currentScore;
            uiManagerReference.UpdateBonusScoreText(bonusScoreBaseValue);
            uiManagerReference.UpdateLivesText(lives);
            uiManagerReference.UpdateLevelText(level);
            uiManagerReference.UpdateCurrentScoreText(currentScore);
            uiManagerReference.UpdateTopScoreText(PlayerPrefs.GetInt("TopScore"));
        }



    }

    private void Awake()
    {
        int numberOfGameManagers = FindObjectsOfType<GameManager>().Length;
        if (numberOfGameManagers != 1)
        {
            Destroy(this.gameObject);
            Destroy(canvasReference);
        }
    }

    private void Start()
    {
        if(deletePlayerPrefs==true)
        {
            PlayerPrefs.DeleteAll();
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvasReference);
    }

    private void StartLevel()
    {
        levelStarted = true;
        bonusScoreCurrentValue = bonusScoreBaseValue;
        timeSinceGameStarted = 0;
        nextActionTime = timeToDecrementBonusScore;
        if(uiManagerReference!=null)
        {
            uiManagerReference.UpdateBonusScoreText(bonusScoreBaseValue);
            uiManagerReference.UpdateLivesText(lives);
            uiManagerReference.UpdateLevelText(level);
        }
        foreach(MovingPlatformSpawner mps in FindObjectsOfType<MovingPlatformSpawner>())
        {
            mps.ActivateSpawner();
        }
    }

    public void LevelComplete()
    {
        /*levelStarted = false;

        currentScore += bonusScoreCurrentValue;
        if(currentScore> PlayerPrefs.GetInt("TopScore"))
        {
            PlayerPrefs.SetInt("TopScore",currentScore);
            uiManagerReference.UpdateTopScoreText(currentScore);
        }
        PlayerPrefs.SetInt("CurrentScore", currentScore);

        uiManagerReference.UpdateCurrentScoreText(currentScore);
        */
        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            Debug.Log("You Win");
            LoadLevel(0);
            levelStarted = false;
        }
    }
    public void SaveScore()
    {
        levelStarted = false;

        currentScore += bonusScoreCurrentValue;
        if (currentScore > PlayerPrefs.GetInt("TopScore"))
        {
            PlayerPrefs.SetInt("TopScore", currentScore);
            uiManagerReference.UpdateTopScoreText(currentScore);
        }
        PlayerPrefs.SetInt("CurrentScore", currentScore);

        uiManagerReference.UpdateCurrentScoreText(currentScore);
    }
    
    private void Update()
    {
        if(levelStarted==true)
        {
            timeSinceGameStarted+=Time.deltaTime; 

            if(timeSinceGameStarted>nextActionTime)
            {
                nextActionTime += timeToDecrementBonusScore;
                bonusScoreCurrentValue = Mathf.Clamp(bonusScoreCurrentValue - bonusScoreDecrementValue,0,bonusScoreBaseValue);
                
                uiManagerReference.UpdateBonusScoreText(bonusScoreCurrentValue);
            }
        }
    }
    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;

        if (camera != null) {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    

    public void LevelFailed()
    {
        currentScore = startLevelScore;
        ResetGameManager();
        lives--;
        Debug.Log(lives);
        levelStarted = false;
        if (lives <= 0) {
            Debug.Log("Game over");
            StartCoroutine(GameOverSequence());
            levelStarted = false;
        } else {
            LoadLevel(level);
        }
    }

    public void DisableEverything()
    {
        foreach(Barrel b in FindObjectsOfType<Barrel>())
        {
            b.DisableBarrel();
        }
        foreach (Fire f in FindObjectsOfType<Fire>())
        {
            f.DisableFire();
        }
        foreach(FireSpawner fs in FindObjectsOfType<FireSpawner>())
        {
            fs.DisableFireSpawner();
        }
        foreach (Spawner s in FindObjectsOfType<Spawner>())
        {
            s.DisableBarrelSpawner();
        }
        foreach (Coin c in FindObjectsOfType<Coin>())
        {
            c.DisableCoin();
        }
        foreach(MovingPlatform m in FindObjectsOfType<MovingPlatform>())
        {
            m.DisableMovingPlatform();
        }
        foreach (MovingPlatformSpawner m in FindObjectsOfType<MovingPlatformSpawner>())
        {
            m.DisalbePlatformSpawner();
        }
        foreach (Princess p in FindObjectsOfType<Princess>())
        {
            p.DisablePrincess();
        }
        foreach (Gorila g in FindObjectsOfType<Gorila>())
        {
            g.DisableGorila();
        }
    }



    public void UpdateScoreUI(int value)
    {
        canvasReference.GetComponent<UIManager>().UpdateCurrentScoreText(value);
    }
    public void UpdateBonusScoreUI(int value)
    {
        canvasReference.GetComponent<UIManager>().UpdateBonusScoreText(value);
    }

    public void AddToScore(int toAdd)
    {
        currentScore += toAdd;
        if (currentScore > PlayerPrefs.GetInt("TopScore"))
        {
            PlayerPrefs.SetInt("TopScore", currentScore);
            uiManagerReference.UpdateTopScoreText(currentScore);
        }
        PlayerPrefs.SetInt("CurrentScore", currentScore);

        uiManagerReference.UpdateCurrentScoreText(currentScore);
    }

    public AudioSource winSound;
    public void RemoveConnector()
    {
        numberOfConnectors--;
        if(numberOfConnectors==0)
        {
            RemoveEverything();
            SaveScore();


        }
    }

    private void RemoveEverything()
    {
        foreach (Fire f in FindObjectsOfType<Fire>())
        {
            Destroy(f.transform.gameObject); 
        }
        foreach (Coin c in FindObjectsOfType<Coin>())
        {
            Destroy(c.transform.gameObject);
        }
        foreach (Princess p in FindObjectsOfType<Princess>())
        {
            Destroy(p.transform.gameObject);
        }
        foreach (Player p in FindObjectsOfType<Player>())
        {
            Destroy(p.transform.gameObject);
        }
        foreach (Gorila g in FindObjectsOfType<Gorila>())
        {
            g.DisableGorila();
            g.StartDeathSequence();
        }
        FindObjectOfType<Level3Manager>().DestroyAllObjects();
        StartCoroutine(StartWinSequence());
    }

    private IEnumerator StartWinSequence()
    {
        yield return new WaitForSeconds(4);
        FindObjectOfType<Level3Manager>().InstantiateAllObjects();
        winSound.time = 5.5f;
        winSound.Play();
        yield return new WaitForSeconds(6);
        ResetGameManager();
        winScoreText.text = "Your score : "+currentScore.ToString();
        winCanvasReference.SetActive(true);
        yield return new WaitForSeconds(4);
        LevelComplete();
        yield return null;
    }
    private void ResetGameManager()
    {
        numberOfConnectors = 8;
    }

    private IEnumerator GameOverSequence()
    {
        gameOverCanvasReference.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverCanvasReference.SetActive(false);

        LoadLevel(0);
        yield return null;
    }


}
