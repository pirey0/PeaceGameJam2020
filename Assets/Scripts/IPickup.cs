using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    Rigidbody GetRigidbody();
    Transform GetTransform();
    Collider GetCollider();
}

