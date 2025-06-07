using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziu;

public class Item : MonoBehaviourRequire<Rigidbody2D>
{
    public PickableItem item;

    private Rigidbody2D rigidBody => _t;

    public void Picked() {
        rigidBody.isKinematic = true;
    }

    public void Dropoed() {
        rigidBody.isKinematic = false;
    }

    public void Used() {
        Destroy(gameObject);
    }
}
