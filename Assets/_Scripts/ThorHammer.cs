using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ThorHammer : MonoBehaviour
{
    public GameObject leftHandObj;
    public GameObject rightHandObj;
    public GameObject head;
    public GameObject hammerHandObj;

    public AudioSource audiosource;

    [SerializeField]
    private float leftHandY;
    [SerializeField]
    private float rightHandY;
    [SerializeField]
    private float headY;

    private float dropTimer;
    private Interactable interactable;
    [SerializeField]
    private bool called = false;
    bool arrived = false;

    public SteamVR_Action_Boolean grabPinch;
    public SteamVR_Input_Sources leftInput = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources rightInput = SteamVR_Input_Sources.RightHand;
    public Hand leftHand;
    public Hand rightHand;

    public ParticleSystem lightning;

    private Vector3 leftPos = new Vector3(50, 15.67f, -2.5f);
    private Quaternion leftRot = Quaternion.Euler(272, -78.2f, 171.9f);
    private Vector3 rightPos = new Vector3(-46.31f, 11.8f, 2.8f);
    private Quaternion rightRot = Quaternion.Euler(272, -78.2f, -1);
    

    GameManager gm;



    void Start()
    {
        interactable = GetComponent<Interactable>();
        ParticleSystem.EmissionModule emission = lightning.emission;
        emission.enabled = false;
        gm = GameManager.GetInstance();

        audiosource.Play();
    }

    void Update()
    {
        leftHandY = leftHandObj.transform.position.y;
        rightHandY = rightHandObj.transform.position.y;
        headY = head.transform.position.y;

        if (gm.gameState == GameManager.GameState.WORTHY && SteamVR_Actions.default_GrabPinch.GetStateDown(leftInput) && leftHandY > headY){
            called = true;
            StartCoroutine(MoveFromTo(gameObject.transform, transform.position, leftHandObj, 12.0f, leftInput, leftHand));
        }
        if (gm.gameState == GameManager.GameState.WORTHY && SteamVR_Actions.default_GrabPinch.GetStateDown(rightInput) && rightHandY > headY){
            called = true;
            StartCoroutine(MoveFromTo(gameObject.transform, transform.position, rightHandObj, 12.0f, rightInput, rightHand));
        }
        
        if (leftHand.ObjectIsAttached(gameObject) && SteamVR_Actions.default_GrabGrip.GetState(leftInput)){
            ParticleSystem.EmissionModule emission = lightning.emission;
            emission.enabled = true;
        }

        if (rightHand.ObjectIsAttached(gameObject) && SteamVR_Actions.default_GrabGrip.GetState(rightInput)){
            ParticleSystem.EmissionModule emission = lightning.emission;
            emission.enabled = true;
        }



        if (SteamVR_Actions.default_GrabGrip.GetStateUp(leftInput) || SteamVR_Actions.default_GrabGrip.GetStateUp(rightInput)){
            ParticleSystem.EmissionModule emission = lightning.emission;
            emission.enabled = false;
        }
        
        
    }

    protected void HandAttachedUpdate(Hand hand)
    {
        if (called){
            GrabTypes grip = hand.GetBestGrabbingType();
            Hand.AttachmentFlags attFlags = gameObject.GetComponent<Throwable>().attachmentFlags | Hand.AttachmentFlags.SnapOnAttach;
            hand.AttachObject(gameObject, grip, attFlags);

            // hammerHandObj.transform.position = rightPos;

            if(hand == leftHand){
                hammerHandObj.transform.localRotation = leftRot;
                hammerHandObj.transform.localPosition = leftPos;
            }
            else{
                hammerHandObj.transform.localRotation = rightRot;
                hammerHandObj.transform.localPosition = rightPos;
            }
        }

        if (hand.IsGrabEnding(this.gameObject))
        {
            hand.DetachObject(gameObject, false);
            called = false;
            arrived = false;
        }
    }

    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, GameObject handObj, float speed, SteamVR_Input_Sources input, Hand hand)
    {
        Vector3 b = handObj.transform.position;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        float dist;
        while (!arrived && SteamVR_Actions.default_GrabPinch.GetState(input))
        {
            b = handObj.transform.position;
            t += step;
            objectToMove.position = Vector3.Lerp(a, b, t);
            dist = Vector3.Distance(objectToMove.position, b);
            // Debug.Log(dist);
            if(dist == 0){
                arrived = true;
                // Debug.Log(arrived);
                HandAttachedUpdate(hand);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
