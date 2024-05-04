using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource audioSource;

    [SerializeField]
    private float mainSceneTempo = 1.0f;  // Normal speed
    [SerializeField]
    private float battleSceneTempo = 1.25f;  // Increased speed

    private string mainSceneName = "MainScene";  // Your main scene's name
    private string battleSceneName = "BattleScene";  // Your battle scene's name

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusic();
    }

    private void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true; // Loop the music
            audioSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainSceneName)
        {
            audioSource.pitch = mainSceneTempo;
        }
        else if (scene.name == battleSceneName)
        {
            audioSource.pitch = battleSceneTempo;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Clean up delegate to avoid memory leaks
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
