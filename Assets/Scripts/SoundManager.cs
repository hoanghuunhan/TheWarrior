using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Ins;

    [SerializeField] private AudioSource _musicSource, _effectSound;

    public AudioClip hurtSound, deadSound, breakCrateSound, ultiSound, tadaSound, appearSound, slurp, failure, battleSong, adventureSong, introBoss, bossDie;


    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        _effectSound.PlayOneShot(clip);
    }

    public void PlayMusicBG(AudioClip clip)
    {
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void ChangeVolumeMusic(float value)
    {
        _musicSource.volume = value;
    }
    public void ChangeVolumeSound(float value)
    {
        _effectSound.volume = value;
    }
    public void ToggleEffect()
    {
        _effectSound.mute = !_effectSound.mute;
    }
    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }
}
