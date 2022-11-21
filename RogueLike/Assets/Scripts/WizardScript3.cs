using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardScript3 : MonoBehaviour
{
    [Header ("Enemy settings:")]
    [SerializeField] public Transform playerToFollow;
    [SerializeField] public float minDistance;
    [SerializeField] public float shootDistance;
    [SerializeField] public Target target;
    [SerializeField] public PlayerMotor playerMotor;
    [SerializeField] public Transform projectileSpawnpoint;
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public GameObject deathParticles;
    [SerializeField] public Transform  deathParticlePosition;
    [SerializeField] public float fireRate;

    private float distance;
    private float countBetweenFire = 1f;
    private Animation animation;


    
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        animation = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, playerToFollow.position);

        if(target.GetHealth() > 0 && distance < minDistance){
            ProcessMoveWizard();
        } else {
            if(target.GetHealth() > 0 ){
                ProcessIdleMovement();
            }
        }
        
    }

    void ProcessMoveWizard(){
        if(distance > shootDistance) {
            transform.LookAt(playerToFollow);
            nav.SetDestination(playerToFollow.position);
        } else {
            Shoot();
        }
    }

    void ProcessIdleMovement(){
        //write the idleMovement here
    }

    void Shoot(){
        transform.LookAt(playerToFollow);
        if(countBetweenFire <= 0f){
            StartCoroutine(ExampleCoroutine());
            animation["attack_short_001"].speed = 2f;
            animation.Play("attack_short_001");
            countBetweenFire = 1f/ fireRate;
        }

        countBetweenFire -= Time.deltaTime;
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(projectilePrefab, projectileSpawnpoint.position, transform.rotation);
    }

    public void Death(){
        nav.enabled = false;
        GameObject temp = Instantiate(deathParticles, deathParticlePosition.position, Quaternion.identity);
        Destroy(temp, 7f);
        animation.Play("dead");
        
    }

    public void Hit(){
        animation.Play("damage_001");
    }
}
