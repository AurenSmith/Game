using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {
    public SpriteRenderer sr;
    public BoxCollider2D bc;
    public GameObject antiCheat;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "Player") {
            StartCoroutine(CloseGate());
        }
    }

    IEnumerator CloseGate() {
        yield return new WaitForSeconds(0.5f);
        bc.isTrigger = false;
        sr.enabled = true;

        yield return new WaitForEndOfFrame();
        sr.color = Color.black;

        yield return new WaitForSeconds(2f);
        antiCheat.SetActive(true);
    }
}
