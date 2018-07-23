using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    public string[,] chessBoard = null;

    void Start()
    {
        InitChessboard();
    }

    public void InitChessboard()
    {
        chessBoard = new string[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                chessBoard[x, y] = "";
            }
        }
    }

    public bool Occupied(Player.Types playerType, int x, int y)
    {
        Debug.Log(string.Format("player={0} going to occupied x={1}, y={2}", playerType.ToString(), x, y));

        if (playerType == Player.Types.None)
        {
            Debug.LogError("Invalid player type");
            return false;
        }

        if (string.IsNullOrEmpty(chessBoard[x, y]))
        {
            chessBoard[x, y] = playerType.ToString();
            return true;
        }
        else
        {
            Debug.LogError("here is not empty choose again.");
            return false;
        }
    }

    public Player.Types CheckWinner()
    {
        Debug.Log("00, 10, 20");
        if (CheckIsLine(chessBoard[0, 0], chessBoard[1, 0], chessBoard[2, 0]))
        {
            return CheckWho(chessBoard[0, 0]);
        }

        Debug.Log("01, 11, 21");
        if (CheckIsLine(chessBoard[0, 1], chessBoard[1, 1], chessBoard[2, 1]))
        {
            return CheckWho(chessBoard[0, 1]);
        }

        Debug.Log("02, 12, 22");
        if (CheckIsLine(chessBoard[0, 2], chessBoard[1, 2], chessBoard[2, 2]))
        {
            return CheckWho(chessBoard[0, 2]);
        }

        Debug.Log("00, 01, 02");
        if (CheckIsLine(chessBoard[0, 0], chessBoard[0, 1], chessBoard[0, 2]))
        {
            return CheckWho(chessBoard[0, 0]);
        }

        Debug.Log("10, 11, 12");
        if (CheckIsLine(chessBoard[1, 0], chessBoard[1, 1], chessBoard[1, 2]))
        {
            return CheckWho(chessBoard[1, 0]);
        }

        Debug.Log("20, 21, 22");
        if (CheckIsLine(chessBoard[2, 0], chessBoard[2, 1], chessBoard[2, 2]))
        {
            return CheckWho(chessBoard[2, 0]);
        }

        Debug.Log("00, 11, 22");
        if (CheckIsLine(chessBoard[0, 0], chessBoard[1, 1], chessBoard[2, 2]))
        {
            return CheckWho(chessBoard[0, 0]);
        }

        Debug.Log("20, 11, 02");
        if (CheckIsLine(chessBoard[2, 0], chessBoard[1, 1], chessBoard[0, 2]))
        {
            return CheckWho(chessBoard[2, 0]);
        }

        return Player.Types.None;
    }

    bool CheckIsLine(string s1, string s2, string s3)
    {
        Debug.Log(string.Format("[CheckIsLine] s1={0} s2={1} s3={2}", s1, s2, s3));
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2) || string.IsNullOrEmpty(s3))
        {
            return false;
        }

        if (s1 == s2 && s1 == s3)
        {
            return true;
        }

        return false;
    }

    Player.Types CheckWho(string s)
    {
        if (s == Player.Types.O.ToString())
            return Player.Types.O;
        else if (s == Player.Types.X.ToString())
            return Player.Types.X;
        else
            return Player.Types.None;
    }

    public bool CheckChessBoardIsFull()
    {
        bool result = true;
        for (int x = 0; x < chessBoard.GetLength(0); x++)
        {
            for (int y = 0; y < chessBoard.GetLength(1); y++)
            {
                if (string.IsNullOrEmpty(chessBoard[x, y]))
                {
                    result = false;
                    return result;
                }
            }
        }

        return result;
    }
}
