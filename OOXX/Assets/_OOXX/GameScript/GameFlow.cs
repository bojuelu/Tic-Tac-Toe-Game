using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    public GameRule gameRule;
    public Player playerO;
    public Player playerX;

    private Player playerHuman;
    private Player playerComputer;
    private Player.Types gameWinner;

    public enum GameState
    {
        Init,

        SelectParty,
        DecideWhoFirst,
        PlayerOsTurn,
        PlayerXsTurn,
        CheckWhosWin,

        ShowGameResult,
    }
    GameState statusNow = GameState.Init;
    GameState statusLast = GameState.Init;

    public GameObject selectPartyPanel;
    public GameObject battleFieldPanel;
    public GameObject gameResultPanel;

    void OnHumanClickPlaceButton(int x, int y)
    {
        playerHuman.PlaceChessViaHuman(gameRule, x, y);

        BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
        battleField.onClickPlace -= OnHumanClickPlaceButton;
    }

    void OnPlayerPlaceChessDone(Player player, int placeX, int placeY, bool placeSuccess)
    {
        if (placeSuccess)
        {
            string key = string.Format("{0}{1}", placeX, placeY);
            BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
            Button button = battleField.indexTable[key];
            button.GetComponentInChildren<Text>().text = player.type.ToString();
            button.interactable = false;
        }
        else
        {
            Debug.LogError("placeSuccess is false");
        }

        player.onPlaceChessDone = null;

        statusNow = GameState.CheckWhosWin;
    }

    void StartBattleFromPlayerO()
    {
        selectPartyPanel.SetActive(false);
        battleFieldPanel.SetActive(true);
        gameResultPanel.SetActive(false);

        BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
        battleField.EnableAllButton();
        battleField.showHumanPlayerParty.text = "You are player:" + playerHuman.type.ToString();

        statusNow = GameState.PlayerOsTurn;
    }

    void StartBattleFromPlayerX()
    {
        selectPartyPanel.SetActive(false);
        battleFieldPanel.SetActive(true);
        gameResultPanel.SetActive(false);

        BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
        battleField.EnableAllButton();
        battleField.showHumanPlayerParty.text = "You are player:" + playerHuman.type.ToString();

        statusNow = GameState.PlayerXsTurn;
    }

    void PlayAgain()
    {
        if (statusNow != GameState.ShowGameResult)
        {
            Debug.LogError("can not playagain at this status= " + statusNow);
            return;
        }

        GameResult gameResult = gameResultPanel.GetComponent<GameResult>();
        gameResult.playAgainButton.onClick.RemoveListener(this.PlayAgain);
        statusNow = GameState.Init;
    }

    void UpdateGameStatus()
    {
        bool isStatusChanged = false;
        if (statusNow != statusLast)
        {
            Debug.Log(string.Format("GameFlow statusNow={0} statusLast={1}", statusNow, statusLast));
            isStatusChanged = true;
        }

        switch (statusNow)
        {
            case GameState.Init:
                {
                    if (isStatusChanged)
                    {
                        statusLast = statusNow;
                    }

                    selectPartyPanel.SetActive(false);
                    battleFieldPanel.SetActive(false);
                    gameResultPanel.SetActive(false);

                    gameRule.InitChessboard();

                    GameAI ai = playerO.GetComponent<GameAI>();
                    if (ai != null)
                        GameObject.Destroy(ai);
                    ai = playerX.GetComponent<GameAI>();
                    if (ai != null)
                        GameObject.Destroy(ai);

                    BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
                    battleField.DisaleAllButton();
                    battleField.ClearAllButtonText();

                    playerHuman = playerComputer = null;
                    gameWinner = Player.Types.None;

                    statusNow = GameState.SelectParty;
                }
                break;
            case GameState.SelectParty:
                {
                    if (isStatusChanged)
                    {
                        selectPartyPanel.SetActive(true);
                        battleFieldPanel.SetActive(false);
                        gameResultPanel.SetActive(false);

                        SelectParty selectParty = selectPartyPanel.GetComponent<SelectParty>();
                        selectParty.selectO.interactable = true;
                        selectParty.selectX.interactable = true;

                        selectParty.onHumanSelectOHandler = () => {
                            playerO.controller = Player.Controller.Human;
                            playerX.controller = Player.Controller.Computer;

                            playerHuman = playerO;
                            playerComputer = playerX;

                            selectParty.onHumanSelectOHandler = null;

                            selectParty.selectO.interactable = false;
                            selectParty.selectX.interactable = false;

                            statusNow = GameState.DecideWhoFirst;
                        };
                        selectParty.onHumanSelectXHandler = () => {
                            playerO.controller = Player.Controller.Computer;
                            playerX.controller = Player.Controller.Human;

                            playerHuman = playerX;
                            playerComputer = playerO;

                            selectParty.onHumanSelectXHandler = null;

                            selectParty.selectO.interactable = false;
                            selectParty.selectX.interactable = false;

                            statusNow = GameState.DecideWhoFirst;
                        };

                        statusLast = statusNow;
                    }
                }
                break;
            case GameState.DecideWhoFirst:
                {
                    if (isStatusChanged)
                    {
                        int num = Random.Range(10, 100000);
                        num %= 2;

                        SelectParty selectParty = selectPartyPanel.GetComponent<SelectParty>();

                        if (num >= 1)
                        {
                            selectParty.showWhoFirst.text = "Player O go first, ready to start battle!";
                            Invoke("StartBattleFromPlayerO", 3f);
                        }
                        else
                        {
                            selectParty.showWhoFirst.text = "Player X go first, ready to start battle!";
                            Invoke("StartBattleFromPlayerX", 3f);
                        }

                        statusLast = statusNow;
                    }
                }
                break;
            case GameState.PlayerOsTurn:
                {
                    if (isStatusChanged)
                    {
                        Player nowPlayer = playerO;

                        BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
                        battleField.nowPlayingParty.text = Player.Types.O.ToString();

                        nowPlayer.onPlaceChessDone = this.OnPlayerPlaceChessDone;

                        Debug.Log("nowPlayer.controller= " + nowPlayer.controller.ToString());

                        if (nowPlayer.controller == Player.Controller.Computer)
                        {
                            Debug.Log("<color=white>Now is computer trun</color>");
                            nowPlayer.LatePlaceChessViaComputer(gameRule);
                        }
                        else if (nowPlayer.controller == Player.Controller.Human)
                        {
                            Debug.Log("<color=white>Now is human trun</color>");
                            battleField.onClickPlace += this.OnHumanClickPlaceButton;
                        }
                        else
                        {
                            Debug.LogError("Player type error");
                        }

                        statusLast = statusNow;
                    }
                }
                break;
            case GameState.PlayerXsTurn:
                {
                    if (isStatusChanged)
                    {
                        Player nowPlayer = playerX;

                        BattleField battleField = battleFieldPanel.GetComponent<BattleField>();
                        battleField.nowPlayingParty.text = Player.Types.X.ToString();

                        Debug.Log("nowPlayer.controller= " + nowPlayer.controller.ToString());

                        nowPlayer.onPlaceChessDone = this.OnPlayerPlaceChessDone;

                        if (nowPlayer.controller == Player.Controller.Computer)
                        {
                            Debug.Log("<color=white>Now is computer trun</color>");
                            nowPlayer.LatePlaceChessViaComputer(gameRule);
                        }
                        else if (nowPlayer.controller == Player.Controller.Human)
                        {
                            Debug.Log("<color=white>Now is human trun</color>");
                            battleField.onClickPlace += this.OnHumanClickPlaceButton;
                        }
                        else
                        {
                            Debug.LogError("Player type error");
                        }

                        statusLast = statusNow;
                    }
                }
                break;
            case GameState.CheckWhosWin:
                {
                    if (isStatusChanged)
                    {
                        gameWinner = gameRule.CheckWinner();
                        Debug.Log("gameWinner is " + gameWinner.ToString());

                        if (gameWinner != Player.Types.None)
                        {
                            statusNow = GameState.ShowGameResult;  // Someone win
                        }
                        else
                        {
                            if (gameRule.CheckChessBoardIsFull())
                            {
                                statusNow = GameState.ShowGameResult; // Game draw
                            }
                            else
                            {
                                switch (statusLast)
                                {
                                    case GameState.PlayerOsTurn:
                                        statusNow = GameState.PlayerXsTurn;
                                        break;
                                    case GameState.PlayerXsTurn:
                                        statusNow = GameState.PlayerOsTurn;
                                        break;
                                    default:
                                        Debug.LogError("statusLast error=" + statusLast.ToString());
                                        break;
                                }
                            }
                        }

                        statusLast = GameState.CheckWhosWin;
                    }
                }
                break;
            case GameState.ShowGameResult:
                {
                    if (isStatusChanged)
                    {
                        selectPartyPanel.SetActive(false);
                        battleFieldPanel.SetActive(true);
                        gameResultPanel.SetActive(true);

                        GameResult gameResult = gameResultPanel.GetComponent<GameResult>();
                        if (gameWinner == Player.Types.None)
                            gameResult.playerText.text = "Game draw";
                        else
                            gameResult.playerText.text = string.Format("Player {0} is winner!", gameWinner.ToString());
                        gameResult.playAgainButton.onClick.AddListener(this.PlayAgain);

                        statusLast = statusNow;
                        break;
                    }
                }
                break;
        }
    }

    void Update()
    {
        UpdateGameStatus();
    }
}
