using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public Vector3 facingDir
    {
        get
        {
            var dir = transform.forward;
            dir.y = 0;
            return dir.normalized;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        DebugExtension.DebugArrow(transform.position, facingDir, Color.green);
    }
}