using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern4 : MonoBehaviour
{
    [SerializeField] float force = 0.5f;

    private void FixedUpdate()
    {
        PlayerMove.instance.transform.position
            = Vector3.MoveTowards(PlayerMove.instance.transform.position, new Vector3(0, 0, 0), force * Time.fixedDeltaTime);
    }
}
