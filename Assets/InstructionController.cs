using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionController : MonoBehaviour
{
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            HUDManager.instance.ShowCurrentHint(sprite);
            HUDManager.instance.ShowCurrentHint(sprite);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            HUDManager.instance.HideHint();
        }
    }
}
