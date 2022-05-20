using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;
using System.Linq;

namespace Permanence.Scripts.Cores
{

    public class SfxController : MonoBehaviour
    {
        [SerializeField]
        private List<SfxAudio> audios;
        public static SfxController instance;

        private void Awake()
        {
            var instances = GameObject.FindObjectsOfType<SfxController>();
            if (instances.Length > 1)
            {
                Destroy(this);
            }
            instance = this;
        }

        public void PlayAudio(GameSfxType type, Vector2 source)
        {
            var audioObj = new GameObject(type.ToString());
            var audioSrc = audioObj.AddComponent<AudioSource>();
            audioSrc.clip = audios.First(aud => aud.type.Equals(type)).audio;
            audioSrc.Play();
            Destroy(audioObj, audioSrc.clip.length);
        }
    }

    [Serializable]
    public class SfxAudio
    {
        public AudioClip audio;
        public GameSfxType type;
    }
}