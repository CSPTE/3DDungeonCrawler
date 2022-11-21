using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{   
    [SerializeField] public Image image;
    [SerializeField] public Target target;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = target.GetHealth()/target.GetOriginalHealth();
    }
}
