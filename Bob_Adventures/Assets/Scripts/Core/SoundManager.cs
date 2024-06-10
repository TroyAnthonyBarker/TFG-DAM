using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null & instance != this)
        {
            Destroy(gameObject);
        }
        
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}
