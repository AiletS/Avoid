using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UIManager.instance.get_Damage(-PlayerMove.instance.heal_damage);
            Destroy(this.gameObject);
        }
    }
}
