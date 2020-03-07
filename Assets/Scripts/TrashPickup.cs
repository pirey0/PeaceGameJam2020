using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPickup : MonoBehaviour, IPickupable
{
    new Rigidbody rigidbody;
    Collider collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public Collider GetCollider()
    {
        return collider;
    }

    public Rigidbody GetRigidbody()
    {
        return rigidbody;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool ShouldPickup()
    {
        return true;
    }

    public void CauseEffect()
    {

    }
}
