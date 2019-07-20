using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool firstPlayerTurn;
    private float scorePlayerOne, scorePlayerTwo;
    private BallController ballController;
    private Text txtScorePlayerOne, txtScorePlayerTwo;
    private List<GameObject> bowlPins;
    private List<GameObject> tippedPins;
    private bool playerHasToPlayAgain;
    private int currentTurn;
    private int lastGames;
    private float timeToCheck = 5;
    private float checkPinTimer;
    [HideInInspector]
    public bool checkPins;

    private void Awake()
    {
        tippedPins = new List<GameObject>();
        bowlPins = new List<GameObject>();
        bowlPins.AddRange(GameObject.FindGameObjectsWithTag("Pin"));
        ballController = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        txtScorePlayerOne = GameObject.Find("GameUI/PanelPlayerOne/Score").GetComponent<Text>();
        txtScorePlayerTwo = GameObject.Find("GameUI/PanelPlayerTwo/Score").GetComponent<Text>();
    }

    void Start()
    {
        checkPins = false;
        checkPinTimer = 0;
        lastGames = 0;
        currentTurn = 1;
        scorePlayerOne = 0;
        scorePlayerTwo = 0;
        firstPlayerTurn = true;
        playerHasToPlayAgain = false;
    }

    private void Update()
    {
        if (checkPins && checkPinTimer < timeToCheck)
        {
            checkPinTimer += Time.deltaTime;
            if (checkPinTimer >= timeToCheck)
            {
                checkPins = false;
                checkPinTimer = 0;
                CheckPins();
            }
        }
    }

    #region Public Functions

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
        ballController.ResetBall();
    }



    #endregion

    #region Private Functions
    private void UpdateUI()
    {
        txtScorePlayerOne.text = scorePlayerOne.ToString();
        txtScorePlayerTwo.text = scorePlayerTwo.ToString();
    }

    private void ChangeTurn()
    {
        firstPlayerTurn = !firstPlayerTurn;
        ballController.canMove = true;
        playerHasToPlayAgain = false;
    }

    private void CheckPins()
    {
        //Pleno 15
        //SemiPleno 13
        //Cada bolo 1 punto
        //Ronda 10 si haces pleno tienes 2 adicionales
        //Check if x or z rotation is less than 60 and is not in the list
        tippedPins.AddRange(bowlPins.Where(w => (w.transform.rotation.x < 60 || w.transform.rotation.z < 60) && !tippedPins.Contains(w)).ToList());
        if (tippedPins.Count == bowlPins.Count && !playerHasToPlayAgain)
        {
            SetPoints(15);
            if (currentTurn == 10 && lastGames < 2)
            {
                lastGames++;
            }
            else
            {
                lastGames = 0;
                ChangeTurn();
            }
        }
        else
        {
            if (!playerHasToPlayAgain)
            {
                playerHasToPlayAgain = true;
            }
            else
            {
                if (tippedPins.Count == bowlPins.Count)
                {
                    SetPoints(13);
                }
                else
                {
                    SetPoints(tippedPins.Count);
                }
            }
        }
    }
    #endregion
}
