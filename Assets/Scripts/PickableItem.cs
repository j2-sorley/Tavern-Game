using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    //public CollisionDetectionMode collisionDetectionMode;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRigidbodyState(true);
        EnableGravity(true);
    }

    public void PickUp(Transform destination)
    {
        transform.SetParent(destination);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        SetRigidbodyState(true);
        EnableGravity(false);
    }

    private void SetRigidbodyState(bool state)
    {
        if (rb != null)
        {
            rb.isKinematic = state;
            rb.useGravity = !state;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    private void EnableGravity(bool state)
    {
        if (rb != null)
        {
            rb.useGravity = state;
        }
    }

    public void Throw()
    {
        // Detach the item from your hand and add forward force to it for the throwing effect
        transform.SetParent(null);

        // Turn on Rigidbody and Gravity again for a correct fall
        SetRigidbodyState(false);
        EnableGravity(true);

        // Adding forward force for a throwing effect
        rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
    }
}
