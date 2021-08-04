using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbusMeasures : MonoBehaviour
{
    private enum Direction
    {
        left = 90,
        ahead = 0,
        right = -90,
        behind = 180
    }

    private Dictionary<GameObject, (float Distance, Direction Direction, bool Visibility)> ObjectsInNimbus = 
        new Dictionary<GameObject, (float Distance, Direction Direction, bool Visibility)>();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NotImplemented();
    }

    private void NotImplemented()
    {
        throw new NotImplementedException();
    }
}
