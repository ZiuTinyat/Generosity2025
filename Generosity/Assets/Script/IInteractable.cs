using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteractable();
    InteractType GetInteractType();

    void Activate();
    void Deactivate();
    void Interact();
    int ComparePriority(IInteractable other) {
        var otherType = other == null ? InteractType.None : other.GetInteractType();
        return GetInteractType() - otherType;
    }

}

public enum InteractType
{
    None = 0,
    Leash = 1,
    Pickup = 3,
    Drag = 5,
    Drop = 7,
    Use = 9,
}
