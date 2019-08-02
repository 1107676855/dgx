using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum EMoveState
    {
        WALKING,
        RUNNING
    }
    public enum EWeaponState
    {
        IDLE,
        FIREING
        //AIMING,
        //AIMFIREING
    }
    public EMoveState MoveState;
    public EWeaponState WeaponState;

    private void Update()
    {
        SetMoveState();
        SetWeaponState();


    }
    void SetWeaponState()
    {
        WeaponState = EWeaponState.IDLE;


    }
    void SetMoveState()
    {
        MoveState = EMoveState.RUNNING;

    }
}
