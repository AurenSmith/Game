using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour {
    public RobotEnemy re;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.name == "Player") {
            re.GetComponent<RobotEnemy>().inside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.name == "Player") {
            re.GetComponent<RobotEnemy>().inside = false;
        }
    }
}
