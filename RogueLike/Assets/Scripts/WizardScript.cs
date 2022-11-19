using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardScript : MonoBehaviour
{   
    [Header ("Enemy settings")]
	[SerializeField] private Transform playerToFollow;
	[SerializeField] private float minDistance;
    [SerializeField] private float shootDistance;
	[SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private float Damage;
    [SerializeField] private Target target;
    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Transform deatParticlePosition;
    
    private float countBetweenFire = 1f;
    private Animation anim;
    [SerializeField] private float FireRate = 2f;

    private NavMeshAgent nav;
    private float distance;


    void Start(){
        nav = GetComponent<NavMeshAgent>();
        
        anim = gameObject.GetComponent<Animation>();
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, playerToFollow.position);
        //transform.LookAt(playerToFollow);

        if((target.GetHealth() > 0) && (distance < minDistance)){
            //transform.localRotation = Quaternion.Euler(new Vector3(0,playerToFollow.localPosition.y,0));
            
            ProcessWizardMove();
        }
    }

    void ProcessWizardMove(){
        if((distance > 8f)) {
            //nav.enabled = true;
            //transform.LookAt(playerToFollow);
            //nav.SetDestination(playerToFollow.position);
        } else {
            //nav.isStopped = true;
            Debug.Log("Should stop");
        }
        //transform.LookAt(playerToFollow);
        if(target.GetHealth() > 0) {
            Shoot();
        }


    }

    void Move(){
        
    }

    void Shoot(){
        //transform.LookAt(playerToFollow);
        
        if(distance < shootDistance){
            //animation time 1.467
            if(countBetweenFire <= 0f){
                StartCoroutine(ExampleCoroutine());
                anim["attack_short_001"].speed = 2f;
                anim.Play("attack_short_001");
                countBetweenFire = 1f/ FireRate;
            }

            countBetweenFire -= Time.deltaTime;
            
        }
    }


     IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(projectilePrefab, projectileSpawnpoint.position, transform.rotation);
    }

    public void Death(){
        Instantiate(deathParticles, deatParticlePosition.position, transform.rotation);
        anim.Play("dead");
    }

    public void Hit(){
        anim.Play("damage_001");
    }
}
