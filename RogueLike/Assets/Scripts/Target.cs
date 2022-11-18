using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static float health;
    public float inputHealth;
    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
        Target.health = inputHealth;
    }

    public void TakeDamage (float amount){
        health -= amount;
        if(health <=0) {
            Die();
        }
    }

    void Die(){
        anim.SetTrigger("Die");
        Destroy(gameObject, 4f);
    }

    public static float GetHealth(){
        return health;
    }
}
