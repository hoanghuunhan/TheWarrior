using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] bool  _musicSource, _effectSound;

    private void Start()
    {
        //change volume music
        if (_musicSource)
        {
            SoundManager.Ins.ChangeVolumeMusic(_slider.value);
            _slider.onValueChanged.AddListener(val => SoundManager.Ins.ChangeVolumeMusic(val));
        }

        //change volume sound effect
        if (_effectSound)
        {
            SoundManager.Ins.ChangeVolumeSound(_slider.value);
            _slider.onValueChanged.AddListener(val => SoundManager.Ins.ChangeVolumeSound(val));
        }
    }
}
