using UnityEngine;
using System.Collections;

public class SimpleScreenShake : MonoBehaviour
{
    // Singleton instance
    public static SimpleScreenShake Instance;

    // Default values for duration and magnitude
    public float defaultDuration = 0.5f;
    public float defaultMagnitude = 0.5f;
    public float smoothTime = 0.1f; // Time for the camera to smooth back to original position
    public float maxOffset = 0.3f; // Maximum distance the camera can move during shake

    // This is the transform that will be shaken
    public Transform shakeContainer; // Assign this to your 'ShakeContainer' GameObject

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Ensures there is only one instance
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: makes it persist across scene loads
        }
    }

    // Public method to trigger the shake with custom parameters
    public void ShakeCamera(float duration, float magnitude)
    {
        if (shakeContainer != null) // Check if the shakeContainer has been assigned
        {
            StartCoroutine(Shake(duration, magnitude));
        }
    }

    // Public method to trigger the shake with default parameters
    public void ShakeCameraWithDefaults()
    {
        Debug.Log("ShakeCameraWithDefaults called");
        StartCoroutine(Shake(defaultDuration, defaultMagnitude));
    }


    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = shakeContainer.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Mathf.Clamp(Random.Range(-1f, 1f) * magnitude, -maxOffset, maxOffset);
            float y = Mathf.Clamp(Random.Range(-1f, 1f) * magnitude, -maxOffset, maxOffset);

            shakeContainer.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null; // Wait until next frame
        }

        // Smoothly return to original position
        while (shakeContainer.localPosition != originalPosition)
        {
            shakeContainer.localPosition = Vector3.Lerp(shakeContainer.localPosition, originalPosition, smoothTime / duration);
            yield return null;
        }
    }
}
