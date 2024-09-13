using System.Collections;
using UnityEngine;

public class HitStop : Singleton<HitStop>
{
    public float defaultTimeScale = 1f;
    public float hitstopTimeScale = 0.2f;
    public float hitstopDuration = 0.1f;

    private bool isHitstopActive = false;
    private Coroutine hitstopCoroutine;

    public void Start()
    {
        LeanTween.init(100000);
    }

    public void TriggerHitstop(float customDuration = -1f)
    {
        if (isHitstopActive)
        {
            StopCoroutine(hitstopCoroutine);
        }

        float durationToUse = customDuration > 0 ? customDuration : hitstopDuration;

        hitstopCoroutine = StartCoroutine(HitstopCoroutine(durationToUse));
    }

    private IEnumerator HitstopCoroutine(float duration)
    {
        isHitstopActive = true;

        Time.timeScale = hitstopTimeScale;

        yield return new WaitForSecondsRealtime(duration);

        ResetTimeScale();
        isHitstopActive = false;
    }

    private void ResetTimeScale()
    {
        Time.timeScale = defaultTimeScale;
    }
}
