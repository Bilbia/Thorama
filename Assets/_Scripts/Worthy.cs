using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Worthy : MonoBehaviour
{
    public GameObject hammer;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        // hammer = GameObject.FindWithTag("hammer");
        // hammerFake = GameObject.FindWithTag("hammer_fake");
        // hammer.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.gameState == GameManager.GameState.WORTHY){
            hammer.GetComponent<Throwable>().attachmentFlags = hammer.GetComponent<Throwable>().attachmentFlags | Hand.AttachmentFlags.ParentToHand;
            hammer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
