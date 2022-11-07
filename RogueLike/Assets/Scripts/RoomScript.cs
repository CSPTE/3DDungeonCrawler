using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public int numberOfGems;
    public GameObject wall;
    public int FloorID;
    private int gemsCollected;
    public GameObject entrance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getNumberOfGems(){
        return numberOfGems;
    }

    public void DestroyBlocker(){
        wall.SetActive(false);
    }
}
