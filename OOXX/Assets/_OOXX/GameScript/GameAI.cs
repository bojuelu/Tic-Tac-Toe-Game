using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAI : MonoBehaviour
{
    private int lineIsFull = -100;

    public bool ThinkingAndPlace(Player player, GameRule gameRule, out int placeX, out int placeY)
    {
        string me = player.type.ToString();
        string enemy = "";
        if (me == Player.Types.O.ToString())
            enemy = Player.Types.X.ToString();
        else
            enemy = Player.Types.O.ToString();

        List<int> scoreOfAxisX = ScoreOfAxisX(gameRule.chessBoard, me, enemy);
        List<int> scoreOfAxisY = ScoreOfAxisY(gameRule.chessBoard, me, enemy);
        int scoreOfAxisLBtoRT = ScoreOfAxisLBtoRT(gameRule.chessBoard, me, enemy);
        int scoreOfAxisLTtoRB = ScoreOfAxisLTtoRB(gameRule.chessBoard, me, enemy);

        // check if self is gonna win
        Debug.Log(string.Format("<color=yellow> check if self is gonna win </color>"));
        for (int i = 0; i < scoreOfAxisX.Count; i++)  // axisX
        {
            if (scoreOfAxisX[i] >= 2 && scoreOfAxisX[i] != lineIsFull)
            {
                int y = i;
                int x = FindEmptyPlaceAxisX(y, gameRule.chessBoard);
                placeX = x;
                placeY = y;
                return gameRule.Occupied(player.type, x, y);
            }
        }
        for (int i = 0; i < scoreOfAxisY.Count; i++)  // axisY
        {
            if (scoreOfAxisY[i] >= 2 && scoreOfAxisY[i] != lineIsFull)
            {
                int x = i;
                int y = FindEmptyPlaceAxisY(x, gameRule.chessBoard);
                placeX = x;
                placeY = y;
                return gameRule.Occupied(player.type, x, y);
            }
        }
        // axisLBtoRT
        if (scoreOfAxisLBtoRT >= 2 && scoreOfAxisLBtoRT != lineIsFull)
        {
            int x, y;
            FindEmptyPlaceAxisLBtoRT(gameRule.chessBoard, out x, out y);
            placeX = x;
            placeY = y;
            return gameRule.Occupied(player.type, x, y);
        }
        // axisLTtoRB
        if (scoreOfAxisLTtoRB >= 2 && scoreOfAxisLTtoRB != lineIsFull)
        {
            int x, y;
            FindEmptyPlaceAxisLTtoRB(gameRule.chessBoard, out x, out y);
            placeX = x;
            placeY = y;
            return gameRule.Occupied(player.type, x, y);
        }
        Debug.Log(string.Format("<color=yellow> no chance, try next </color>"));

        // check enemy is gonna win
        Debug.Log(string.Format("<color=yellow> check enemy is gonna win </color>"));
        for (int i = 0; i < scoreOfAxisX.Count; i++)
        {
            if (scoreOfAxisX[i] <= -2 && scoreOfAxisX[i] != lineIsFull)
            {
                int y = i;
                int x = FindEmptyPlaceAxisX(y, gameRule.chessBoard);
                placeX = x;
                placeY = y;
                return gameRule.Occupied(player.type, x, y);
            }
        }
        for (int i = 0; i < scoreOfAxisY.Count; i++)
        {
            if (scoreOfAxisY[i] <= -2 && scoreOfAxisY[i] != lineIsFull)
            {
                int x = i;
                int y = FindEmptyPlaceAxisY(x, gameRule.chessBoard);
                placeX = x;
                placeY = y;
                return gameRule.Occupied(player.type, x, y);
            }
        }
        // axisLBtoRT
        if (scoreOfAxisLBtoRT <= -2 && scoreOfAxisLBtoRT != lineIsFull)
        {
            int x, y;
            FindEmptyPlaceAxisLBtoRT(gameRule.chessBoard, out x, out y);
            placeX = x;
            placeY = y;
            return gameRule.Occupied(player.type, x, y);
        }
        // axisLTtoRB
        if (scoreOfAxisLTtoRB <= -2 && scoreOfAxisLTtoRB != lineIsFull)
        {
            int x, y;
            FindEmptyPlaceAxisLTtoRB(gameRule.chessBoard, out x, out y);
            placeX = x;
            placeY = y;
            return gameRule.Occupied(player.type, x, y);
        }
        Debug.Log(string.Format("<color=yellow> nope, try the best move </color>"));

        // find best place
        Debug.Log(string.Format("<color=yellow> find best place </color>"));
        int highestScoreAxisX = 0;
        int yIndexOfhighestScoreAxisX = 0;
        for (int i = 0; i < scoreOfAxisX.Count; i++)
        {
            int score = scoreOfAxisX[i];
            if (score > highestScoreAxisX)
            {
                highestScoreAxisX = score;
                yIndexOfhighestScoreAxisX = i;
            }
        }
        int highestScoreAxisY = 0;
        int xIndexOfhighestScoreAxisY = 0;
        for (int i = 0; i < scoreOfAxisY.Count; i++)
        {
            int score = scoreOfAxisY[i];
            if (score > highestScoreAxisY)
            {
                highestScoreAxisY = score;
                xIndexOfhighestScoreAxisY = i;
            }
        }
        Debug.Log("highestScoreAxisX=" + highestScoreAxisX + " " + "highestScoreAxisY=" + highestScoreAxisY);
        Debug.Log("yIndexOfhighestScoreAxisX=" + yIndexOfhighestScoreAxisX + " " + "xIndexOfhighestScoreAxisY=" + xIndexOfhighestScoreAxisY);
        if (highestScoreAxisX >= highestScoreAxisY)
        {
            int y = yIndexOfhighestScoreAxisX;
            int x = FindEmptyPlaceAxisX(y, gameRule.chessBoard);
            placeX = x;
            placeY = y;
            return gameRule.Occupied(player.type, x, y);
        }
        else
        {
            int x = xIndexOfhighestScoreAxisY;
            int y = FindEmptyPlaceAxisY(x, gameRule.chessBoard);
            placeX = x;
            placeY = y;
            return gameRule.Occupied(player.type, x, y);
        }
    }

    List<int> ScoreOfAxisX(string[,] cheesBoard, string me, string enemy)
    {
        List<int> scoreList = new List<int>();

        for (int y = 0; y < cheesBoard.GetLength(1); y++)
        {
            int score = 0;
            bool isFull = true;
            for (int x = 0; x < cheesBoard.GetLength(0); x++)
            {
                if (string.IsNullOrEmpty(cheesBoard[x, y]))
                {
                    isFull = false;
                }
            }

            if (isFull)
            {
                score = lineIsFull;
                Debug.Log("[ScoreOfAxisX] " + y + " is FULL. score=" + score);
                scoreList.Add(score);
                continue;
            }
            else
            {
                for (int x = 0; x < cheesBoard.GetLength(0); x++)
                {
                    if (cheesBoard[x, y] == me)
                        score += 1;
                    else if (cheesBoard[x, y] == enemy)
                        score += -1;
                }
                Debug.Log("[ScoreOfAxisX] " + y + " score=" + score);
                scoreList.Add(score);
                continue;
            }
        }

        return scoreList;
    }

    List<int> ScoreOfAxisY(string[,] cheesBoard, string me, string enemy)
    {
        List<int> scoreList = new List<int>();

        for (int x = 0; x < cheesBoard.GetLength(0); x++)
        {
            int score = 0;
            bool isFull = true;
            for (int y = 0; y < cheesBoard.GetLength(1); y++)
            {
                if (string.IsNullOrEmpty(cheesBoard[x, y]))
                {
                    isFull = false;
                }
            }

            if (isFull)
            {
                score = lineIsFull;
                Debug.Log("[ScoreOfAxisY] " + x + " is FULL. score=" + score);
                scoreList.Add(score);
                continue;
            }
            else
            {
                for (int y = 0; y < cheesBoard.GetLength(1); y++)
                {
                    if (cheesBoard[x, y] == me)
                        score += 1;
                    else if (cheesBoard[x, y] == enemy)
                        score += -1;
                }
                Debug.Log("[ScoreOfAxisY] " + x + " score=" + score);
                scoreList.Add(score);
                continue;
            }
        }

        return scoreList;
    }

    int ScoreOfAxisLBtoRT(string[,] cheesBoard, string me, string enemy)
    {
        int score = 0;
        string[] theLine = new string[] { cheesBoard[0, 0], cheesBoard[1, 1], cheesBoard[2, 2] };

        bool isFull = true;
        for (int i = 0; i < theLine.Length; i++)
        {
            if (string.IsNullOrEmpty(theLine[i]))
            {
                isFull = false;
            }
        }

        if (isFull)
        {
            score = lineIsFull;
            Debug.Log("[ScoreOfAxisLBtoRT] is FULL, score=" + score);
            return score;
        }
        else
        {
            for (int i = 0; i < theLine.Length; i++)
            {
                if (theLine[i] == me)
                    score += 1;
                else if (theLine[i] == enemy)
                    score -= 1;
            }
        }

        Debug.Log("[ScoreOfAxisLBtoRT] score=" + score);
        return score;
    }

    int ScoreOfAxisLTtoRB(string[,] cheesBoard, string me, string enemy)
    {
        int score = 0;
        string[] theLine = new string[] { cheesBoard[0, 2], cheesBoard[1, 1], cheesBoard[2, 0] };

        bool isFull = true;
        for (int i = 0; i < theLine.Length; i++)
        {
            if (string.IsNullOrEmpty(theLine[i]))
            {
                isFull = false;
            }
        }

        if (isFull)
        {
            score = lineIsFull;
            Debug.Log("[ScoreOfAxisLTtoRB] is FULL, score=" + score);
            return score;
        }
        else
        {
            for (int i = 0; i < theLine.Length; i++)
            {
                if (theLine[i] == me)
                    score += 1;
                else if (theLine[i] == enemy)
                    score -= 1;
            }
        }

        return score;
    }

    int FindEmptyPlaceAxisX(int y, string[,] chessBoard)
    {
        Debug.Log("<color=white> FindEmptyPlaceAxisX </color>");
        for (int x = 0; x < chessBoard.GetLength(0); x++)
        {
            if (string.IsNullOrEmpty(chessBoard[x, y]))
            {
                return x;
            }
        }
        return -1;
    }

    int FindEmptyPlaceAxisY(int x, string[,] chessBoard)
    {
        Debug.Log("<color=white> FindEmptyPlaceAxisY </color>");
        for (int y = 0; y < chessBoard.GetLength(1); y++)
        {
            if (string.IsNullOrEmpty(chessBoard[x, y]))
            {
                return y;
            }
        }
        return -1;
    }

    void FindEmptyPlaceAxisLBtoRT(string[,] chessBoard, out int x, out int y)
    {
        x = -1;
        y = -1;

        if (string.IsNullOrEmpty(chessBoard[0, 0]))
        {
            x = 0;
            y = 0;
            return;
        }
        else if (string.IsNullOrEmpty(chessBoard[1, 1]))
        {
            x = 1;
            y = 1;
            return;
        }
        else if (string.IsNullOrEmpty(chessBoard[2, 2]))
        {
            x = 2;
            y = 2;
            return;
        }
    }

    void FindEmptyPlaceAxisLTtoRB(string[,] chessBoard, out int x, out int y)
    {
        x = -1;
        y = -1;

        if (string.IsNullOrEmpty(chessBoard[0, 2]))
        {
            x = 0;
            y = 2;
            return;
        }
        else if (string.IsNullOrEmpty(chessBoard[1, 1]))
        {
            x = 1;
            y = 1;
            return;
        }
        else if (string.IsNullOrEmpty(chessBoard[2, 0]))
        {
            x = 2;
            y = 0;
            return;
        }
    }
}
