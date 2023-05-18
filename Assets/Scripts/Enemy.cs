using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Transform player;

    public float heightCorrection = 0.5f;
    public float sideCorrection = 0.47f;

    public float speed = 4f;
    public float jump = 10f;
    public float sprint = 8f;
    public float minSprint = 1f;

    public float reach = 10f;

    public int health = 100;
    public int direction = -1;
    private int dUp = -1;

    public float knockback = 10f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(health <= 0) { //dead

        }

        float cPos = player.position.x - transform.position.x;

        if(cPos > 0) { //right
            direction = 1;
            if(rb.velocity.x < speed) {
                rb.AddForce(Vector2.right * 2);
                if(rb.velocity.x > 0) {
                    rb.AddForce(Vector2.right * rb.velocity.x);
                }
            }
        }
        if(cPos < 0) { //left
            direction = -1;
            if(rb.velocity.x > -speed) {
                rb.AddForce(Vector2.left * 2);
                if(rb.velocity.x < 0) {
                    rb.AddForce(Vector2.left * -rb.velocity.x);
                }
            }
        }
        if(direction != dUp) {
            if(rb.velocity.x > 1 || rb.velocity.x < -1) {
                rb.velocity -= new Vector2(rb.velocity.x / 2.5f, 0);
            }
        }
        dUp = direction;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.name == "Weapon") {
            health -= 20;
            rb.AddForce(new Vector2(-direction, 0.1f) * knockback);
            sr.color = Color.gray;
            StartCoroutine(Tick());
        }
    }

    IEnumerator Tick() {
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.black;
    }

    private void OnCollisionStay2D(Collision2D collision) { //height correction (going up stairs automatically)
        Vector3 bottom = new Vector3(0, 0.9f, 0);

        RaycastHit2D hitL = Physics2D.Raycast(transform.position - bottom, Vector2.left, Mathf.Infinity);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position - bottom, Vector2.right, Mathf.Infinity);

        if(hitL.collider != null && hitL.collider == collision.collider) {
            if(hitL.collider != null) {
                float distance = Mathf.Abs(hitL.point.x - transform.position.x);
                RaycastHit2D adjustment = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity);

                if(adjustment.collider != null) {
                    float distanceA = Mathf.Abs(adjustment.point.x - transform.position.x);

                    if(distanceA > distance && distanceA != 0) { //apply hight adjustment
                        transform.position += new Vector3(-sideCorrection, heightCorrection);
                    } else {
                        //Jump
                        if(rb.velocity.y == 0) {
                            rb.velocity += new Vector2(0, jump);
                        }
                    }
                } else {
                    transform.position += new Vector3(-sideCorrection, heightCorrection);
                }
            }
        } else if(hitR.collider != null && hitR.collider == collision.collider) {
            if(hitR.collider != null) {
                float distance = Mathf.Abs(hitR.point.x - transform.position.x);
                RaycastHit2D adjustment = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity);
                if(adjustment.collider != null) {
                    float distanceA = Mathf.Abs(adjustment.point.x - transform.position.x);

                    if(distanceA > distance && distanceA != 0) { //apply hight adjustment
                        transform.position += new Vector3(sideCorrection, heightCorrection);
                    } else {
                        //Jump
                        if(rb.velocity.y == 0) {
                            rb.velocity += new Vector2(0, jump);
                        }
                    }
                } else {
                    transform.position += new Vector3(sideCorrection, heightCorrection);
                }
            }
        }
    }
}
