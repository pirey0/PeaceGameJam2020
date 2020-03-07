using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class PlayerController : MonoBehaviour
{

    [SerializeField] string horizontalInputAxis;
    [SerializeField] string verticalInputAxis;
    [SerializeField] string pickupInputButton, interactInputButton;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] Transform pickupPositionTransform;

    [SerializeField] float pickupRange = 1;

    [Range(1,10)]
    [SerializeField] float speed = 1;

    [SerializeField] Animator animator;
    [SerializeField] FastIKFabric rightArm, leftArm;
    [SerializeField] Transform rightArmNormalTarget, leftArmNormalTarget;


    new Rigidbody rigidbody;

    IPickupable currentPickup;
    IPickupable closestPickup;

    IInteractable closestInteractable;

    public IPickupable CurrentPickup { get => currentPickup; }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rightArm.Target = rightArmNormalTarget;
        leftArm.Target = leftArmNormalTarget;
    }

    void Update()
    {
        UpdateMovement();
        UpdatePickup();
        UpdateInteract();
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

        animator.SetFloat("Speed", rigidbody.velocity.magnitude);

    }

    void UpdateInteract()
    {
        closestInteractable = GetClosestObjectInRange<IInteractable>();
    }

    void UpdatePickup()
    {
        bool pickupClicked = Input.GetButtonDown(pickupInputButton);
        bool pickupReleased = Input.GetButtonUp(pickupInputButton);

        closestPickup = GetClosestObjectInRange<IPickupable>();


        if (pickupClicked && currentPickup == null)
        {
            Debug.Log(name + ": pickupClicked");

                TryPickup();
        }
        else if( pickupReleased && currentPickup != null)
        {
                Release();
        }
    }

    T GetClosestObjectInRange<T>()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, pickupRange, Vector3.up, 0.1f, pickupLayer, QueryTriggerInteraction.Collide);

        T minObject = default(T);
        float minDistance = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];

            T pickupable = hit.transform.GetComponent<T>();

            if (pickupable != null)
            {

                float currentDistance = Vector3.Distance(transform.position, hit.transform.position);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    minObject = pickupable;
                }
            }
        }

        return minObject;
    }

    void TryPickup()
    {
        //look for pickup

        if(closestPickup != null)
        {
            currentPickup = closestPickup;
            currentPickup.GetRigidbody().isKinematic = true;
            currentPickup.GetCollider().enabled = false;
            var pickupTransform = currentPickup.GetTransform();

            pickupTransform.parent = transform;
            pickupTransform.position = pickupPositionTransform.position;

            rightArm.Target = currentPickup.GetTransform();
            leftArm.Target = currentPickup.GetTransform();
        }

    }

    public void ForceRelease()
    {
        Release();
    }

    void Release()
    {
        if (closestInteractable != null && closestInteractable.CanInteractWith(this))
        {
            Debug.Log("Interacting");
            closestInteractable.InteractWith(this);
        }

        rightArm.Target = rightArmNormalTarget;
        leftArm.Target = leftArmNormalTarget;
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