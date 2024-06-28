using Objects;
using UnityEngine;

public class SoundService : MonoBehaviour
{
  [SerializeField]
  private AudioSource _musicSource;
  [SerializeField]
  private AudioSource _audioSource;

  // ===================================================================================================
  public void Awake()
  {
    Global.Instance.SoundService = this;
    DontDestroyOnLoad(gameObject);

    AddVolumeMusic(ConstProvider.STANDAED_VOLUME_MUSIC);
    AddVolumeAudio(ConstProvider.STANDAED_VOLUME_AUDIO);
  }

  // ===================================================================================================
  public void PlaySound(string name)
  {
    _musicSource.clip = FindSound(name);
    _musicSource.loop = true;
    _musicSource.Play();
  }

  // ===================================================================================================
  public void PlayOne(string name)
  {
    var clip = FindSound(name);
    _audioSource.PlayOneShot(clip);
  }

  // ===================================================================================================
  public void AddVolumeMusic(float volume)
  {
    _musicSource.volume = volume;
  }

  // ===================================================================================================
  public void AddVolumeAudio(float volume)
  {
    _audioSource.volume = volume;
  }

  // ===================================================================================================
  public float GetVolumeMusic()
  {
    return _musicSource.volume;
  }

  // ===================================================================================================
  public float GetVolumeAudio()
  {
    return _audioSource.volume;
  }

  // ===================================================================================================
  public void OffSound()
  {
    _musicSource.Stop();
    _audioSource.Stop();
  }

  // ===================================================================================================
  private AudioClip FindSound(string name)
  {
    var sound = Factory.Instance.GetSound(name);
    return sound != null ? sound : Factory.Instance.GetSound("default_sound");
  }
}
