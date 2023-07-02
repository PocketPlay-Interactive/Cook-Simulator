using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum Music
{
    Home,
    Ingame
}

public class MusicManager : MonoSingletonGlobal<MusicManager>
{
    [System.Serializable]
    public class MusicTable
    {
        public Music music;
        public AudioClip clip;
    }

    [SerializeField] private MusicTable[] musics;
    [SerializeField] AudioSource audioSource;
    private Dictionary<Music, AudioClip> musicDics = new Dictionary<Music, AudioClip>();

    private IEnumerator Start()
    {
        foreach (var _s in musics)
        {
            musicDics.Add(_s.music, _s.clip);
        }
        yield return null;

        audioSource.mute = !RuntimeStorageData.Sound.isMusic;
        if (musics.Length == 0)
            yield break;

        PlayMusic(Music.Home);
    }

    public void PlayMusic(Music sound, float _volume = 1.0f)
    {
        PauseSound();
        audioSource.clip = ConverToClip(sound);
        audioSource.loop = true;
        audioSource.volume = _volume;
        audioSource.Play();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void UnPauseSound()
    {
        audioSource.UnPause();
    }

    AudioClip ConverToClip(Music sound)
    { 
        if (musicDics.ContainsKey(sound))
            return musicDics[sound];
        return null;
    }

    public void ChangeMusic(Music sound, float _volume = 1.0f)
    {
        audioSource.DOFade(0, 0.3f).OnComplete(() =>
        {
            PlayMusic(sound, 0);
            audioSource.DOFade(_volume, 0.3f);
        });
    }

    public void Turn(bool isEnble)
    {
        int volume = 1;
        if (isEnble == true) volume = 1;
        else volume = 0;
        audioSource.DOFade(volume, 0.3f).OnComplete(() => { audioSource.mute = !isEnble; });
    }
}
