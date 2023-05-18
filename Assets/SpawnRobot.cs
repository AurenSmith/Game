using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRobot : MonoBehaviour {
    public Rigidbody2D rb;
    public HiddenRobot hr;
    public BoxCollider2D hrCollider;

    // Start is called before the first frame update
    void Start() {
        hr.enabled = false;
        hrCollider.enabled = false;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "Player") {
            hr.enabled = true;
            hrCollider.enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.freezeRotation = true;
        }
    }
}
