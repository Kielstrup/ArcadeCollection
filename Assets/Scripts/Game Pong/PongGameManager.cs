using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages the core game flow and state for the Pong game.
/// Tracks player scores, handles ball resets, victory conditions, UI updates, and game start/end logic.
/// </summary>
public class PongGameManager : MonoBehaviour
{
    /// <summary>Score for the left player.</summary>
    public int leftPlayerScore = 0;

    /// <summary>Score for the right player.</summary>
    public int rightPlayerScore = 0;

    /// <summary>UI text for left player score display.</summary>
    public TextMeshProUGUI leftScoreText;

    /// <summary>UI text for right player score display.</summary>
    public TextMeshProUGUI rightScoreText;

    /// <summary>Reference to the ball controller script, assigned in inspector.</summary>
    public BallController ball;

    /// <summary>Transform marking the start position of the ball.</summary>
    public Transform ballStartPosition;

    /// <summary>Delay in seconds before the ball resets after a point is scored.</summary>
    public float resetDelay = 1.5f;

    /// <summary>Score required to win the game.</summary>
    public int winningScore = 5;

    /// <summary>UI panel shown before the game starts.</summary>
    public GameObject startPanel;

    /// <summary>UI panel shown when the game ends.</summary>
    public GameObject endPanel;

    /// <summary>Text component to display winner message.</summary>
    public TextMeshProUGUI endGameText;

    /// <summary>Left player paddle GameObject.</summary>
    public GameObject leftPaddle;

    /// <summary>Right player paddle GameObject.</summary>
    public GameObject rightPaddle;

    /// <summary>Tracks whether the game has started.</summary>
    public bool isGameStarted = false;

    /// <summary>
    /// Called when the right player scores a point.
    /// Updates score, checks for victory, and resets the ball.
    /// </summary>
    public void ScoreLeft()
    {
        leftPlayerScore++;
        UpdateScoreUI();
        CheckVictory();
        StartCoroutine(ResetBall());
    }

    /// <summary>
    /// Called when the left player scores a point.
    /// Updates score, checks for victory, and resets the ball.
    /// </summary>
    public void ScoreRight()
    {
        rightPlayerScore++;
        UpdateScoreUI();
        CheckVictory();
        StartCoroutine(ResetBall());
    }

    /// <summary>
    /// Updates the on-screen score UI for both players.
    /// </summary>
    void UpdateScoreUI()
    {
        leftScoreText.text = leftPlayerScore.ToString();
        rightScoreText.text = rightPlayerScore.ToString();
    }

    /// <summary>
    /// Coroutine to reset the ball after a delay.
    /// Temporarily disables the ball, repositions it, then launches it.
    /// </summary>
    IEnumerator ResetBall()
    {
        ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(resetDelay);
        ball.transform.position = ballStartPosition.position;
        ball.gameObject.SetActive(true);
        ball.LaunchBall();
    }

    /// <summary>
    /// Initializes the game state and UI at startup.
    /// Disables gameplay elements until the game starts.
    /// </summary>
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

    /// <summary>
    /// Checks if a player has reached the winning score.
    /// If so, ends the game and displays the winner.
    /// </summary>
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

    /// <summary>
    /// Handles ending the game by disabling gameplay and showing the end screen.
    /// </summary>
    /// <param name="winnerMessage">Message indicating the winning player.</param>
    private void EndGame(string winnerMessage)
    {
        isGameStarted = false;

        ball.gameObject.SetActive(false);
        leftPaddle.SetActive(false);
        rightPaddle.SetActive(false);

        endGameText.text = winnerMessage;
        endPanel.SetActive(true);
    }

    /// <summary>
    /// Called by UI button to start the game.
    /// Enables gameplay elements and launches the ball after a delay.
    /// </summary>
    public void OnStartButtonPressed()
    {
        isGameStarted = true;
        startPanel.SetActive(false);
        leftPaddle.SetActive(true);
        rightPaddle.SetActive(true);
        ball.gameObject.SetActive(true);
        StartCoroutine(DelayedBallLaunch());
    }

    /// <summary>
    /// Coroutine that delays ball launch after game start or restart.
    /// </summary>
    private IEnumerator DelayedBallLaunch()
    {
        ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(resetDelay);
        ball.gameObject.SetActive(true);
        ball.LaunchBall();
    }

    /// <summary>
    /// Called by UI button to restart the game.
    /// Resets scores, enables gameplay, and launches the ball.
    /// </summary>
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

    /// <summary>
    /// Called by UI button to return to the main menu scene.
    /// </summary>
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
