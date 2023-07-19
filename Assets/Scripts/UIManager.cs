using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text CurrentScoreText;
    public Text BonusScoreText;
    public Text TopScoreText;
    public Text LivesText;
    public Text LevelText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCurrentScoreText(int currentScore)
    {
        CurrentScoreText.text = "I "+currentScore.ToString("D6");
    }
    public void UpdateBonusScoreText(int bonusScore)
    {
        BonusScoreText.text = bonusScore.ToString("D4");
    }
    public void UpdateTopScoreText(int topScore)
    {
        TopScoreText.text = "TOP " + topScore.ToString("D6");
    }
    public void UpdateLivesText(int lives)
    {
        LivesText.text = "m\n" + lives.ToString();
    }
    public void UpdateLevelText(int level)
    {
        LevelText.text = "l\n" + level.ToString();
    }
}
