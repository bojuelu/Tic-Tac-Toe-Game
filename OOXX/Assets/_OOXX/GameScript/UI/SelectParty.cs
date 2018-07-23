using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectParty : MonoBehaviour
{
    public Button selectO;
    public Button selectX;

    public Text showWhoFirst;

    public delegate void HumanSelectHandler();
    public HumanSelectHandler onHumanSelectOHandler;
    public HumanSelectHandler onHumanSelectXHandler;

    void OnSelectOClick()
    {
        if (onHumanSelectOHandler != null)
            onHumanSelectOHandler();
    }

    void OnSelectXClick()
    {
        if (onHumanSelectXHandler != null)
            onHumanSelectXHandler();
    }

    private void Start()
    {
        selectO.onClick.AddListener(OnSelectOClick);
        selectX.onClick.AddListener(OnSelectXClick);
    }

    private void OnDestroy()
    {
        selectO.onClick.RemoveListener(OnSelectOClick);
        selectX.onClick.RemoveListener(OnSelectXClick);
    }
}
