using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour
{   
    [Header ("Settings:")]
    [SerializeField] public Transform endPoint;
    [SerializeField] public Transform startPoint;
    [SerializeField] public float speed = 50f;
    [SerializeField] public float damage = 1f;
    [SerializeField] public float spearRate = 1f;
    private bool shouldUseSpear = false;
    private bool isFirstAttack = true;
    private bool takeDamageIfInside;
    private bool spearRateControl = true;
    
    public AudioSource spearSound;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && spearRateControl){
            spearRateControl = false;
            takeDamageIfInside = true;
            HandleAnimation();
            StartCoroutine(WaitForSpear(spearRate));
            spearSound.Play();
        }

        
    }


    void HandleAnimation(){
        isFirstAttack = true;
        shouldUseSpear = true;
        transform.position = endPoint.position;
        StartCoroutine(WaitBeforeTransformingSpearBack(0.5f));
    }

    IEnumerator WaitBeforeTransformingSpearBack(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = startPoint.position;
        shouldUseSpear = false;
    }

    IEnumerator WaitForSpear(float delay){
        yield return new WaitForSeconds(delay);
        spearRateControl = true;
    }

    private void OnTriggerEnter(Collider other) {
        if((other.gameObject.tag == "Wizard" || other.gameObject.tag == "Centaur" || other.gameObject.tag == "Dragon") && shouldUseSpear && isFirstAttack){
            takeDamageIfInside = false;
            isFirstAttack = false;
            other.gameObject.GetComponent<Target>().TakeDamage(damage);
            //spearSound.Play();
        }
    }

    private void OnTriggerStay(Collider other){
         if((other.gameObject.tag == "Wizard" || other.gameObject.tag == "Centaur" || other.gameObject.tag == "Dragon") && shouldUseSpear && isFirstAttack && takeDamageIfInside){
            takeDamageIfInside = false;
            isFirstAttack = false;
            other.gameObject.GetComponent<Target>().TakeDamage(damage);
            //spearSound.Play();
        }
    }

}
