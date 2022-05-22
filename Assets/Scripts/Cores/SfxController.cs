using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;
using System.Linq;

namespace Permanence.Scripts.Cores
{

    public class SfxController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource bgmSource;
        [SerializeField]
        private List<SfxAudio> audios;
        [SerializeField]
        private Slider volumeSlider;
        public static SfxController instance;
        private float volumeAmount;

        private void Awake()
        {
            var instances = GameObject.FindObjectsOfType<SfxController>();
            if (instances.Length > 1)
            {
                Destroy(this);
            }
            instance = this;
        }

        private void Start() {
            volumeSlider.value = bgmSource.volume;
        }

        public void PlayAudio(GameSfxType type, Vector2 source)
        {
            var audioObj = new GameObject(type.ToString());
            var audioSrc = audioObj.AddComponent<AudioSource>();
            audioSrc.clip = audios.First(aud => aud.type.Equals(type)).audio;
            audioSrc.volume = volumeAmount;
            audioSrc.Play();
            Destroy(audioObj, audioSrc.clip.length);
        }

        public void AdjustVolume(float amount)
        {
            volumeAmount = amount;
            bgmSource.volume = amount;
        }
    }

    [Serializable]
    public class SfxAudio
    {
        public AudioClip audio;
        public GameSfxType type;
    }
}