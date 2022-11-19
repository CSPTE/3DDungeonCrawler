using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    public float health;
    public float inputHealth;
    private Animator anim;

    public int ID;
    public Canvas youWin;

    void Start(){
        anim = GetComponent<Animator>();
        health = inputHealth;
    }

    public void TakeDamage (float amount){
        health = health - amount;
        if(health <=0) {
            Die();
        }
    }

    void Die(){
        anim.SetTrigger("Die");
        if (ID == 3){
            loadGameOverPanel();
        }
        Destroy(gameObject, 4f);
    }

    public float GetHealth(){
        return health;
    }

    void stopRenderingCanvas(){
        youWin.GetComponent<Canvas>().gameObject.SetActive(false);
    }

    void loadGameOverPanel(){
        youWin.GetComponent<Canvas>().gameObject.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        stopRenderingCanvas();
    }
}