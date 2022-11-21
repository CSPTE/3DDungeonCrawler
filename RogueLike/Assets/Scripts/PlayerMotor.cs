using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [Header ("Player settings:")]
    [SerializeField] public GameObject shield;
    [SerializeField] public GameObject gun;
    [SerializeField] public GameObject spear;
    [SerializeField] public int currentHealth;
    [SerializeField] public Gun gunScript;
    [SerializeField] public SpearScript spearScript;
    //static int healthToHandle;
    private GameObject currentFloor;
    private int currentGemTarget;
    private int currentGemsCollected;
    private int newFloorPlace = 0;
    public bool isShieldActivated = false;
    public bool isGunActivated = false;
    public bool isSpearActivated = true;

    public AudioSource collectGemSound;
    public AudioSource floorDoorUnlockLock;
    public AudioSource step;
    public AudioSource jump;
    public AudioSource background;
    public AudioSource hurt;

    public TextMeshProUGUI GemCount;
    public TextMeshProUGUI HealthCount;
    

    private bool tutorial = true;
    public Canvas tutorial1;
    public Canvas tutorial2;
    public Canvas tutorial3;
    public Canvas tutorial4;
    private Button turnOffTutorialButton;
    private Button okButton;
    private int currentTutorial = 1;
    private float tempSpeed;

    public Canvas gameOver;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeigth, stepRayUpper.transform.position.z);
        UpdateCurrentRoom();
        background.Play();
        SetHealthCount(currentHealth);
        shield.SetActive(isShieldActivated);
        gun.SetActive(isGunActivated);
        spear.SetActive(isSpearActivated);
        tempSpeed = speed;
        //PlayerMotor.healthToHandle = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Q)) {
            isShieldActivated = !isShieldActivated;
            shield.SetActive(isShieldActivated);
            isGunActivated = false;
            gun.SetActive(isGunActivated);
            isSpearActivated = false;
            spear.SetActive(isSpearActivated);
        }

        if(Input.GetKeyDown(KeyCode.E)){
            gunScript.SetUsageFromOtherScript();
            isGunActivated = !isGunActivated;
            gun.SetActive(isGunActivated);
            isShieldActivated = false;
            shield.SetActive(isShieldActivated);
            isSpearActivated = false;
            spear.SetActive(isSpearActivated);
        }

        if(Input.GetKeyDown(KeyCode.C)){
            spearScript.SetUsageFromOtherScript();
            isGunActivated = false;
            isShieldActivated = false;
            isSpearActivated = !isSpearActivated;
            gun.SetActive(isGunActivated);
            shield.SetActive(isShieldActivated);
            spear.SetActive(isSpearActivated);
        }

        if(isShieldActivated) tempSpeed=0;
        else tempSpeed = speed;

        isGrounded = controller.isGrounded;
        stepClimb();
        if (currentHealth <= 0){
            loadGameOverPanel(gameOver);
        }
    }

    //receive input for InputManager.cs and use them to our character controller
    public void ProcessMove(Vector2 input){
        //step.Play();
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * tempSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void Jump(){
        if(isGrounded){
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            StartCoroutine(PlaySoundAfterDelay(jump, 1f));
        }
    }

   
    void stepClimb(){
        //program goes here but it probably never goes into the if statements need to check why
        RaycastHit hitLower;
        if(Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.5f)){
            //Debug.Log("Went through the first if statement");
            RaycastHit hitUpper;
            if(!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.8f)){
                rigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                //Debug.Log("Second if statement");
            }
        }
    }

    void UpdateCurrentRoom(){
        currentFloor = dungeon.transform.GetChild(1).GetChild(newFloorPlace).gameObject;
        currentGemsCollected = 0;
        currentGemTarget = currentFloor.GetComponent<RoomScript>().getNumberOfGems();
        newFloorPlace = newFloorPlace + 2;
        SetCountText(0, currentGemTarget);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Gem")){
            other.gameObject.SetActive(false);
            collectGemSound.Play();
            currentGemsCollected++;
            SetCountText(currentGemsCollected, currentGemTarget);
            if(currentGemsCollected == currentGemTarget){
                currentFloor.GetComponent<RoomScript>().DestroyBlocker();
                floorDoorUnlockLock.Play();
            }
        } 

        if(other.gameObject.CompareTag("TutorialWall")){
            if(currentTutorial == 2){
                loadNextTutorial(tutorial2);
            } else if (currentTutorial == 3){
                loadNextTutorial(tutorial3);
            } else if (currentTutorial == 4){
                loadNextTutorial(tutorial4);
            } else {
                loadNextTutorial(tutorial1);
            }    
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("LockFloor")){
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            floorDoorUnlockLock.Play();
            UpdateCurrentRoom();
        }
    }

    IEnumerator PlaySoundAfterDelay(AudioSource audioSource, float delay)
    {
        if (audioSource == null)
            yield break;
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }

    void SetCountText(int current, int total)
    {
        GemCount.text = current + "/" + total;
    }

    public int GetHealth(){
        return currentHealth;
    }


    public void SetHealth(int toRemove){
        if(!isShieldActivated){
            currentHealth -= toRemove;
            hurt.Play();
        } else {
            //TO-DO put some blocking sound here with the shield
        }
        
    }
    public void gainHealthFromKill(){
        currentHealth = currentHealth + 1;
        SetHealthCount(currentHealth);
    }
    public void SetHealthCount(int health){
        if(health >= 0) HealthCount.text = health.ToString();
        else HealthCount.text = "0";
    }

    void loadNextTutorial(Canvas can){
        if (tutorial == true){
            can.GetComponent<Canvas>().gameObject.SetActive(true);
        }
    }

    public void turnOffTutorial(){
        tutorial = false;
        stopRenderingCanvas(tutorial1);
    }

    public void continueTutorial(){
        stopRenderingCanvas(tutorial1);
        currentTutorial++;
    }

    public void turnOffTutorial2(){
        tutorial = false;
        stopRenderingCanvas(tutorial2);
    }

    public void continueTutorial2(){
        stopRenderingCanvas(tutorial2);
        currentTutorial++;
    }

    public void turnOffTutorial3(){
        tutorial = false;
        stopRenderingCanvas(tutorial3);
    }

    public void continueTutorial3(){
        stopRenderingCanvas(tutorial3);
        currentTutorial++;
    }

    public void turnOffTutorial4(){
        tutorial = false;
        stopRenderingCanvas(tutorial4);
    }

    public void continueTutorial4(){
        stopRenderingCanvas(tutorial4);
        currentTutorial++;
    }

    void stopRenderingCanvas(Canvas can){
        can.GetComponent<Canvas>().gameObject.SetActive(false);
    }

    void loadGameOverPanel(Canvas can){
        can.GetComponent<Canvas>().gameObject.SetActive(true);
    }

    public void RestartGame(Canvas can) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        stopRenderingCanvas(can);
    }

}
