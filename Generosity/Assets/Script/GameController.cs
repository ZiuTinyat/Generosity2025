using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum MoveState
{
    NoMove,
    RightMove,
    LeftMove,
}

public enum PickableItem
{
    None,
    Ball,
    Harness,
}
