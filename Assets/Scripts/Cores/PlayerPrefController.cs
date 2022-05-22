using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Permanence.Scripts.UI;

namespace Permanence.Scripts.Cores
{
    public class PlayerPrefController : MonoBehaviour
    {
        public static PlayerPrefController instance;
        [SerializeField]
        private string startSceneName;
        public string PlayerName { get; private set; }
        public int GenerationNumber { get; private set; }

        private void Awake() {
            var instances = GameObject.FindObjectsOfType<PlayerPrefController>();
            if (instances.Length > 1)
            {
                Destroy(gameObject);
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            GenerationNumber = 1;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == startSceneName)
            {
                GenerationNumber = 1;
            }
        }
        
        public void IncreaseGeneration() {
            ++GenerationNumber;
        }

        public void SetName(string name)
        {
            this.PlayerName = name;
        }
    }
}