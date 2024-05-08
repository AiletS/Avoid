using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSource : MonoBehaviour
{
    public static PatternSource instance;
    public GameObject[] ps;

    private void Awake()
    {
        instance = this;
    }
}
