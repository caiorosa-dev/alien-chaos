using UnityEngine;

public enum ClipType {
    GameOver,
    PlayerHit,
    PlayerShoot,
    EnemyHit,
    AsteroidBreak,
    RoundLose,
    RoundWin,
    AliensAlert,
    Upgrade,
    TurretShoot,
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [Space]

    [Header("Music Clips")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip battleMusicClip;
    [Space]

    [Header("SFX Clips")]
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip asteroidBreakClip;
    [SerializeField] private AudioClip roundLoseClip;
    [SerializeField] private AudioClip roundWinClip;
    [SerializeField] private AudioClip aliensAlertClip;
    [SerializeField] private AudioClip upgradeClip;
    [SerializeField] private AudioClip turretShootClip;

    public void Start()
    {
        DontDestroyOnLoad(Instance.gameObject);

        GameStateManager.Instance.onGameStateChange += HandleGameStateChange;
        HandleGameStateChange(GameState.Explore);
    }

    public void HandleGameStateChange(GameState state)
    {
        if (state == GameState.Battle)
        {
            music.clip = battleMusicClip;
            Play(ClipType.AliensAlert);
        }
        else
        {
            music.clip = musicClip;
        }

        music.Play();
    }

    public AudioClip getClipFromType(ClipType type)
    {
        switch (type)
        {
            case ClipType.GameOver:
                return gameOverClip;
            case ClipType.PlayerHit:
                return playerHitClip;
            case ClipType.PlayerShoot:
                return playerShootClip;
            case ClipType.EnemyHit:
                return enemyHitClip;
            case ClipType.AsteroidBreak:
                return asteroidBreakClip;
            case ClipType.RoundLose:
                return roundLoseClip;
            case ClipType.RoundWin:
                return roundWinClip;
            case ClipType.AliensAlert:
                return aliensAlertClip;
            case ClipType.Upgrade:
                return upgradeClip;
            case ClipType.TurretShoot:
                return turretShootClip;
            default:
                return null;
        }
    }

    public void Play(ClipType clip)
    {
        AudioClip clipToPlay = getClipFromType(clip);

        if (clipToPlay != null)
        {
            //sfx.pitch = Random.Range(0.9f, 1.1f);
            sfx.PlayOneShot(clipToPlay);
        }
    }
}
