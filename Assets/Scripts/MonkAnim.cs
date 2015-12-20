using UnityEngine;
using System.Collections;

public class MonkAnim : MonoBehaviour
{
    //public GameObject monk;
    public Transform monk;
    Vector3 iPos;

    void Start()
    {
        iPos = monk.position;
    }

    void Update()
    {
        monk.position = new Vector3(iPos.x, iPos.y+3*Mathf.Sin(Time.realtimeSinceStartup), iPos.z);
    }
}