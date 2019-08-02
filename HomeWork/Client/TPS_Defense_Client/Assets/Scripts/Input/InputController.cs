using UnityEngine;

public class InputController : MonoBehaviour
{
       
 

    public float Vertical;
    public float Horizontal;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Fire2;
    public bool Fire1_one;
    public bool Fire2_one;
    public int InputWeapon=1;
    //public bool Reload;
    public bool GetInTank;
    //连发切换
    public bool Space;
    public bool Reload;
    public bool BuyBullet;


    

    private void Update()
    {
        
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("Fire2");
        Fire1_one = Input.GetButtonDown("Fire1");
        Fire2_one = Input.GetButtonDown("Fire2");
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InputWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InputWeapon = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InputWeapon = 3;
        }

        //Reload = Input.GetKey(KeyCode.R);
        GetInTank = Input.GetKeyDown(KeyCode.F);
        Space = Input.GetKeyDown(KeyCode.Space);
        Reload = Input.GetKeyDown(KeyCode.R);
        BuyBullet = Input.GetKeyDown(KeyCode.B);
   
    }
}
