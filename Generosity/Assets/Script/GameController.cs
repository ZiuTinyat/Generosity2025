using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziu;

public class GameController : MonoBehaviourSingleton<GameController>
{
    public Master master;
    public Dog dog;

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
