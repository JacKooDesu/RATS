using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResChangeScene : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 0.1)
        {
            Application.LoadLevel("FactoryFix");
        }
    }
}
