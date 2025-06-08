using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterUseInteract : MonoBehaviour, IInteractable
{
    public ApproachableUI ui;
    public string useBallText;
    public string useHarnessText;

    private GameController gc => GameController.Instance;

    public void Activate() {
        ui.SetText(GetItemText(gc.dog.holdingItem.item));
        ui.ShowInstruction();
    }

    public void Deactivate() {
        ui.HideInstruction();
    }

    public InteractType GetInteractType() {
        return InteractType.Use;
    }

    public bool IsInteractable() {
        if (gc.dog.holdingItem == null) return false;
        switch (gc.dog.holdingItem.item) {
            case PickableItem.Ball:
                return true;
            case PickableItem.Harness:
                return true;
            default:
                return false;
        }
    }

    public void Interact() {
        Debug.Assert(gc.dog.holdingItem);
        switch (gc.dog.holdingItem.item) {
            case PickableItem.Ball:
                Debug.Log("Ball!");
                break;
            case PickableItem.Harness:
                Debug.Log("Use Harness");
                gc.master.UseHarness();
                break;
            default:
                break;
        }
        gc.dog.Use();
    }

    private string GetItemText(PickableItem item) {
        switch (item) {
            case PickableItem.Ball:
                return useBallText;
            case PickableItem.Harness:
                return useHarnessText;
            default:
                return " ";
        }
    }
}
