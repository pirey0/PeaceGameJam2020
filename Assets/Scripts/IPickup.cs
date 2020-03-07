using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    Rigidbody GetRigidbody();
    Transform GetTransform();
    Collider GetCollider();
}

public interface IInteractable
{
    bool CanInteractWith(IPickupable currentPickup);

    void InteractWith(IPickupable currentPickup);
}