using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText, bestText;

    
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    private int score, bestScore;
    

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score : " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null )  // Null checking the game manager reference
        {
            Debug.LogError("Game Manager is NULL.");
        }

       /* bestScore = PlayerPrefs.GetInt("HighScore", 0);
        bestText.text = "Best : " + bestScore;*/
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score : " + playerScore;
    }

   /* public void CheckForBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            bestText.text = "Best : " + bestScore;  
        }
    } */

    public void UpdateLives(int currentLives) 
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0) 
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverlickerRoutine());
    }

    IEnumerator GameOverlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResumePlay()
    {
        GameManager gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gm.ResumeGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
