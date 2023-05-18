using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpawner : MonoBehaviour {
    public GameObject enemy;

    // Start is called before the first frame update
    void Start() {
        enemy.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "Player") {
            //spawn healling enemy
            enemy.SetActive(true);
        }
    }
}
