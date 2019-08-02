using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{



    public Animator animator;
    public Vector3 temp;
    public int weapon_class=1;
    private int preWeapon;
    public bool isThrow=false;
    public bool isDie = false;
 


    // Start is called before the first frame update
    void Awake()
    {
        
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame


    public void Update()
    {
        if (isDie)
        {
            animator.SetBool("die", true);
  
        }
    }



    private void ThrowGenade()
    {
        animator.SetBool("throw", false);
        isThrow = false;
    }
    

    private void SwapWeapon(int w)
    {
        preWeapon = weapon_class;
        if (w==1)
        {
            weapon_class = 1;
        }
        else if (w==2)
        {
            weapon_class = 2;
        }
        else if (w == 3)
        {
            weapon_class = 3;
        }

        

        if (preWeapon != weapon_class)
        {
            animator.SetInteger("WeaponClass", weapon_class);
            preWeapon = weapon_class;
         
        }
    }

    public void AnimationControl(float h,float v,Vector2 m,int w,bool t)
    {
        if (weapon_class == 3)
        {

            if (t&&this.GetComponent<PlayerHp>().Genade>0)
            {

                animator.SetBool("throw", true);
                isThrow = true;
                Invoke("ThrowGenade", 0.5f);

            }
        }
        temp = new Vector3(transform.eulerAngles.x - m.y, transform.eulerAngles.y, transform.eulerAngles.z);
        SetAnimation(h, v, GetAngle());
        SwapWeapon(w);
    
    }

    public void SetAnimation(float h,float v,float angle)
    {
        animator.SetFloat("Horizontal", h);
        animator.SetFloat("Vertical", v);
        animator.SetFloat("AimAngle", angle);
    }
    public float GetAngle()
    {
    
        return CheckAngle(temp.x);
    }
    public float CheckAngle(float value)
    {
        float angle = value - 180;
        if (angle > 0)
            return angle - 180;
        return angle + 180;
    }
}
