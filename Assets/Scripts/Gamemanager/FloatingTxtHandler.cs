using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTxtHandler : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f);
        transform.localPosition += new Vector3(0, 0.5f, 0);
    }
}
