using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] tracks;       // drag your music clips here
    [SerializeField] private float musicVolume = 0.5f;

    void Awake()
    {
        // Singleton — persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        PlayTrack(0);           // plays first track on startup
    }

    public void PlayTrack(int index)
    {
        if (index < 0 || index >= tracks.Length) return;
        musicSource.clip = tracks[index];
        musicSource.Play();
    }

    public void StopMusic() { musicSource.Stop(); }
    public void PauseMusic() { musicSource.Pause(); }
    public void ResumeMusic() { musicSource.UnPause(); }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }
}