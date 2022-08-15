using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggerAudio : MonoBehaviour
{
    [SerializeField] bool _toggleMusic, _toggleEffect;

    public void Toggle()
    {
        if (_toggleEffect) SoundManager.Ins.ToggleEffect();
        if (_toggleMusic) SoundManager.Ins.ToggleMusic();
    }
}
