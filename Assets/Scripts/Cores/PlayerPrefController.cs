using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Permanence.Scripts.Cores
{
    public class PlayerPrefController : MonoBehaviour
    {
        public static PlayerPrefController instance;
        public string PlayerName { get; private set; }
        public int GenerationNumber { get; private set; }

        private void Awake() {
            var instances = GameObject.FindObjectsOfType<PlayerPrefController>();
            if (instances.Length > 1)
            {
                Destroy(gameObject);
            }
            instance = this;
            GenerationNumber = 1;
            DontDestroyOnLoad(gameObject);
            PlayerName = "Player";
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