using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemy : MonoBehaviour {
    //References
    public Rigidbody2D rb;
    public Animator anim;
    public Transform player;
    public GameObject weapon;

    public bool awake = false;
    public bool inside = false;

    //floats
    public float attackSpeed = 2f;
    public float range = 2.5f;
    private float speed = 2f;
    public float pos;

    //ints
    public int health = 200;

    //booleans
    public bool attacking = false;
    public bool canAttack = true;
    public bool wasInside = false;
    private bool cycle = true;
    public bool nearPlayer = false;

    //private bool damage = false;
    public bool left = false;
    public bool right = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //can attack and cycle has looped
        if(canAttack && cycle) {
            //start timer
            //StartCoroutine(TimeAttack());
        }

        //check how close the player is
        if(attacking == false) {
            //left = false;
            //right = false;
            //Vector3 offset = new Vector3(0, 1, 0);

            //check radius
            pos = player.position.x - transform.position.x;
            if(pos <= range && pos >= -range) {
                //player is within range
                if(awake) {
                    //attack
                    nearPlayer = true;
                    if(canAttack) {
                        StartCoroutine(Attack());
                    }
                } else {
                    anim.Play("BoxOpen");
                    //offset = new Vector3(0, 3, 0);
                    StartCoroutine(WakeUp());
                }
                
            } else {
                nearPlayer = false;
            }
        }

        //calculate direction
        float dir = player.position.x - transform.position.x;
        int direction = 0;
        Vector3[] face = { new Vector3(0, 0, 0), new Vector3(0, 180, 0) };

        if(dir > 0) {
            //right
            direction = 0;

            //move
            if(attacking == false && nearPlayer == false) {
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

            //move
            if(attacking == false && nearPlayer == false) {
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

    IEnumerator Attack() {
        attacking = true;
        canAttack = false;

        int x = Random.Range(1, 10);
        if(x == 5) {
            anim.Play("BoxFlash");
            yield return new WaitForSeconds(1f);
            anim.Play("BoxSlam");
            yield return new WaitForSeconds(1.20f);
            attacking = false;
            yield return new WaitForSeconds(2f);
            canAttack = true;
        } else {

        }        
    }

    IEnumerator WakeUp() {
        yield return new WaitForSeconds(1f);
        awake = true;
    }
}
