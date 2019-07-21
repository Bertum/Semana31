using System.Collections.Generic;
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
    private List<Vector3> pinInitialPositions;
    private List<Quaternion> pinInitialRotations;
    private bool playerHasToPlayAgain;
    private int currentTurn;
    private int lastGames;
    private float timeToCheck = 10;
    private float checkPinTimer;
    [HideInInspector]
    public bool checkPins;

    private void Awake()
    {
        tippedPins = new List<GameObject>();
        bowlPins = new List<GameObject>();
        pinInitialPositions = new List<Vector3>();
        pinInitialRotations = new List<Quaternion>();
        bowlPins.AddRange(GameObject.FindGameObjectsWithTag("Pin"));
        ballController = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        txtScorePlayerOne = GameObject.Find("GameUI/PanelPlayerOne/Score").GetComponent<Text>();
        txtScorePlayerTwo = GameObject.Find("GameUI/PanelPlayerTwo/Score").GetComponent<Text>();
        foreach (var pin in bowlPins)
        {
            pinInitialPositions.Add(pin.transform.position);
            pinInitialRotations.Add(pin.transform.rotation);
        }
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
        ResetPins();
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
        ResetPins();
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
        foreach (var pin in bowlPins)
        {
            if (Mathf.Abs(pin.transform.rotation.x) > 0.1f && !tippedPins.Contains(pin))
            {
                tippedPins.Add(pin);
                pin.SetActive(false);
            }
        }
        if (tippedPins.Count == bowlPins.Count && !playerHasToPlayAgain)
        {
            SetPoints(15);
            if (currentTurn == 10 && lastGames < 2)
            {
                ResetPins();
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
        ballController.ResetBall();
    }

    private void ResetPins()
    {
        tippedPins.Clear();
        for (int i = 0; i < bowlPins.Count; i++)
        {
            bowlPins[i].SetActive(true);
            bowlPins[i].transform.position = pinInitialPositions[i];
            bowlPins[i].transform.rotation = pinInitialRotations[i];
        }
    }
    #endregion
}
