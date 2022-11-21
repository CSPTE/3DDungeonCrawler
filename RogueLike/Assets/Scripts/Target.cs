using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Target : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public PlayerMotor playerMotor;
    private Animator anim;
    private GameObject wizard;
    private WizardScript3 wizardScript;
    private Centaur_ctrl centaurScript;
    private DragonScript dragonScript;
    private float originalHealth;

    public Canvas youWinCanvas;


    void Start(){
        originalHealth = health;
        anim = GetComponent<Animator>();
        //health = inputHealth;
        if(gameObject.tag == "Wizard") wizardScript = gameObject.GetComponent<WizardScript3>();
        if(gameObject.tag == "Centaur") centaurScript = gameObject.GetComponent<Centaur_ctrl>();
        if(gameObject.tag == "Dragon") dragonScript = gameObject.GetComponent<DragonScript>();

        
    }

    public void TakeDamage (float amount){
        if(health > 0) {
            if(gameObject.tag == "Wizard"){
                wizardScript.Hit();
            }
            health -= amount;
            if(health <=0) {
                Die();
            }
        }
    }

    void Die(){
        if (gameObject.tag != "Dragon") playerMotor.HandleKillUpgrade();
        if(gameObject.tag == "Wizard"){
            wizardScript.Death();
            Destroy(gameObject, 7f);
        }

        if(gameObject.tag == "Centaur"){
            centaurScript.Death();
            Destroy(gameObject, 4f);
        }

        if(gameObject.tag == "Dragon"){
             dragonScript.Death();
             //TODO lead win panel
             loadGameOverPanel();
        }
        
    }

    public float GetHealth(){
        return health;
    }

    void loadGameOverPanel(){
        youWinCanvas.GetComponent<Canvas>().gameObject.SetActive(true);
    }

    void stopRenderingCanvas(){
        youWinCanvas.GetComponent<Canvas>().gameObject.SetActive(false);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        stopRenderingCanvas();
    }

    public float GetOriginalHealth(){
        return originalHealth;
    }
}
