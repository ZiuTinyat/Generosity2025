using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziu;

public class Item : MonoBehaviourRequire<Rigidbody2D>, IInteractable
{
    public PickableItem item;
    public ApproachableUI ui;
    public string instructionText;

    public bool isHeld;

    private Rigidbody2D rigidBody => _t;
    private GameController gc => GameController.Instance;

    public bool IsInteractable() {
        return isHeld || gc.dog.canPickup;
    }

    public InteractType GetInteractType() {
        return isHeld ? InteractType.Drop : InteractType.Pickup;
    }

    public void Picked() {
        rigidBody.isKinematic = true;
        isHeld = true;
        ui.HideInstruction(0);
    }

    public void Dropoed() {
        rigidBody.isKinematic = false;
        isHeld = false;
        ui.ShowInstruction();
    }

    public void Used() {
        Destroy(gameObject);
    }

    public void Activate() {
        if (!isHeld) ui.ShowInstruction();
    }

    public void Deactivate() {
        if (!isHeld) ui.HideInstruction();
    }

    public void Interact() {
        if (isHeld) gc.dog.Drop();
        else gc.dog.Pickup(this);
    }

    private void Start() {
        ui.SetText(instructionText);
    }

    /*private static readonly Dictionary<PickableItem, string> _itemName = new() {
        { PickableItem.None, "" },
        { PickableItem.Ball, "ball" },
        { PickableItem.Harness, "harness" },
    };*/
}

public enum PickableItem
{
    None,
    Ball,
    Harness,
}
