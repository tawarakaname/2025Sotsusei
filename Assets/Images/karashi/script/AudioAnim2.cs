using UnityEngine;

public class AudioAnim2 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip SoundEffect;

    public void PlaysecondSound()
    {
        audioSource.PlayOneShot(SoundEffect);
    }
}
