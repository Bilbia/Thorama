using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ThorHammer : MonoBehaviour
{
    public GameObject Player;
    public GameObject snapTo;
    private Rigidbody body;
    public float snapTime = 3;

    private float dropTimer;
    private Interactable interactable;
    private bool called = false;


    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        body = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // hands = GameObject.FindGameObjectsWithTag("hand");
        // if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(inputSource))
        //     called = true;
        //     Debug.Log(called);
        
        
    }

    // protected void HandAttachedUpdate(Hand hand)
    // {
    //     // Se tocar no colisor, soltar da mão
    //     if (called){
    //         hand.AttachObject(gameObject, GrabTypes Grip, Hand.AttachmentFlags flags = Hand.AttachmentFlags.SnapOnAttach);
    //     }
    // }

    // private void FixedUpdate()
    // {

    // }

}
