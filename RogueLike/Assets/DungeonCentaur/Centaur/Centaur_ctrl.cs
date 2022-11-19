using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Centaur_ctrl : MonoBehaviour {
	
	[Header ("Player settings")]
	[SerializeField] private Transform playerToFollow;
	[SerializeField] private float minDistance;
	public PlayerMotor playerMotor;

	[Header ("Enemy settings:")]
	public Target target;

	private float startTime;
	private float distance;
	private bool shouldTakeDamage = true;
	private Animation animation;
	private NavMeshAgent nav;
	

	void Start () 
	{						
		animation = gameObject.GetComponent<Animation>();
		nav = gameObject.GetComponent<NavMeshAgent>();
		animation.Play("Centaur_rig_walk2");
	}
	

	void Update () 
	{		
		//calculate distance
		distance = Vector3.Distance(transform.position, playerToFollow.position);
		
		if(target.GetHealth() > 0f && distance < minDistance) {	
			ProcessMoveCentaur();
		} else {
			if(target.GetHealth() > 0f){
				ProcessIdleMovement();
			}
			
		}
	}


	void ProcessMoveCentaur(){
			if (playerToFollow == null) {
				return;
			}
			
			if((distance < minDistance) && (distance > 8f)) {
				//look at the player
				transform.LookAt(playerToFollow);

				//play the walking animation
				if(!animation.IsPlaying("Centaur_rig_walk2")) {
					animation.Play("Centaur_rig_walk2");
				}
				nav.SetDestination(playerToFollow.position);
				startTime = Time.time;
			} else {
				handleAttack();
			}

		}
	

	void handleAttack(){
		//look at the player when attacking
		transform.LookAt(playerToFollow);
		
		//check if we need to play the animation or not
		if(!animation.IsPlaying("Centaur_rig_attack3")){
			animation.Play("Centaur_rig_attack3");
			shouldTakeDamage = true;
			startTime = Time.time;
		}

		//check if we need to take damage
		if((startTime + 1.5f <= Time.time) && (Vector3.Distance(transform.position, playerToFollow.position) < 8f) && shouldTakeDamage){
			shouldTakeDamage = false;
			playerMotor.SetHealthCount(playerMotor.GetHealth() - 1);
			playerMotor.SetHealth(1);
		}

	}


	void ProcessIdleMovement(){
		//need to write this
		if(!animation.Play("Centaur_rig_idle1")) {
			animation.Play("Centaur_rig_idle1");
		}
		
	}


	public void Death(){
		//play the deat animation
		if(!animation.Play("Centaur_rig_death1")){
			animation.Play("Centaur_rig_death1");
		}
		
	}

}