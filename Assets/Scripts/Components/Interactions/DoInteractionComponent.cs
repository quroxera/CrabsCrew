﻿using UnityEngine;

namespace Scripts.Components.Interactions
{
    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();

            if (interactable != null)
                interactable.Interact();
        }
    }
}