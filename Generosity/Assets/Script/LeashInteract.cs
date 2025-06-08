using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeashInteract : MonoBehaviour, IInteractable
{
    public ApproachableUI ui;
    public string leashText;
    public string unleashText;

    private GameController gc => GameController.Instance;

    public void Activate() {
        ui.SetText(gc.dog.isLeashed ? unleashText : leashText);
        ui.ShowInstruction();
    }

    public void Deactivate() {
        ui.HideInstruction();
    }

    public InteractType GetInteractType() {
        return InteractType.Leash;
    }

    public bool IsInteractable() {
        return gc.master.wearingHarness;
    }

    public void Interact() {
        StartCoroutine(LeashCoroutine());
    }

    private IEnumerator LeashCoroutine() {
        gc.dog.HoldMove();
        ui.HideInstruction();
        yield return new WaitForSeconds(1f);
        gc.dog.UnholdMove();
        gc.dog.SwitchLeashState();
        ui.SetText(gc.dog.isLeashed ? unleashText : leashText);
        ui.ShowInstruction();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
