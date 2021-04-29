using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioPlayManager
{
    public static AudioSource gBGMSource;

    /// <summary>
    /// 设置BGM源音量
    /// </summary>
    public static void SetBGMVolume(float volume)
    {
        if (gBGMSource != null)
            gBGMSource.volume = volume;
    }

    /// <summary>
    /// 设置音效源音量
    /// </summary>
    public static void SetSFXVolume(float volume)
    {
        var audiosources = Object.FindObjectsOfType<AudioSource>();

        foreach(AudioSource audio in audiosources)
        {
            if (!audio.Equals(gBGMSource))
                audio.volume = volume;
        }
    }

    /// <summary>
    /// 在指定位置播放音效
    /// </summary>
    /// <param name="clip">音效</param>
    /// <param name="root">播放位置</param>
    public static void PlaySceneAudioOneShoot(AudioClip clip, Transform point)
    {
        AudioSource.PlayClipAtPoint(clip, point.position);
    }

    /// <summary>
    /// 在指定音源处播放音效
    /// </summary>
    /// <param name="audio">音源</param>
    /// <param name="clip">音效</param>
    /// <param name="loop">是否循环</param>
    public static void PlaySceneAudioAtSource(AudioSource audio, AudioClip clip, bool loop = false)
    {
        audio.clip = clip;
        audio.loop = loop;
        audio.Play();
    }

    /// <summary>
    /// 菜单音效播放，在主相机位置播放
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayMenuAudio(AudioClip clip)
    {
        if (Camera.main != null)
            Camera.main.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
