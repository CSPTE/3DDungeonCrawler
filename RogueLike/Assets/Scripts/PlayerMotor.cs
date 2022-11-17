using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{   
    private CharacterController controller;
    private Vector3 playerVelocity;
    Rigidbody rigidBody;
    [Header ("Player movement change:")]
    [SerializeField] public float speed = 5f;
    //adding gravity
    private bool isGrounded;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] public float jumpHeight = 1.5f;

    [Header("Player to step over some things:")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeigth = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    public GameObject dungeon;
    private Animation swordAnimation;
    private Animator animator;
    private GameObject currentFloor;
    private int currentGemTarget;
    private int currentGemsCollected;
    private int newFloorPlace = 0;
    private float timestamp;
    private float attackTime;
    private float NTime;
    AnimatorStateInfo animStateInfo;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeigth, stepRayUpper.transform.position.z);
        UpdateCurrentRoom();
        //swordAnimation = gameObject.GetComponent<Animation>();
        /*animator = GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips){
            //Debug.Log("went here " + clip.name);
            if(clip.name == "RightHand@Attack01"){
                attackTime = clip.length;
            }
        }
        //animator.enabled = false;*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = controller.isGrounded;
        /*animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        NTime = animStateInfo.normalizedTime;
        if(Input.GetMouseButtonDown(0)){
            SwingSword();
        } else {
            if( NTime > 1.0f){
                animator.enabled = false;
                //timestamp = Time.time + 2f;
            }
        }*/

        stepClimb();
    }

    //receive input for InputManager.cs and use them to our character controller
    public void ProcessMove(Vector2 input){
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void SwingSword(){
        //animator.SetBool("swingSword", true);
        //need to add the part to actually calculate if it hit something
        //Debug.Log("Sword animator should have been called");
        //animator.Play("RightHand@Attack01");
        //animator.enabled = true;
        //animator.Play("Base Layer.Attack01");
        //swordAnimation.Play();
    }

    public void Jump(){
        if(isGrounded){
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }



   
    void stepClimb(){
        //program goes here but it probably never goes into the if statements need to check why
        RaycastHit hitLower;
        if(Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.5f)){
            RaycastHit hitUpper;
            if(!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.8f)){
                rigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.Log("Second if statement");
            }
        }
    }

    void UpdateCurrentRoom(){
        currentFloor = dungeon.transform.GetChild(1).GetChild(newFloorPlace).gameObject;
        currentGemsCollected = 0;
        currentGemTarget = currentFloor.GetComponent<RoomScript>().getNumberOfGems();
        newFloorPlace = newFloorPlace + 2;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Gem")){
            other.gameObject.SetActive(false);
            currentGemsCollected++;
            if(currentGemsCollected == currentGemTarget){
                currentFloor.GetComponent<RoomScript>().DestroyBlocker();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("LockFloor")){
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            UpdateCurrentRoom();
        }
    }

}
