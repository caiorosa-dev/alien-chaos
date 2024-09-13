using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GameState
{
    Battle,
    Explore,
}

[System.Serializable]
public class GameTurn
{
    public GameState state;
    public float duration;
}

public class GameStateManager : Singleton<GameStateManager>
{
    [SerializeField] private List<GameTurn> turns;

    [SerializeField] private int currentTurnIndex;
    [SerializeField] private float currentTurnDuration;

    public delegate void OnGameStateChange(GameState state);
    public OnGameStateChange onGameStateChange;

    public delegate void OnGameFinish();
    public OnGameFinish onGameFinish;

    void Start()
    {
        triggerTurnLoad();

        WaveManager.Instance.onWaveSuccess += handleWaveWin;
        WaveManager.Instance.onWaveFail += handleWaveLose;
    }

    void handleWaveWin()
    {
        AudioManager.Instance.Play(ClipType.RoundWin);
        currentTurnIndex++;
        if (currentTurnIndex >= turns.Count)
        {
            MenuScripts.goToSuccesScree();
            onGameFinish();
            return;
        }
        triggerTurnLoad();
    }

    void handleWaveLose()
    {
        AudioManager.Instance.Play(ClipType.RoundLose);
        currentTurnIndex--;
        triggerTurnLoad();
    }

    void triggerTurnLoad()
    {
        GameTurn currentTurn = getCurrentTurn();
        currentTurnDuration = 0;
        onGameStateChange?.Invoke(currentTurn.state);

        if (currentTurn.state == GameState.Battle)
        {
            AudioManager.Instance.Play(ClipType.AliensAlert);
            WaveManager.Instance.TriggerNextWaveStart();
        }
    }

    public GameTurn getCurrentTurn()
    {
        return turns[currentTurnIndex];
    }

    public GameState getCurrentState()
    {
        return getCurrentTurn().state;
    }

    public int getRemainingTimeToNextTurn()
    {
        return (int) (getCurrentTurn().duration - currentTurnDuration);
    }

    void Update()
    {
        currentTurnDuration += Time.deltaTime;

        if (currentTurnDuration >= getCurrentTurn().duration && getCurrentState() != GameState.Battle)
        {
            currentTurnIndex++;
            triggerTurnLoad();
        }
    }
}
