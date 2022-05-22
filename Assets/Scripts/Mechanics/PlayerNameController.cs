using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;
using TMPro;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(GameCard))]
    public class PlayerNameController : MonoBehaviour
    {
        private PlayerPrefController playerPrefController;
        [SerializeField]
        private TMP_Text nameText;
        private GameCard gameCard;
        public string CurrentName { get; private set; }

        private void Awake() {
            playerPrefController = FindObjectOfType<PlayerPrefController>();
            gameCard = GetComponent<GameCard>();
        }

        private void Start() {
            CurrentName = playerPrefController.PlayerName;
            if (playerPrefController.GenerationNumber > 1)
            {
                string genNumber = "";
                switch(playerPrefController.GenerationNumber)
                {
                    case 2:
                        genNumber = " the 2nd";
                        break;
                    case 3:
                        genNumber = " the 3rd";
                        break;
                    default:
                        genNumber = $" the {playerPrefController.GenerationNumber}th";
                        break;
                }
                CurrentName += genNumber;
            }
            nameText.text = CurrentName;

            gameCard.cardName = CurrentName;
        }
    }
}