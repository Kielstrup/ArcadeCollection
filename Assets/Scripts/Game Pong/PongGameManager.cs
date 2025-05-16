using UnityEngine;
using TMPro;

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

    [System.Obsolete]
    public void ScoreLeft()
    {
        leftPlayerScore++;
        UpdateScoreUI();
        CheckVictory();
        StartCoroutine(ResetBall());
    }

    [System.Obsolete]
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

    [System.Obsolete]
    System.Collections.IEnumerator ResetBall()
    {
        ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(resetDelay);
        ball.transform.position = ballStartPosition.position;
        ball.gameObject.SetActive(true);
        ball.LaunchBall();
    }

    private void Start()
    {
        UpdateScoreUI();
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
        // Disable ball and paddles, or show a victory UI panel, etc.
        ball.gameObject.SetActive(false);
    

        // Show victory message (you can create a UI Text or popup for this)
        Debug.Log(winnerMessage);

        // You could also add logic here to restart or return to menu
    }

    
}
