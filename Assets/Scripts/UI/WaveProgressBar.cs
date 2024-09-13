using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveProgressBar : MonoBehaviour
{
    private Image progresBar;

    private void Start()
    {
        progresBar = GetComponent<Image>();
        progresBar.fillAmount = 0;

        WaveManager.Instance.onWaveProgress += (float progress) =>
        {
            progresBar.fillAmount = progress;
        };

        GameStateManager.Instance.onGameStateChange += (GameState newState) => {
            if (newState == GameState.Battle)
            {
                progresBar.fillAmount = 0;
            }
        };
    }
}
