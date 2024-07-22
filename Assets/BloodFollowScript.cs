using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFollowScript : MonoBehaviour
{
    public string pathName;
    public float timeInSeconds;

    private void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath (pathName),"easeType",iTween.EaseType.easeInOutSine,"time",timeInSeconds , "looptype",iTween.LoopType.loop));
        //this can be extended to start always in front of the character showing them the correct path to follow. 
        // in this case it will loop to add some abmient cells to the scene. 
    }
}
