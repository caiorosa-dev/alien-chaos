using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HideUIOnBattle : MonoBehaviour
{
    public Vector3 hidePosition;
    public bool showInBattle = true;

    public float hideSpeed = 4f;
    public float showSpeed = 4f;

    private Vector3 showLocation;
    private Vector3 hideLocation;

    void Start()
    {
        showLocation = transform.localPosition;
        hideLocation = transform.localPosition + hidePosition;

        if (showInBattle)
        {
            transform.localPosition = hideLocation;
        }

        GameStateManager.Instance.onGameStateChange += (GameState newState) => {
            if (newState == GameState.Battle)
            {
                if (showInBattle) Show();
                else Hide();
            }
            else
            {
                if (showInBattle) Hide();
                else Show();
            }
        };
    }

    public void Show()
    {
        transform.LeanMoveLocal(showLocation, showSpeed).setEaseOutCubic();
    }

    public void Hide()
    {
        transform.LeanMoveLocal(hideLocation, hideSpeed).setEaseInCubic();
    }
}
