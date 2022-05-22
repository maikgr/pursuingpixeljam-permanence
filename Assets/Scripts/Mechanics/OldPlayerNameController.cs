using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;
using TMPro;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(GameCard))]
    public class OldPlayerNameController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameText;
        private GameCard gameCard;

        private void Awake() {
            gameCard = GetComponent<GameCard>();
        }

        public void SetName(string name)
        {
            nameText.text = name;
            gameCard.cardName = name;
        }
    }
}