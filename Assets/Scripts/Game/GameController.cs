using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool firstPlayerTurn;
    private float scorePlayerOne, scorePlayerTwo;

    void Start()
    {
        scorePlayerOne = 0;
        scorePlayerTwo = 0;
        firstPlayerTurn = true;
    }

    private void ChangeTurn()
    {
        firstPlayerTurn = !firstPlayerTurn;
    }
}
