using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Permanence.Scripts.UI
{
    public class EventNotificationController : MonoBehaviour
    {
        public static EventNotificationController instance;
        [SerializeField]
        private GameObject windowParent;
        [SerializeField]
        private TMP_Text titleText;
        [SerializeField]
        private TMP_Text descriptionText;

        private void Awake() {
            var instances = FindObjectsOfType<EventNotificationController>();
            if (instances.Length > 1)
            {
                Destroy(gameObject);
            }
            instance = this;
        }

        public void ShowNotification(string title, string description)
        {
            windowParent.gameObject.SetActive(true);
            Time.timeScale = 0;
            titleText.text = title;
            descriptionText.text = description;
        }

        public void CloseNotification()
        {
            Time.timeScale = 1;
            titleText.text = string.Empty;
            descriptionText.text = string.Empty;
            windowParent.gameObject.SetActive(false);
        }
    }
}