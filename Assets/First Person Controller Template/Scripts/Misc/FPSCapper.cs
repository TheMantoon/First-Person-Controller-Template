using UnityEngine;

public class FPSCapper : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.maxQueuedFrames = 2;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }
}