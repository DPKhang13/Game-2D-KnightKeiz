using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundClip != null)
        {
            backgroundMusic.clip = backgroundClip;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }
    public void PlayCoinSound()
    {
        if (effectsSource != null && coinClip != null)
        {
            effectsSource.PlayOneShot(coinClip);
        }
    }
    public void PlayJumpSound()
    {
        if (effectsSource != null && jumpClip != null)
        {
            effectsSource.PlayOneShot(jumpClip);
        }
    }
}
