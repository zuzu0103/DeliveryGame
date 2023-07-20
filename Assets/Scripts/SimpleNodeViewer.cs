using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNodeViewer : MonoBehaviour
{
    
    public bool visible;

    void OnDrawGizmos() {
        if (visible)
        {
            foreach (Transform child in transform)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(child.transform.position, Vector3.one * 0.5f);
            }
        }
    }
}
