using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] string horizontalInputAxis;
    [SerializeField] string verticalInputAxis;
    [SerializeField] string pickupInputButton;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] Transform pickupPositionTransform;

    [SerializeField] float pickupRange = 1;

    [Range(1,10)]
    [SerializeField] float speed = 1;

     new Rigidbody rigidbody;

    IPickupable currentPickup;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMovement();
        UpdatePickup();
    }


    void UpdateMovement()
    {
        float inputX = Input.GetAxis(horizontalInputAxis);
        float inputY = Input.GetAxis(verticalInputAxis);

        rigidbody.velocity = new Vector3(inputX * speed, rigidbody.velocity.y, inputY * speed);

        if(rigidbody.velocity.magnitude > 0.5f)
        {
            var vel = rigidbody.velocity;
            vel.y = 0;

            transform.forward = vel;
        }

    }


    void UpdatePickup()
    {
        bool pickupClicked = Input.GetButtonDown(pickupInputButton);

        if (pickupClicked)
        {
            Debug.Log(name + ": pickupClicked");

            if(currentPickup == null)
            {
                TryPickup();
            }
            else
            {
                Release();
            }
        }

    }

    void TryPickup()
    {
        //look for pickup

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, pickupRange, Vector3.up, 0.1f, pickupLayer, QueryTriggerInteraction.Collide);

        IPickupable minPickup = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];

            IPickupable pickupable = hit.transform.GetComponent<IPickupable>();

            if(pickupable != null)
            {

                float currentDistance = Vector3.Distance(transform.position, hit.transform.position);

                if(currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    minPickup = pickupable;
                }
            }
        }

        if(minPickup != null)
        {
            currentPickup = minPickup;
            currentPickup.GetRigidbody().isKinematic = true;
            currentPickup.GetCollider().enabled = false;
            var pickupTransform = currentPickup.GetTransform();

            pickupTransform.parent = transform;
            pickupTransform.position = pickupPositionTransform.position;
        }

    }

    void Release()
    {
        currentPickup.GetTransform().parent = null;
        currentPickup.GetRigidbody().isKinematic = false;
        currentPickup.GetCollider().enabled = true;
        currentPickup = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

}