using Cinemachine;

public class CameraShake : Singleton<CameraShake>
{
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.3f;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private bool isShaking = false;
    private bool ignoreNewShake = false;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float customShakeDuration = -1f, float customIntensity = 0.3f)
    {
        if (ignoreNewShake) return;

        if (isShaking)
        {
            CancelInvoke("StopShake");
        }

        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (channelPerlin != null)
        {
            float duration = customShakeDuration > 0 ? customShakeDuration : shakeDuration;

            channelPerlin.m_AmplitudeGain = shakeIntensity;
            isShaking = true;
            Invoke("StopShake", duration);
        }
    }

    void StopShake()
    {
        if (channelPerlin != null)
        {
            channelPerlin.m_AmplitudeGain = 0f;
        }
        isShaking = false; // Shake has ended
        if (ignoreNewShake)
        {
            ignoreNewShake = false;
        }
    }
}
