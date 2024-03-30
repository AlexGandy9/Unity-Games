using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Vector2 minPos;
    public Vector2 maxPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }
}
