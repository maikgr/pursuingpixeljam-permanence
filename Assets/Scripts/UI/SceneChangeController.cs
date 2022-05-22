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
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
    }
}