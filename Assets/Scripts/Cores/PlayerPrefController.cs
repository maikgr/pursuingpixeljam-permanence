using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            GenerationNumber = 1;
        }

        private void Start() {
            if (SceneManager.GetActiveScene().name == startSceneName)
            {
                GenerationNumber = 1;
                PlayerName = "Player";
            }
        }

        public void SetName(string name) {
            if (string.IsNullOrEmpty(name))
            {
                this.PlayerName = "Player";
            }
            else
            {
                this.PlayerName = name;
            }
        }
        
        public void IncreaseGeneration() {
            ++GenerationNumber;
        }
    }
}