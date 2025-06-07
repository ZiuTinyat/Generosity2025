using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    // Refs
    [SerializeField] private Dog dog;
    [SerializeField] private Transform container;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    public Transform handPoint;

    // Move
    public float maxSpeed;
    public float acceleration;
    public float followDistance;
    private MoveState moveState = MoveState.NoMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
    }

    private void MoveUpdate() {
        if (dog.isLeashed) {
            float dis = transform.position.x - dog.transform.position.x;
            if (dis < -followDistance) {
                SetMoveState(MoveState.RightMove);
            } else if (dis > followDistance) {
                SetMoveState(MoveState.LeftMove);
            } else {
                SetMoveState(MoveState.NoMove);
            }
        } else {
            SetMoveState(MoveState.NoMove);
        }
        float speed = rigidBody.velocity.x;
        float delta = acceleration * Time.deltaTime;
        switch (moveState) {
            case MoveState.NoMove:
                if (speed > delta) speed -= delta;
                else if (speed < -delta) speed += delta;
                else speed = 0;
                break;
            case MoveState.RightMove:
                speed = Mathf.Min(maxSpeed, speed + delta);
                break;
            case MoveState.LeftMove:
                speed = Mathf.Max(-maxSpeed, speed - delta);
                break;
        }
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
    }

    private void SetMoveState(MoveState state) {
        if (state == moveState) return;
        moveState = state;
        switch (state) {
            case MoveState.NoMove:
                // TODO animation
                break;
            case MoveState.RightMove:
                container.localScale = Vector3.one;
                // TODO animation
                break;
            case MoveState.LeftMove:
                container.localScale = new Vector3(-1, 1, 1);
                // TODO animation
                break;
        }
    }
}
