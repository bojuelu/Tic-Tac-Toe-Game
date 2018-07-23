using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleField : MonoBehaviour
{
    public Button place00;
    public Button place10;
    public Button place20;

    public Button place01;
    public Button place11;
    public Button place21;

    public Button place02;
    public Button place12;
    public Button place22;

    public Text showHumanPlayerParty;
    public Text nowPlayingParty;

    public delegate void ClickPlaceButtonHandler(int x, int y);
    public ClickPlaceButtonHandler onClickPlace;

    public Dictionary<string, Button> indexTable = new Dictionary<string, Button>();

    private void Start()
    {
        place00.onClick.AddListener(OnPlace00Click);
        place10.onClick.AddListener(OnPlace10Click);
        place20.onClick.AddListener(OnPlace20Click);

        place01.onClick.AddListener(OnPlace01Click);
        place11.onClick.AddListener(OnPlace11Click);
        place12.onClick.AddListener(OnPlace12Click);

        place02.onClick.AddListener(OnPlace02Click);
        place21.onClick.AddListener(OnPlace21Click);
        place22.onClick.AddListener(OnPlace22Click);

        indexTable.Add("00", place00);
        indexTable.Add("10", place10);
        indexTable.Add("20", place20);

        indexTable.Add("01", place01);
        indexTable.Add("11", place11);
        indexTable.Add("12", place12);

        indexTable.Add("02", place02);
        indexTable.Add("21", place21);
        indexTable.Add("22", place22);
    }

    private void OnDestroy()
    {
        place00.onClick.RemoveListener(OnPlace00Click);
        place10.onClick.RemoveListener(OnPlace10Click);
        place20.onClick.RemoveListener(OnPlace20Click);

        place01.onClick.RemoveListener(OnPlace01Click);
        place11.onClick.RemoveListener(OnPlace11Click);
        place12.onClick.RemoveListener(OnPlace12Click);

        place02.onClick.RemoveListener(OnPlace02Click);
        place21.onClick.RemoveListener(OnPlace21Click);
        place22.onClick.RemoveListener(OnPlace22Click);
    }

    void OnPlace00Click()
    {
        if (onClickPlace != null)
            onClickPlace(0, 0);
    }

    void OnPlace10Click()
    {
        if (onClickPlace != null)
            onClickPlace(1, 0);
    }

    void OnPlace20Click()
    {
        if (onClickPlace != null)
            onClickPlace(2, 0);
    }

    void OnPlace01Click()
    {
        if (onClickPlace != null)
            onClickPlace(0, 1);
    }

    void OnPlace11Click()
    {
        if (onClickPlace != null)
            onClickPlace(1, 1);
    }

    void OnPlace21Click()
    {
        if (onClickPlace != null)
            onClickPlace(2, 1);
    }

    void OnPlace02Click()
    {
        if (onClickPlace != null)
            onClickPlace(0, 2);
    }

    void OnPlace12Click()
    {
        if (onClickPlace != null)
            onClickPlace(1, 2);
    }

    void OnPlace22Click()
    {
        if (onClickPlace != null)
            onClickPlace(2, 2);
    }

    public void EnableAllButton()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
            b.interactable = true;
    }

    public void DisaleAllButton()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
            b.interactable = false;
    }

    public void ClearAllButtonText()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
            b.GetComponentInChildren<Text>().text = "";
    }
}
