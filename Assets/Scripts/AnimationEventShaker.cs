using UnityEngine;

public class AnimationEventShaker : MonoBehaviour
{
    public void TriggerShake()
    {
        // Access the singleton instance of SimpleScreenShake and trigger the shake
        if (SimpleScreenShake.Instance != null)
        {
            SimpleScreenShake.Instance.ShakeCameraWithDefaults();
        }
        else
        {
            Debug.LogError("SimpleScreenShake instance not found. Make sure it is initialized.");
        }
    }
}
