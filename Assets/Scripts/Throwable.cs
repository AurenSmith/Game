using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {
    public Rigidbody2D rb;
    public BoxCollider2D bc;

    // Start is called before the first frame update
    void Start() {
        bc.isTrigger = true;
    }

    // Update is called once per frame
    void Update() {
        //recall
        if(Input.GetKeyDown(KeyCode.V)) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //becomes solid object once colliding with the map
        if(collision.tag == "Map") {
            bc.isTrigger = false;
        } else if(collision.tag == "Enemy") {
            Destroy(this.gameObject);
        }
    }
}
