using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageChute : MonoBehaviour, IInteractable
{
    [SerializeField] Transform target;  
    [SerializeField] Animator animator;
    public bool CanInteractWith(PlayerController playerController){
        return true;
    }

    public void InteractWith(PlayerController playerController){
        if(playerController.CurrentPickup!=null){
            IPickupable garbage = playerController.CurrentPickup;
            playerController.ForceRelease();
            garbage.GetTransform().position = target.position;
            animator.SetTrigger("Open");
        }
        else{
            // Feedback nothing to throw away
        }
    }


}
