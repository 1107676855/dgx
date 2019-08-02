using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // public float speed = 1.0F;
    CharacterController player_controller;
    void Start()
    {

        player_controller = GetComponent<CharacterController>();

    }


    void Update()
    {

           }

    public void ControlPlayer(float h, float v, Vector2 MouseInput)
    {
        Move(h, v);
        LookAround(MouseInput);
    }

    private void Move(float h, float v)
    {

        Vector3 direction = transform.forward * v + transform.right * h;
        transform.LookAt(this.transform.forward + transform.position);
        player_controller.Move(direction * Time.deltaTime*5);


    }
    private void LookAround(Vector2 MouseInput)
    {
        transform.Rotate(Vector3.up * MouseInput.x);

    }

    private int FloatToInt(float f)
    {
        if (f < 0.1f && f > -0.1f)
        {
            return 0;
        }
        else if (f > 0.1f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
