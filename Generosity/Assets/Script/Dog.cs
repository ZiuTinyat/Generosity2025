using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Dog : MonoBehaviour
{
    // Refs
    [SerializeField] private Transform container;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Transform leash;
    [SerializeField] private Transform leashHead;
    [SerializeField] private Transform mouth;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Sprite bodySuit;
    [SerializeField] private GameObject woof;
    private GameController gc => GameController.Instance;
    private Master master => gc.master;

    // Move
    public bool isLeashed;
    public float leashedSpeed;
    public float freeSpeed;
    public float acceleration;
    public bool wearingHarness = false;
    private MoveState moveState = MoveState.NoMove;

    // Pickup
    public Item holdingItem;
    public bool canPickup => !isLeashed && !holdingItem;
    private Transform holdingItemParent;

    // Audio
    public List<AudioClip> barks;

    public void UseHarness() {
        if (wearingHarness) return;
        wearingHarness = true;
        body.sprite = bodySuit;
        SwitchLeashState();
    }

    public void SwitchLeashState() {
        isLeashed = !isLeashed;
        leash.gameObject.SetActive(isLeashed);
        UpdateInteractable();
        UpdateMoveAnim();
    }

    public void Pickup(Item item) {
        StartCoroutine(PickupCoroutine(item));
    }

    private IEnumerator PickupCoroutine(Item item) {
        Debug.Assert(!holdingItem);
        animator.SetTrigger("Pick");
        HoldMove();
        yield return new WaitForSeconds(0.25f);
        holdingItemParent = item.transform.parent;
        item.transform.SetParent(mouth);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.Picked();
        holdingItem = item;
        UnholdMove();
        UpdateInteractable();
    }

    public void Drop() {
        Debug.Assert(holdingItem);
        holdingItem.transform.SetParent(holdingItemParent);
        holdingItem.transform.localRotation = Quaternion.identity;
        holdingItem.transform.localScale = Vector3.one;
        holdingItem.Dropoed();
        holdingItem = null;
        UpdateInteractable();
    }

    public void Use() {
        Debug.Assert(holdingItem);
        RemoveInteractable(holdingItem);
        holdingItem.transform.SetParent(null);
        holdingItem.Used();
        holdingItem = null;
        UpdateInteractable();
    }

    public bool GetBarked() {
        return Input.GetKeyDown(KeyCode.Space);
    }

    #region Hold Move
    private int holdCounter = 0;
    private bool moveHeld => holdCounter > 0;
    public void HoldMove() {
        holdCounter++;
    }
    public void UnholdMove() {
        holdCounter--;
    }
    #endregion

    #region Interactables
    private HashSet<IInteractable> interactables = new();
    private IInteractable currentInteract = null;
    private void AddInteractable(IInteractable interactable) {
        if (interactables.Contains(interactable)) return;
        interactables.Add(interactable);
        if (!interactable.IsInteractable()) return;
        if (interactable.ComparePriority(currentInteract) >= 0) {
            AssignCurrentInteract(interactable);
        }
    }
    private void RemoveInteractable(IInteractable interactable) {
        if (!interactables.Contains(interactable)) return;
        interactables.Remove(interactable);
        if (currentInteract == interactable) {
            UpdateInteractable();
        }
    }
    private void UpdateInteractable() {
        IInteractable potentialInteract = null;
        foreach (var interactable in interactables) {
            if (!interactable.IsInteractable()) continue;
            if (interactable.ComparePriority(potentialInteract) >= 0) {
                potentialInteract = interactable;
            }
        }
        AssignCurrentInteract(potentialInteract);
    }
    private void AssignCurrentInteract(IInteractable interactable) {
        if (currentInteract == interactable) return;
        currentInteract?.Deactivate();
        currentInteract = interactable;
        currentInteract?.Activate();
    }
    #endregion

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

        // Interact
        if (Input.GetKeyDown(KeyCode.E)) {
            if (currentInteract != null) {
                currentInteract.Interact();
            }
        }

        // Bark
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!holdingItem) {
                audioSource.clip = barks[Random.Range(0, barks.Count)];
                audioSource.Play();
                //StartCoroutine(WoofCoroutine());
                animator.SetTrigger("Bark");
            }
        }
    }

    private void MoveUpdate() {
        // Speed
        if (moveHeld) {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        } else {
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
        }

        // Jump TODO
        // Craw TODO
    }

    private void SetMoveState(MoveState state) {
        if (state == moveState) return;
        moveState = state;
        switch (state) {
            case MoveState.NoMove:
                UpdateMoveAnim();
                break;
            case MoveState.RightMove:
                container.localScale = Vector3.one;
                UpdateMoveAnim();
                break;
            case MoveState.LeftMove:
                container.localScale = new Vector3(-1, 1, 1);
                UpdateMoveAnim();
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

    private IEnumerator WoofCoroutine(float time = 0.25f) {
        woof.SetActive(true);
        yield return new WaitForSeconds(time);
        woof.SetActive(false);
    }

    private void UpdateMoveAnim() {
        if (moveHeld || moveState == MoveState.NoMove) {
            animator.SetInteger("MoveState", 0);
        } else {
            animator.SetInteger("MoveState", isLeashed ? 1 : 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var interactable = collision.GetComponent<IInteractable>();
        if (interactable != null) {
            //Debug.Log($"Enter {collision.gameObject.name}");
            AddInteractable(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        var interactable = collision.GetComponent<IInteractable>();
        if (interactable != null) {
            //Debug.Log($"Exit {collision.gameObject.name}");
            RemoveInteractable(interactable);
        }
    }
}

public enum MoveState
{
    NoMove,
    RightMove,
    LeftMove,
}
