using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    Rigidbody GetRigidbody();
    Transform GetTransform();
    Collider GetCollider();
    void CauseEffect();

    bool ShouldPickup();
}

public interface IInteractable
{
    bool CanInteractWith(PlayerController playerController);

    void InteractWith(PlayerController playerController);
}