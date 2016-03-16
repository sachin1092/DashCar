using UnityEngine;
using System.Collections;

public class GasPumpScript : MonoBehaviour
{
    public float objectSpeed = -0.1f;

    void Update()
    {
        transform.Translate(0, 0, objectSpeed);
    }
}