using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euphonium : MonoBehaviour
{
    private Animator animator;
    private static readonly int Psiton1Main = Animator.StringToHash("Psiton1_Main");
    private static readonly int Psiton2Main = Animator.StringToHash("Psiton2_Main");
    private static readonly int Psiton3Main = Animator.StringToHash("Psiton3_Main");

    /// <summary>
    /// 0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    private bool[] _keyDown = new bool[3];
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

   

    // Update is called once per frame
    void Update()
    {
        _keyDown[0] = Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.LeftArrow);
        _keyDown[1] = Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.DownArrow);
        _keyDown[2] = Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow);
        
        
        
        animator.SetBool(Psiton1Main,_keyDown[0]);
        animator.SetBool(Psiton2Main,_keyDown[1]);
        animator.SetBool(Psiton3Main,_keyDown[2]);
    }

  
}
