using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Target : MonoBehaviour
{
    public float health;
    //public float inputHealth = 20f;
    private Animator anim;
    private GameObject wizard;
    public WizardScript3 wizardScript;
    public Centaur_ctrl centaurScript;


    void Start(){
        anim = GetComponent<Animator>();
        //health = inputHealth;
        
        
    }

    public void TakeDamage (float amount){
        if(health > 0) {
            if(gameObject.tag == "Wizard"){
                wizardScript.Hit();
            }
            health -= amount;
            if(health <=0) {
                Die();
            }
        }
    }

    void Die(){
        if(gameObject.tag == "Wizard"){
            wizardScript.Death();
            Destroy(gameObject, 7f);
        }
        if(gameObject.tag == "Centaur"){
            centaurScript.Death();
            Destroy(gameObject, 4f);
        }
        
    }

    public float GetHealth(){
        return health;
    }
}
