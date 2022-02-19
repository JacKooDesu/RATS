using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JacDev.Fix
{
    public class SortOrderSetter : MonoBehaviour
    {
        public SortOrderSetter()
        {
        }

        void Update()
        {
            if (PlayerMovement.player.position.y > transform.position.y)
                GetComponent<SpriteRenderer>().sortingOrder = PlayerMovement.player.GetComponent<SpriteRenderer>().sortingOrder + 1;
            else
                GetComponent<SpriteRenderer>().sortingOrder = PlayerMovement.player.GetComponent<SpriteRenderer>().sortingOrder - 1;

        }
    }
}