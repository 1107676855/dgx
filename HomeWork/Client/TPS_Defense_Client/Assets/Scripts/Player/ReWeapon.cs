using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReWeapon : MonoBehaviour
{
    public enum WeaponClass
    {
        WEAPON_FAL,
        WEAPON_GLOCK,
        GRENADE
    }
    //是否单发
    public bool isOne = false;
    WeaponClass temp;
    public static int weaponNO;
    public GameObject weapon_fal;
    public GameObject weapon_glock;
    public GameObject grenade;
    private List<GameObject> childObjs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
    
      


    }

    public void IsOneOrMul(bool s)
    {
        
        if(s&&temp!=WeaponClass.GRENADE)
            isOne = !isOne;
    }

    public void ReWeaponClass(int n)
    {
        switch (n)
        {
            case 1:
                temp = WeaponClass.WEAPON_FAL;
                weaponNO = 1;
                break;
            case 2:
                temp = WeaponClass.WEAPON_GLOCK;
                weaponNO = 2;
                break;
            case 3:
                temp = WeaponClass.GRENADE;
                weaponNO = 3;
                break;
            default:
                break;
        }

        SwapWeapon(temp);
    } 

    private void SwapWeapon(WeaponClass w)
    {
        if (w == WeaponClass.WEAPON_FAL)
        {
            weapon_fal.GetComponent<Renderer>().enabled = true;
            weapon_glock.GetComponent<Renderer>().enabled = false;
            grenade.GetComponent<Renderer>().enabled = false;
        }
        else if (w == WeaponClass.WEAPON_GLOCK)
        {
            weapon_fal.GetComponent<Renderer>().enabled = false;
            weapon_glock.GetComponent<Renderer>().enabled = true;
            grenade.GetComponent<Renderer>().enabled = false;
        }
        else 
        {
            weapon_fal.GetComponent<Renderer>().enabled = false;
            weapon_glock.GetComponent<Renderer>().enabled = false;
            grenade.GetComponent<Renderer>().enabled = true;
        }
       
    }

}
