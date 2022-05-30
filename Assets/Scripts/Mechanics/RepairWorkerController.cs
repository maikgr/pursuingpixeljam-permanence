using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Permanence.Scripts.Constants;
using Permanence.Scripts.Cores;

namespace Permanence.Scripts.Mechanics
{
    [RequireComponent(typeof(StackableCard))]
    public class RepairWorkerController : MonoBehaviour
    {
        private StackableCard stack;

        private void Awake() {
            stack = GetComponent<StackableCard>();
        }

        private void Start() {
            stack.AddEventListener(StackableCardEvent.ON_STACKED, Repair);
        }

        private void OnDestroy() {
            stack.RemoveEventListener(StackableCardEvent.ON_STACKED, Repair);
        }

        private void Repair(IEnumerable<StackableCard> stacks) {
            var other = stacks.Last();
            if (other == null) return;

            var structure = other.GetComponent<StructureCard>();
            if (structure == null) return;
            if (structure.CurrentHealth < structure.MaxHealth)
            {
                structure.RestoreHealth();
                Destroy(gameObject);
            }
        }
    }
}