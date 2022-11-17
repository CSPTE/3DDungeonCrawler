using UnityEngine;
using System.Collections;

public class Centaur_ctrl : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private int battle_state = 0;
	public float speed = 6.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
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

	}
	
	void Update () 
	{		
		//Hello Sasi. Here is a list of all actions with descriptions for ease of use. Good luck my dear friend! :)))
		//STATES
		stillState();
		battleState();

		//MOVEMENT
		moveForward();
		moveBackward();

		//ATTACKS
		attack1();
		attack2();
		attack3();

		//DEFENCE TOGGLE
		defenceStart();
		defenceEnd();

		//IDK what the rest of this code does so I left it in to not ruin it
		//-------------------------------------------------------------------TURNS
		var vert_modul = Mathf.Abs(Input.GetAxis("Vertical"));
		Debug.Log(vert_modul);
		
		if ((Input.GetAxis ("Horizontal") > 0.1f)&&(vert_modul > 0.3f)) 
		{
			anim.SetLayerWeight(1,1f);
			anim.SetBool ("turn_right", true);
		} else if (vert_modul > 0.3f)
		{
			anim.SetBool ("turn_right", false);
			//anim.SetLayerWeight(1,0f);
		}
		
		if ((Input.GetAxis ("Horizontal") < -0.1f)&&(vert_modul > 0.3f)) 
		{
			anim.SetLayerWeight(1,1f);
			anim.SetBool ("turn_left", true);
		} else if (vert_modul > 0.3f)
		{
			anim.SetBool ("turn_left", false);
			//anim.SetLayerWeight(1,0f);
		}
		
		if (controller.isGrounded) 
		{
			moveDirection=transform.forward * Input.GetAxis ("Vertical") * speed * runSpeed;
			//if (Mathf.Abs(Input.GetAxis ("Vertical")) > 0.2f)
			if (vert_modul > 0.2f)
				{
					float turn = Input.GetAxis("Horizontal");
					transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);		
				}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
		//----------------------------------------------------------------------------------------
	}

	void stillState(){
		if (Input.GetKey ("1")){ 	
			anim.SetInteger ("battle", 0);
			battle_state = 0;
			runSpeed = 1;
		}
	}
	void battleState(){
		if (Input.GetKey ("2")){
			anim.SetInteger ("battle", 1);
			anim.SetBool ("armed", true);
			armed=true;
			battle_state = 1;
			runSpeed = r_sp;
		}
	}
	void moveForward(){
		if (Input.GetKey ("up")) {
			anim.SetInteger ("moving", 1);
		} else {
			anim.SetInteger ("moving", 0);
		}
	}
	void moveBackward(){
		if (Input.GetKey ("down")) {
			anim.SetInteger ("moving", 12);
			runSpeed = 1;
		}
		if (Input.GetKeyUp ("down")) {
			if (battle_state == 0) runSpeed = 1;
			else if (battle_state >0) runSpeed = r_sp;
		}
	}
	void attack1(){
		if (Input.GetMouseButtonDown (0)) {
			anim.SetInteger ("moving", 2);
		}
	}
	void attack2(){
		if (Input.GetMouseButtonDown (1)) {
			anim.SetInteger ("moving", 3);
		}
	}
	void attack3(){
		if (Input.GetMouseButtonDown (2)) {
			anim.SetInteger ("moving", 6);
		}
	}
	void defenceStart(){
		if (Input.GetKeyDown("p")) {
			anim.SetInteger("moving", 11);
		}
	}
	void defenceEnd(){
		if (Input.GetKeyUp("p")) {
			anim.SetInteger("moving", 12);
		}
	}
}



