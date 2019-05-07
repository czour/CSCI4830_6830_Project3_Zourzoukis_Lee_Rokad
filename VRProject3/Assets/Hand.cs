using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public OVRInput.Controller myController;
    PickupObject currentAttachment = null;
    public float pickupTriggerThresh;
    public float releaseTriggerThresh;
    public bool disappearOnPickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other){
        Rigidbody rb = other.attachedRigidbody;
        if(rb == null){
            return;
        }
        PickupObject p = rb.GetComponent<PickupObject>();
        if(p == null) {
            return;
        }
        float triggerValue;
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, myController);
        if(currentAttachment == null && triggerValue > pickupTriggerThresh ) {
            currentAttachment = p;
            currentAttachment.pickedUp(this.transform);
            if(disappearOnPickup) {
                MeshRenderer[]  meshRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer m in meshRenderers) {
                 m.enabled = false;
                }
            }
        }
        if(currentAttachment != null && triggerValue < releaseTriggerThresh) {
            currentAttachment.released(this.transform, OVRInput.GetLocalControllerVelocity(myController));
            currentAttachment = null;
            if(disappearOnPickup) {
                MeshRenderer[]  meshRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer m in meshRenderers) {
                 m.enabled = true;
                }
            }
        }
        Debug.Log("intersecting pickup object " + p.name + " at " + Time.time);
    }
}
