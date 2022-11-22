using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Projectile") {
            Destroy(other.gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Projectile") {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Projectile") {
            Destroy(other.gameObject);
        }
    }
    /*
    private void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Wall") {
            Destroy(other.gameObject);
        }
    }*/
}
