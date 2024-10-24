using UnityEngine;

public class AudioAnim1 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SoundEffect;

    public void PlayfirstSound()
    {
        audioSource.PlayOneShot(SoundEffect);
    }
}
