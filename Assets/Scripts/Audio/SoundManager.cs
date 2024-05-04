using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;

    // Dictionary to store unit sounds
    public AudioClip solitudeAttack;
    public AudioClip redlineAttack;
    public AudioClip catalystAttack;
    public AudioClip brujahAttack;
    public AudioClip z3nAttack;

    void Awake()
    {
        // Log to check if another instance was already present
        if (instance != null)
        {
            Debug.LogWarning("Another instance of SoundManager was detected and will be destroyed.");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            // Ensuring it persists across scenes
            DontDestroyOnLoad(gameObject);
        }
        Debug.Log("SoundManager instance was set.");
    }

    public void PlayAttackSound(string unitName)
    {
        Debug.Log($"Attempting to play attack sound for unit: {unitName}");
        AudioClip clip = null;
        switch (unitName)
        {
            case "Solitude":
                clip = solitudeAttack;
                break;
            case "Redline":
                clip = redlineAttack;
                break;
            case "Catalyst":
                clip = catalystAttack;
                break;
            case "Brujah":
                clip = brujahAttack;
                break;
            case "Z3N":
                clip = z3nAttack;
                break;
            default:
                Debug.LogWarning($"No attack sound available for unit: {unitName}");
                return;
        }

        if (clip != null)
        {
            Debug.Log($"Playing sound clip: {clip.name}");
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Audio clip for {unitName} is not assigned or is missing.");
        }
    }
}
