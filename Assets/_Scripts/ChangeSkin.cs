using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    public AnimatorOverrideController blue;
    public AnimatorOverrideController gold;

    //Code that overrides the base animations with the animations for the 2 types of armor
    public void Blue(){

        GetComponent<Animator>().runtimeAnimatorController = blue as RuntimeAnimatorController;
    }
    public void Gold(){

        GetComponent<Animator>().runtimeAnimatorController = gold as RuntimeAnimatorController;
    }
}
