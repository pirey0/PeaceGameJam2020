using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObject : MonoBehaviour, IPickupable
{

    [SerializeField] Transform targetTransform;
    [SerializeField] GameObject prefab;

    public void CauseEffect()
    {
        Instantiate(prefab, targetTransform.position, targetTransform.rotation);
    }

    public Collider GetCollider()
    {
        throw new System.NotImplementedException();
    }

    public Rigidbody GetRigidbody()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetTransform()
    {
        throw new System.NotImplementedException();
    }

    public bool ShouldPickup()
    {
        return false;
    }
}
