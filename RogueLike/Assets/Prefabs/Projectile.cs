using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float launchForce = 25f;
    [SerializeField] private float destroyAfterSeconds = 3f;
    [SerializeField] private int damage = 1;
    private PlayerMotor playerMotor;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {   
        player = GameObject.Find("Player");
        //gameObject.tag = "Projectile";
        playerMotor = player.GetComponent<PlayerMotor>();
        rb.velocity = transform.forward * launchForce;

    }

    // Update is called once per frame
    void Update()
    {   
        Destroy(gameObject, destroyAfterSeconds);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            playerMotor.SetHealthCount(playerMotor.GetHealth() - damage);
			playerMotor.SetHealth(damage);
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Wall"){
            Destroy(gameObject);
            Debug.Log("Object was destroyed");
        }
        //Debug.Log(other.gameObject.layer);
        

        
    }

    
}
