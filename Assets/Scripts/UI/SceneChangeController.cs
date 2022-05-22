using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Permanence.Scripts.UI
{
    public class SceneChangeController : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        public void ChangeScene()
        {
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}