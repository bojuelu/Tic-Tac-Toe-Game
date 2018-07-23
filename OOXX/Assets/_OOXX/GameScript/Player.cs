using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Types
    {
        None,

        O,
        X
    }
    public Types type = Types.None;

    public enum Controller
    {
        Human,
        Computer,
    }
    public Controller controller = Controller.Computer;

    GameAI gameAI;

    public delegate void PlaceChessDoneHandler(Player player, int x, int y, bool placeSuccess);
    public PlaceChessDoneHandler onPlaceChessDone;

    void Start()
    {
    }

    public void LatePlaceChessViaComputer(GameRule gameRule)
    {
        StartCoroutine(PlaceChessViaComputerFakeThinking(gameRule));
    }

    private IEnumerator PlaceChessViaComputerFakeThinking(GameRule gameRule)
    {
        float thinkingSec = Random.Range(1f, 3f);

//        Debug.Log("PlaceChessViaComputerFakeThinking wait for " + thinkingSec + " secs...");
        yield return new WaitForSeconds(thinkingSec);

        PlaceChessViaComputer(gameRule);
    }

    public bool PlaceChessViaComputer(GameRule gameRule)
    {
        Debug.Log("PlaceChessViaComputer");

        if (gameAI == null)
            gameAI = gameObject.AddComponent<GameAI>();
        int placeX;
        int placeY;
        bool result = gameAI.ThinkingAndPlace(this, gameRule, out placeX, out placeY);

        Debug.Log(string.Format("[PlaceChessViaComputer] playerType={0} placeX={1} placeY={2}", this.type, placeX, placeY));

        if (onPlaceChessDone != null)
        {
            Debug.Log(string.Format("[PlaceChessViaComputer] Invoke done event"));
            onPlaceChessDone(this, placeX, placeY, result);
        }
        return result;
    }

    public bool PlaceChessViaHuman(GameRule gameRule, int x, int y)
    {
        Debug.Log("PlaceChessViaHuman");
        Debug.Log(string.Format("[PlaceChessViaHuman] playerType={0} placeX={1} placeY={2}", this.type, x, y));

        bool result = gameRule.Occupied(type, x, y);
        if (onPlaceChessDone != null)
        {
            Debug.Log(string.Format("[PlaceChessViaHuman] Invoke done event"));
            onPlaceChessDone(this, x, y, result);
        }
        return result;
    }

}
