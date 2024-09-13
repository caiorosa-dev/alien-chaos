using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterTexts : MonoBehaviour
{
    public TextMeshProUGUI relaxText;
    public TextMeshProUGUI attackText;

    void Update()
    {
        relaxText.text = formatText("Relax Time", GameStateManager.Instance.getRemainingTimeToNextTurn());
        attackText.text = formatText("Alien Attack", WaveManager.Instance.getRemainingTimeToEndWave());
    }

    string formatText(string text, int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        // Formata os minutos e segundos para sempre ter dois dígitos
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);

        return text + " " + formattedTime;
    }
}
