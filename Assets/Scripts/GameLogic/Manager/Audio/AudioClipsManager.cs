using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipsManager : MonoBehaviour
{
    [Serializable]
    public struct AudioInfo
    {
        public string audioName;
        public AudioClip audioClip;
    }

    public List<AudioInfo> mAudios = new List<AudioInfo>();

    public AudioClip GetAudioClipByName(string name)
    {
        foreach(AudioInfo info in mAudios)
        {
            if (info.audioName.Equals(name))
                return info.audioClip;
            else continue;
        }

        Debug.Log("不存在该音效!!");
        return null;
    }
}
