using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dog : MonoBehaviour
{
    // Refs
    [SerializeField] private Master master;
    [SerializeField] private Transform container;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private Transform leash;
    [SerializeField] private Transform leashHead;
    [SerializeField] private Transform mouth;

    // Move
    public bool isLeashed;
    public float leashedSpeed;
    public float freeSpeed;
    public float acceleration;
    private MoveState moveState;

    // Pickup
    public Item holdingItem;
    public bool isHoldingItem => holdingItem;
    private Item itemInRange;


    public void SwitchLeashState() {
        isLeashed = !isLeashed;
        leash.gameObject.SetActive(isLeashed);
    }

    public void Pickup(Item item) {
        if (!isHoldingItem) {
            if (itemInRange) {
                item.transform.SetParent(mouth);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.Picked();
                holdingItem = item;
            }
        }
    }

    public void Drop() {
        if (isHoldingItem) {
            holdingItem.transform.SetParent(null);
            holdingItem.Dropoed();
            holdingItem = null;
        }
    }

    public void Use() {
        if (isHoldingItem) {
            holdingItem.transform.SetParent(null);
            holdingItem.Used();
            holdingItem = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlUpdate();
        MoveUpdate();
        LeashUpdate();
    }

    private void ControlUpdate() {
        // Move
        if (Input.GetKey(KeyCode.RightArrow)) {
            SetMoveState(MoveState.RightMove);
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            SetMoveState(MoveState.LeftMove);
        } else {
            SetMoveState(MoveState.NoMove);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            // TODO bark
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            // TODO use
            SwitchLeashState();
        }
    }

    private void MoveUpdate() {
        // Speed
        //Vector2 dir = transform.right;
        float target = isLeashed ? leashedSpeed : freeSpeed;
        float speed = rigidBody.velocity.x; //Vector2.Dot(rigidBody.velocity, dir);
        //Vector2 spareVel = rigidBody.velocity - speed * dir;
        float delta = acceleration * Time.deltaTime;
        
        switch (moveState) {
            case MoveState.NoMove:
                if (speed > delta) speed -= delta;
                else if (speed < -delta) speed += delta;
                else speed = 0;
                break;
            case MoveState.RightMove:
                speed = Mathf.Min(target, speed + delta);
                break;
            case MoveState.LeftMove:
                speed = Mathf.Max(-target, speed - delta);
                break;
        }
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y); //speed * dir + spareVel;

        // Jump TODO
        // Craw TODO
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

    private Vector3 leashVec => leashHead.localPosition; // assume no scale
    private void LeashUpdate() {
        if (!isLeashed) return;
        leash.transform.localScale = Vector3.one;
        leash.transform.localRotation = Quaternion.identity;
        Vector3 handVec = leash.InverseTransformPoint(master.handPoint.position);
        leash.localRotation = Quaternion.FromToRotation(leashVec, handVec);
        float r = Mathf.Sqrt(handVec.sqrMagnitude / leashVec.sqrMagnitude);
        leash.localScale = new Vector3(r, 1, 1);
    }
}
