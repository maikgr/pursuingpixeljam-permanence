using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.UI
{
    public class NameInputController : MonoBehaviour
    {
        [SerializeField]
        private string gameSceneName;
        public string PlayerName { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                this.PlayerName = "Player";
            }
            else
            {
                this.PlayerName = name;
            }
        }

        public void StartGame()
        {
            PlayerPrefController.instance.SetName(this.PlayerName);
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}