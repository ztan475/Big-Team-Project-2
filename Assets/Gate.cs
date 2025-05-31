using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    // Helper script to swap player when they open door
    private Vector3 location;

    void Start()
    {
        location = transform.position;
    }

    public void Teleport(Transform playerPos)
    {
        playerPos.position = location;
    }
}
