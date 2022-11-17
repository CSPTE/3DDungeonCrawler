using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //message to be displayed to player when looking at something that you can interact with
    public string promptMessage;

    
    public void BaseInteract(){
        Interact();
    }
    // Start is called before the first frame update
    protected virtual void Interact(){

    }


}
