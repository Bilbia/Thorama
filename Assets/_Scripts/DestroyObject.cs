using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    public AudioSource audiosource;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnParticleCollision(GameObject gameObject)
    {
        Debug.Log("particle hit");
        Destroy(this.gameObject);
        audiosource.Play();
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "hammer"){
            Destroy(this.gameObject);
            audiosource.Play();
        }
    }
}
