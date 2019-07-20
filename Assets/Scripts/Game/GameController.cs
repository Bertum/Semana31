using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool firstPlayerTurn;
    private float scorePlayerOne, scorePlayerTwo;
    private BallController ballController;
    private Text txtScorePlayerOne, txtScorePlayerTwo;

    private void Awake()
    {
        ballController = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        txtScorePlayerOne = GameObject.Find("GameUI/PanelPlayerOne/Score").GetComponent<Text>();
        txtScorePlayerTwo = GameObject.Find("GameUI/PanelPlayerTwo/Score").GetComponent<Text>();
    }

    void Start()
    {
        scorePlayerOne = 0;
        scorePlayerTwo = 0;
        firstPlayerTurn = true;
    }

    private void ChangeTurn()
    {
        firstPlayerTurn = !firstPlayerTurn;
        ballController.canMove = true;
    }

    public void SetPoints(float points)
    {
        if (firstPlayerTurn)
        {
            scorePlayerOne += points;
        }
        else
        {
            scorePlayerTwo += points;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        txtScorePlayerOne.text = scorePlayerOne.ToString();
        txtScorePlayerTwo.text = scorePlayerTwo.ToString();
    }
}
