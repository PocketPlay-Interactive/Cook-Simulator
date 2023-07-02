using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsSoundID;
    public bool IsSoundClip;
    public bool IsAwake;
    public AudioClip _audio;
    public Sound _sound = Sound.Button;

    private void OnEnable()
    {
        if (IsAwake) PlaySound();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySound();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    private void PlaySound()
    {

        if (IsSoundClip) SoundManager.Instance.PlayOnShot(_audio);
        if (IsSoundID) SoundManager.Instance.PlayOnShot(_sound);
    }
}
