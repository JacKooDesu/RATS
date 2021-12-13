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

        void Update(){
      if(playermovement.player.position.y > transform.position.y)
          GetComponent<SpriteRenderer>().sortingOrder = playermovement.player.GetComponent<SpriteRenderer>().sortingOrder+1;
       else
          GetComponent<SpriteRenderer>().sortingOrder = playermovement.player.GetComponent<SpriteRenderer>().sortingOrder-1;
      
    }
}
}