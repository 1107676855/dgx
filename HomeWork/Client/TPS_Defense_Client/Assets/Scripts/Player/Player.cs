using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerAnimation animator_Input;
    PlayerMove move_Input;
    PlayerShoot shoot_Input;
    ReWeapon weapon_Input;
    Camera tpscam;
    public GameObject Tank;

    private void Start()
    {
    
        animator_Input = GetComponent<PlayerAnimation>();
        move_Input = GetComponent<PlayerMove>();
        shoot_Input = GetComponent<PlayerShoot>();
        weapon_Input = GetComponent<ReWeapon>();
    }
    private void Awake()
    {
        tpscam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Tank = GameObject.FindGameObjectWithTag("Tank");
    }

    private void Update()
    {


        if (!tpscam.GetComponent<ThirdCamera>().isInTank)
        {
            transform.localScale = Vector3.one;
            move_Input.ControlPlayer(GameManager.Instance.InputController.Horizontal, GameManager.Instance.InputController.Vertical, GameManager.Instance.InputController.MouseInput);
            transform.parent = null;
            GetComponent<PlayerMove>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            animator_Input.AnimationControl(GameManager.Instance.InputController.Horizontal, GameManager.Instance.InputController.Vertical, GameManager.Instance.InputController.MouseInput,GameManager.Instance.InputController.InputWeapon, GameManager.Instance.InputController.Fire1);
            weapon_Input.IsOneOrMul(GameManager.Instance.InputController.Space);
            if (!weapon_Input.isOne)
            {
                shoot_Input.ShootControl(GameManager.Instance.InputController.Fire1);
            }
            else
            {
                shoot_Input.ShootControl(GameManager.Instance.InputController.Fire1_one);
            }

            weapon_Input.ReWeaponClass(GameManager.Instance.InputController.InputWeapon);
        }
        else
        {

            transform.position = Tank.transform.position-Tank.transform.forward*3;
            Debug.Log(transform.position);
            transform.localScale = Vector3.zero;
            GetComponent<PlayerMove>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
        }
    
 
    }

    private void OnDestroy()
    {
        Debug.Log("destroy");
    }
}
