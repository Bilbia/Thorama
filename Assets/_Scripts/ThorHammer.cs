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

    public GameObject lightning;

    GameManager gm;



    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
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
            lightning.SetActive(true);
            // Debug.Log("lancou raio");
        }

        if (rightHand.ObjectIsAttached(gameObject) && SteamVR_Actions.default_GrabGrip.GetState(rightInput)){
            lightning.SetActive(true);
            // Debug.Log("lancou raio");
        }



        if (SteamVR_Actions.default_GrabGrip.GetStateUp(leftInput) || SteamVR_Actions.default_GrabGrip.GetStateUp(rightInput)){
            lightning.SetActive(false);
        }
        
        
    }

    protected void HandAttachedUpdate(Hand hand)
    {
        if (called){
            GrabTypes grip = hand.GetBestGrabbingType();
            Hand.AttachmentFlags attFlags = gameObject.GetComponent<Throwable>().attachmentFlags | Hand.AttachmentFlags.SnapOnAttach;
            hand.AttachObject(gameObject, grip, attFlags);
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
