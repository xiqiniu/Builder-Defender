using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    public static SoundManager Instance { get; private set; }
    private float volume = 0.5f;
    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver,
    }
    private AudioSource audioSource;
    private void Awake()
    {
        Instance = this;
        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        audioSource = GetComponent<AudioSource>();
        foreach(Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }
    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound],volume);
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public float GetVolume()
    {
        return volume;
    }

}
