using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInTank : MonoBehaviour
{
    Renderer[] PlayerRender;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRender = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
