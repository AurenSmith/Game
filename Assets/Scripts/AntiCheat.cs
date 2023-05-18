using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCheat : MonoBehaviour {
    //References
    public Rigidbody2D rb;
    public Transform player;
    public Animator anim;
    public SpriteRenderer sr;

    //floats
    private float stagger = 1f;
    private float range = 3f;
    private float cooldown = 2f;
    private float speed = 4f;

    //ints
    public int health = 100;
    private int offset = 1;

    //booleans
    public bool attacking = false;
    public bool canAttack = true;
    public bool wasInside = false;
    private bool cycle = true;

    private bool damage = false;
    public bool left = false;
    public bool right = false;
    public bool dead = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //can attack and cycle has looped
        if(canAttack && cycle) {
            //start timer
            StartCoroutine(TimeAttack());
        }

        //check how close the player is
        if(attacking == false) {
            left = false;
            right = false;

            //check left side
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity);
            if(hit.collider != null) {
                float distance = Mathf.Abs(hit.point.x - transform.position.x);
                if(hit.collider.name == "Player" && distance <= range) { //player is within attacking distance
                    //attack
                    left = true;
                    if(canAttack) {
                        StartCoroutine(Attack());
                    }
                }
            }
            //check right side
            hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity);
            if(hit.collider != null) {
                float distance = Mathf.Abs(hit.point.x - transform.position.x);
                if(hit.collider.name == "Player" && distance <= range) { //player is within attacking distance
                    //attack
                    right = true;
                    if(canAttack) {
                        StartCoroutine(Attack());
                    }
                }
            }

            if(left == false && right == false) {
                //can't find player
                if(canAttack && wasInside == false) {
                    StartCoroutine(AirAttack());
                }
            } else {
                //the player was inside range
                wasInside = true;
            }
        }

        //calculate direction
        float dir = player.position.x - transform.position.x;
        int direction = 0;
        Vector3[] face = { new Vector3(0, 0, 0), new Vector3(0, 180, 0) };

        if(dir > 0) {
            //right
            direction = 0;
            offset = 1;

            //move
            if(attacking == false && right == false) {
                if(rb.velocity.x < speed) {
                    rb.AddForce(Vector2.right * 2);
                    if(rb.velocity.x > 0) {
                        rb.AddForce(Vector2.right * rb.velocity.x);
                    }
                }
            }
        } else if(dir < 0) {
            //left
            direction = 1;
            offset = -1;

            //move
            if(attacking == false && left == false) {
                if(rb.velocity.x > -speed) {
                    rb.AddForce(Vector2.left * 2);
                    if(rb.velocity.x < 0) {
                        rb.AddForce(Vector2.left * -rb.velocity.x);
                    }
                }
            }
        }

        if(attacking == false) {
            transform.eulerAngles = face[direction];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "Weapon" && damage == false) {
            //take damage
            StartCoroutine(TakeDamage(10));
        } else if(collision.tag == "Throwable" && damage == false) {
            //take hammer damage
            StartCoroutine(TakeDamage(50));
        }
    }

    IEnumerator TakeDamage(int strength) {
        //damage caluculator
        damage = true;

        if(health <= strength) { //health is less than damage delt
            //die
            dead = true;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(0.2f, 0.2f, 0.2f);

            //kill anti-cheat
            gameObject.SetActive(false);
        } else {
            //damaged tick
            sr.color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(0.2f, 0.2f, 0.2f);

            health -= strength;
        }

        yield return new WaitForSeconds(stagger);
        damage = false;
    }

    IEnumerator TimeAttack() {
        canAttack = false;
        cycle = false;
        yield return new WaitForSeconds(0.5f);
        wasInside = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    IEnumerator AirAttack() {
        canAttack = false;

        sr.color = new Color(0.2f, 0.75f, 0.75f);
        yield return new WaitForSeconds(0.1f);
        sr.color = new Color(0.2f, 0.2f, 0.2f);
        yield return new WaitForSeconds(0.1f);
        sr.enabled = false;
        transform.position = new Vector2(player.position.x - (range * offset), transform.position.y + 7);
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(0.1f);
        sr.enabled = true;
        yield return new WaitForSeconds(1f);
        sr.enabled = false;
        transform.position = new Vector2(transform.position.x, player.position.y + 0.5f);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;
        yield return new WaitForSeconds(0.1f);
        sr.enabled = true;

        StartCoroutine(Attack());
    }

    IEnumerator Attack() {
        attacking = true;
        canAttack = false;

        //charge up
        sr.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        sr.color = new Color(0.2f, 0.2f, 0.2f);
        yield return new WaitForSeconds(1f);

        anim.Play("CheatAttack");

        yield return new WaitForSeconds(0.15f);
        attacking = false;

        yield return new WaitForSeconds(cooldown);
        cycle = true;
        canAttack = true;
    }
}
