using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PongGameManager : MonoBehaviour
{
    public int leftPlayerScore = 0;
    public int rightPlayerScore = 0;

    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;

    public BallController ball; // Assign in inspector

    public Transform ballStartPosition;

    public float resetDelay = 1.5f;
    public int winningScore = 5;
    public GameObject startPanel;
    public GameObject endPanel;
    public TextMeshProUGUI endGameText;
    public GameObject leftPaddle;
    public GameObject rightPaddle;
    public bool isGameStarted = false;


    public void ScoreLeft()
    {
        leftPlayerScore++;
        UpdateScoreUI();
        CheckVictory();
        StartCoroutine(ResetBall());
    }

    
    public void ScoreRight()
    {
        rightPlayerScore++;
        UpdateScoreUI();
        CheckVictory();
        StartCoroutine(ResetBall());
    }

    void UpdateScoreUI()
    {
        leftScoreText.text = leftPlayerScore.ToString();
        rightScoreText.text = rightPlayerScore.ToString();
    }

    
    IEnumerator ResetBall()
    {
        ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(resetDelay);
        ball.transform.position = ballStartPosition.position;
        ball.gameObject.SetActive(true);
        ball.LaunchBall();
    }

    private void Start()
    {
        isGameStarted = false;
        UpdateScoreUI();
        startPanel.SetActive(true);
        endPanel.SetActive(false);
        leftPaddle.SetActive(false);
        rightPaddle.SetActive(false);
        ball.gameObject.SetActive(false);

    }
    private void CheckVictory()
    {
        if (leftPlayerScore >= winningScore)
        {
            Debug.Log("Left Player Wins!");
            EndGame("Left Player Wins!");
        }
        else if (rightPlayerScore >= winningScore)
        {
            Debug.Log("Right Player Wins!");
            EndGame("Right Player Wins!");
        }
    }

    private void EndGame(string winnerMessage)
    {
        isGameStarted = false;

        ball.gameObject.SetActive(false);
        leftPaddle.SetActive(false);
        rightPaddle.SetActive(false);

        endGameText.text = winnerMessage;
        endPanel.SetActive(true);
    }

    
    public void OnStartButtonPressed()
    {
        isGameStarted = true;
        startPanel.SetActive(false);
        leftPaddle.SetActive(true);
        rightPaddle.SetActive(true);
        ball.gameObject.SetActive(true);
        StartCoroutine(DelayedBallLaunch());
    }

    
    private IEnumerator DelayedBallLaunch()
    {
        ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(resetDelay);
        ball.gameObject.SetActive(true);
        ball.LaunchBall();

    }

    
    public void OnRestartButtonPressed()
    {
        leftPlayerScore = 0;
        rightPlayerScore = 0;
        UpdateScoreUI();
        endPanel.SetActive(false);

        leftPaddle.SetActive(true);
        rightPaddle.SetActive(true);
        ball.gameObject.SetActive(true);

        isGameStarted = true;
        StartCoroutine(DelayedBallLaunch());
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
}
