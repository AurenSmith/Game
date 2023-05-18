using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideEnemy : MonoBehaviour {
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    private int direction = 1;
    private int health = 30;

    private float speed = 2f;

    private bool damage = false;
    public bool dead = false;

    public int spawnNo = 1;

    // Start is called before the first frame update
    void Start() {
        dead = false;
        damage = false;
        direction = 1;
        health = 30;
        sr.color = Color.black;
    }

    // Update is called once per frame
    void Update() {
        if(damage == false) {
            if(direction == 1) { //right
                if(rb.velocity.x < speed) {
                    rb.AddForce(Vector2.right * 2);
                    if(rb.velocity.x > 0) {
                        rb.AddForce(Vector2.right * rb.velocity.x);
                    }
                }
            }
            if(direction == -1) { //left
                if(rb.velocity.x > -speed) {
                    rb.AddForce(Vector2.left * 2);
                    if(rb.velocity.x < 0) {
                        rb.AddForce(Vector2.left * -rb.velocity.x);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "DirectionChange") {
            direction = direction / -1;
        }

        if(collision.name == "Weapon") {
            if(damage == false) {
                damage = true;
                health -= 20;
                if(health <= 0) { //dead
                    sr.color = Color.red;
                    dead = true;
                } else {
                    sr.color = Color.gray;
                }
                StartCoroutine(Tick());
            }
        }
    }

    IEnumerator Tick() {
        //take damage tick
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.black;

        //stop moving tick
        yield return new WaitForSeconds(0.5f);
        damage = false;
    }
}
