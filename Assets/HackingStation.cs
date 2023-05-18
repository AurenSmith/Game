using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingStation : MonoBehaviour {
    public SpriteRenderer sr;
    private int health = 2;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "Weapon") {
            health--;
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit() {
        sr.color = new Color(0.5f, 0.75f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        sr.color = new Color(0.25f, 0.75f, 0.25f);
        
        if(health == 0) {
            //give upgrade
            this.gameObject.SetActive(false);
        }
    }
}
