using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonScript : MonoBehaviour
{
    [Header ("Dragon settings:")]
    [SerializeField] public int damage;
    [SerializeField] public Transform playerToFollow;
    [SerializeField] public Target target;
    [SerializeField] public PlayerMotor playerMotor;
    [SerializeField] public float minDistance;
    [SerializeField] public Transform projectileSpawnpoint;
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public float waitBetweenFlames = 5f;
    
    private bool playFlame = true;
    private Animation animation;
    private bool shouldSpawnFlame = true;
    private float distance;
    private NavMeshAgent nav;
    private bool isFirstAttack;
    private bool takeDamageIfInside;
    private bool shouldTakeDamage;
    private bool playTakeOff = true;
    //private Random random;
    // Start is called before the first frame update
    void Start()
    {
        animation = gameObject.GetComponent<Animation>();
        nav = gameObject.GetComponent<NavMeshAgent>();
        animation.Play("Scream");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, playerToFollow.position);
        
        if( distance < minDistance && target.GetHealth() > 0f) {
            ProcessDragonMove();
        } else {
            if(target.GetHealth() > 0f){
                ProcessDragonIdle(); //Idle movement is working good
            }
        }
        
    }


    void ProcessDragonMove(){
        transform.LookAt(playerToFollow);
        if(distance > 15f && playFlame){
            nav.isStopped = true;
            playFlame = false;
            Flame();
            StartCoroutine(WaitForFlamePlay(waitBetweenFlames));
        }
        if(distance > 7f){
            nav.isStopped = false;
            if(!animation.IsPlaying("Run")){
                animation.Play("Run");
            }
            nav.SetDestination(playerToFollow.position);
            shouldTakeDamage = false;

        } else {
            HandleDragonAttack();
        }
        
    }


    void ProcessDragonIdle(){
        DecideWhichAnimationToPlay(Random.Range(0, 5));
        //DecideWhichAnimationToPlay(4);
    }

    bool CheckIfDragonIdleAnimationNotPlaying(){
        return (!animation.IsPlaying("Idle01") && !animation.IsPlaying("Scream") && !animation.IsPlaying("Idle02") && !animation.IsPlaying("Sleep") 
        && !animation.IsPlaying("Take off") && !animation.IsPlaying("Land"));
    }

    bool CheckIfAttackAnimationIsNotPlaying(){
        return (!animation.IsPlaying("Basic Attack") && !animation.IsPlaying("Claw Attack") && !animation.IsPlaying("Flame Attack")
         && !animation.IsPlaying("Get Hit"));
    }

    void DecideWhichAnimationToPlay(float temp){
        if(CheckIfDragonIdleAnimationNotPlaying()){
            switch(temp){
                case 0:
                    animation.Play("Idle01");
                    break;
                case 1:
                    animation.Play("Idle02");
                    break;
                case 2:
                    animation.Play("Scream");
                    break;
                case 3:
                    animation.Play("Sleep");
                    break;
                case 4:
                    TakeOffAndLand();
                    break;
            }
        }
    }

    void TakeOffAndLand(){
        animation.Play("Take Off");
        animation.PlayQueued("Land", QueueMode.CompleteOthers);
    }

    void HandleDragonAttack(){
        takeDamageIfInside = false;
        isFirstAttack = false;
        shouldTakeDamage = false;
        if(CheckIfAttackAnimationIsNotPlaying()){
            takeDamageIfInside = true;
            isFirstAttack = true;
            shouldTakeDamage = true;
            animation.Play("Basic Attack");
        }

        
        // play a random attack if not the flame one that check with colliders
    }

    private void OnTriggerEnter(Collider other){
        if((other.gameObject.tag == "Player") && isFirstAttack && shouldTakeDamage){
            shouldTakeDamage = false;
            takeDamageIfInside = false;
            isFirstAttack = false;
            playerMotor.SetHealthCount(playerMotor.GetHealth() - damage);
			playerMotor.SetHealth(damage);
            Debug.Log("There was a collision");
        }
    }

    private void OnTriggerStay(Collider other){
        if((other.gameObject.tag == "Player") && isFirstAttack && takeDamageIfInside && shouldTakeDamage){
            shouldTakeDamage = false;
            isFirstAttack = false;
            takeDamageIfInside = false;
            playerMotor.SetHealthCount(playerMotor.GetHealth() - damage);
			playerMotor.SetHealth(damage);
            Debug.Log("There was a collision");
        }
    }

    public void Death(){
        animation.Play("Die");

    }

    void Flame(){
        transform.LookAt(playerToFollow);
        animation.Play("Flame Attack");

        for(float i =-1; i < 1;){
            if(shouldSpawnFlame){
                Vector3 positionToSpawn = new Vector3(projectileSpawnpoint.position.x + i, projectileSpawnpoint.position.y, projectileSpawnpoint.position.z);
                Instantiate(projectilePrefab, positionToSpawn, transform.rotation);
                StartCoroutine(WaitForFlame(0.1f));
                i = i + 0.05f;
            }
        }

        
    }

    IEnumerator WaitForFlame(float delay){
        yield return new WaitForSeconds(delay);
        shouldSpawnFlame = true;
    }

    IEnumerator WaitForFlamePlay(float delay){
        yield return new WaitForSeconds(delay);
        playFlame = true;
    }

}
