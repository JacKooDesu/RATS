using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFOVfollow : MonoBehaviour
{
    public GameObject player; //Drag the "player" GO here in the Inspector    

    public void LateUpdate()
    {
        transform.position = player.transform.position;
   
    }
    // Start is called before the first frame update

}
