using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    public Text playerText;
    public Button playAgainButton;

    public void SetPlayerText(string s)
    {
        playerText.text = s;
    }
}
