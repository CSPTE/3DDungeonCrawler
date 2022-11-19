using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Centaur_ctrl : MonoBehaviour {
	
	[Header ("Player settings")]
	[SerializeField] private Transform playerToFollow;
	[SerializeField] private float minDistance;
	public PlayerMotor playerMotor;
	private Animator anim;
	private CharacterController controller;
	private int battle_state = 0;

	[Header ("Enemy settings:")]
	public float speed = 6.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	private float actualSpeed = 3.0f;
	private Vector3 moveDirection = Vector3.zero;
	public Target target;
	private float animationLength;
	private bool isFirstAttack = false;
	private bool isFirstWalk = true;
	private float startTime;
	private float distance;
	private bool shouldTakeDamage;
	private NavMeshAgent nav;
	private float y_position;
	
	private float w_sp = 0.0f;
	private float r_sp = 0.0f;
	private bool armed = true;
	
	void Start () 
	{						
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();
		w_sp = speed; //read walk speed
		r_sp = runSpeed; //read run speed
		battle_state = 0;
		runSpeed = 1;
		nav = GetComponent<NavMeshAgent>();
		y_position = transform.localPosition.y;
		//Debug.Log(Target.GetHealth());

	}
	
	void Update () 
	{		
		//Debug.Log(Target.GetHealth());
		if(target.GetHealth() > 0f) ProcessMoveCentaur();
		
	}

	void ProcessMoveCentaur(){
			if (playerToFollow == null) {
				return;
			}
			
			
			isFirstAttack = true;
			//calculate distance
			distance = Vector3.Distance(transform.position, playerToFollow.position);

			if((distance < minDistance) && (distance > 8f)) {
				//look at the player
				transform.LookAt(playerToFollow);
				anim.SetBool("isPlayerCloseEnough", false);
				anim.SetInteger("moving", 1);
				nav.SetDestination(playerToFollow.position);
				startTime = Time.time;
			} else {
				shouldTakeDamage = true;
				handleAttack();
			}

		}
	
	void handleAttack(){
		//look at the player when attacking
		transform.LookAt(playerToFollow);
		

		if(isFirstAttack) {
			anim.SetInteger("moving", 50);
			isFirstAttack = false;
		}


		if(startTime +2.2f <= Time.time){
			startTime = Time.time;	
			anim.SetBool("isPlayerCloseEnough", true);	
			shouldTakeDamage = true;
			
		}

		HandleDamage(shouldTakeDamage);


	}

	//handle the damage taken
	void HandleDamage(bool temp){
		var currentClip = anim.GetCurrentAnimatorStateInfo(0);

		if(temp && (currentClip.normalizedTime < 0.55) && (currentClip.normalizedTime > 0.52) && (Vector3.Distance(transform.position, playerToFollow.position) < 8f)){
			shouldTakeDamage = false;
			playerMotor.SetHealthCount(playerMotor.GetHealth() - 1);
			playerMotor.SetHealth(1);
		}
	}
}



