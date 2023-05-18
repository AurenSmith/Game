using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    //References
    public Rigidbody2D rb;
    public Animator anim;
    public BoxCollider2D bc;
    public BoxCollider2D playerCollider;
    public ParticleSystem ps;
    public Transform healthBar;
    public Transform ocBar;
    public SpriteRenderer sr;
    public ParticleSystem lightning;
    public Transform lightningPos;
    public GameObject throwable;
    public GameObject tc;
    public SpriteRenderer weaponSR;
    public AntiCheat ac;
    public GameObject inventory;

    GameObject hammerObj;

    //floats
    public float heightCorrection = 0.5f;
    public float sideCorrection = 0.47f;
    public float speed = 4f;
    public float jump = 12f;
    public float knockback = 3f;
    public float knockbackUp = 8f;
    public float sprint = 8f;
    public float minSprint = 1f;
    public float reach = 10f;
    public float overClock = 50f;
    public float rate = 0.75f;
    public float strikeSpeed = 1f;

    //ints
    public int health = 100;
    public int direction = -1;
    public int hsHealth = 2;

    //booleans
    public bool grounded = true;
    public bool iFrame = false;
    public bool hammerUse = true;
    public bool heavyAttack = false;

    private bool extraOC = false;
    private bool ocGiven = false;
    private bool checkAttack = false;
    private bool isAttacking = false;
    private bool canAttack = true;
    private bool hammerIsUnlocked = false;
    private bool inHS = false;
    public bool obtainDash = false;
    private bool canDash = true;
    private bool atackable = true;
    private bool inventoryOpen = false;

    // Start is called before the first frame update
    void Start() {
        ps.Stop();
        bc.enabled = false;
        inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if(hammerIsUnlocked == false) {
            if(ac.dead) {
                hammerIsUnlocked = true;
            }
        }

        healthBar.localScale = new Vector2(health / 10, 1);
        ocBar.localScale = new Vector2(overClock / 10, 1);

        if(grounded == true) {
            extraOC = false;
        }

        if(extraOC == true) {
            ps.Play();
        } else {
            ps.Stop();
        }

        if(extraOC == true && ocGiven == false) {
            float ocUp = rb.velocity.y;
            ocUp -= knockback;
            if(ocUp > 0) {
                if(overClock <= 85) {
                    overClock += 15;
                }
                ocGiven = true;
            }
        }

        if(Input.GetKey(KeyCode.RightArrow)) { //right
            direction = 1;
            if(rb.velocity.x < speed) {
                rb.AddForce(Vector2.right * 2);
                if(rb.velocity.x > 0) {
                    rb.AddForce(Vector2.right * rb.velocity.x);
                }
            }
        }
        if(Input.GetKey(KeyCode.LeftArrow)) { //left
            direction = -1;
            if(rb.velocity.x > -speed) {
                rb.AddForce(Vector2.left * 2);
                if(rb.velocity.x < 0) {
                    rb.AddForce(Vector2.left * -rb.velocity.x);
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
            if(rb.velocity.x > 1 || rb.velocity.x < -1) {
                rb.velocity -= new Vector2(rb.velocity.x / 2.5f, 0);
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) && rb.velocity.y == 0) {
            if(rb.velocity.x > -sprint && rb.velocity.x < sprint) {
                if(rb.velocity.x < -minSprint && Input.GetKey(KeyCode.LeftArrow)) {
                    rb.AddForce(Vector2.left * -rb.velocity.x);
                } else if(rb.velocity.x > minSprint && Input.GetKey(KeyCode.RightArrow)) {
                    rb.AddForce(Vector2.right * rb.velocity.x);
                }
            }
        }

        if(isAttacking == false) {
            if(direction == -1) { //left
                transform.eulerAngles = new Vector3(0, 180, 0);
            } else { //right
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        if(rb.velocity.y <= 0.05f && rb.velocity.y >= -0.05f) {
            grounded = true;
        } else {
            grounded = false;
        }

        if(Input.GetKeyDown(KeyCode.Z)) { //jump
            if(rb.velocity.y <= 0.05f && rb.velocity.y >= -0.05f) {
                rb.velocity += new Vector2(0, jump);
            }
        }

        if(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            bc.enabled = false;
            isAttacking = false;
        }

        //attack
        if(hammerIsUnlocked) {
            if(canAttack) {
                if(Input.GetKeyDown(KeyCode.X)) { //Light Attack
                    checkAttack = true;
                    StartCoroutine(AttackCheck());
                } else if(Input.GetKeyUp(KeyCode.X)) { //light attack
                    if(checkAttack) {
                        checkAttack = false;
                        StopCoroutine(AttackCheck());
                        weaponSR.color = new Color(0.05f, 0.05f, 0.05f);
                        heavyAttack = false;
                        Attack();
                    }
                }
            }
        } else {
            if(canAttack) {
                if(Input.GetKeyDown(KeyCode.X)) {
                    //light attack
                    Attack();
                }
            }
        }
        

        if(hammerIsUnlocked) {
            //ranged attack
            if(Input.GetKeyDown(KeyCode.C)) {
                RangeAttack();
            }
        }

        //recall
        if(Input.GetKeyDown(KeyCode.V)) {
            if(hammerUse == false) {
                //objects have been thrown
                hammerUse = true;
            }
        }

        //dash
        if(obtainDash) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                if(canDash) {
                    canDash = false;
                    atackable = false;
                    StartCoroutine(DashCooldown());
                }
            }
        }

        //inventory
        if(Input.GetKeyDown(KeyCode.F)) {
            inventoryOpen = !inventoryOpen;
            if(inventoryOpen) {
                //open inventory
                inventory.SetActive(true);
            } else {
                //close inventory
                inventory.SetActive(false);
            }
        }
    }

    IEnumerator DashCooldown() {
        for(int i = 0; i < 10; i++) {
            rb.AddForce(new Vector2(direction, 0) * 300f);
            yield return new WaitForSeconds(0.01f);
            if(rb.velocity.x > 20f || rb.velocity.x < -20f) {
                rb.velocity -= new Vector2(rb.velocity.x / 3f, 0);
            }
            if(i > 7) {
                rb.velocity -= new Vector2(rb.velocity.x / 3f, 0);
                if(rb.velocity.x < 2.5f || rb.velocity.x > -2.5f) {
                    rb.velocity -= new Vector2(rb.velocity.x / 1.1f, 0);
                }
            }
        }
        atackable = true;

        yield return new WaitForSeconds(2f);
        canDash = true;
    }

    IEnumerator AttackCheck() {
        yield return new WaitForSeconds(strikeSpeed);
        if(checkAttack) {
            heavyAttack = true;
            weaponSR.color = new Color(0.2f, 0.75f, 0.75f);
            Attack();
        }
    }

    void Attack() {
        canAttack = false;
        checkAttack = false;
        isAttacking = true;

        bc.enabled = true;
        if(Input.GetKey(KeyCode.UpArrow)) {
            anim.Play("AttackUp");
        } else if(Input.GetKey(KeyCode.DownArrow)) {
            anim.Play("AttackDown");
        } else {
            anim.Play("Attack");
        }

        if(inHS) {
            hsHealth--;

            if(hsHealth == 0) {
                hsHealth = 2;
                obtainDash = true;
                inHS = false;
            }
        }

        StartCoroutine(AttackRate());
    }

    IEnumerator AttackRate() {
        yield return new WaitForSeconds(rate);
        canAttack = true;
    }

    //directional variables
    bool[] dir = { false, false, false, false };
    Vector3[] dirNo = {
        new Vector3(0, 0, -90),
        new Vector3(0, 0, 90),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 180)
    };

    void RangeAttack() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
        float length = 10f;

        //cast ray in direction
        if(Input.GetKey(KeyCode.UpArrow)) {
            hit = Physics2D.Raycast(transform.position, Vector2.up * length, Mathf.Infinity);
            dir[0] = true;
        } else if(Input.GetKey(KeyCode.DownArrow)) {
            hit = Physics2D.Raycast(transform.position, Vector2.down * length, Mathf.Infinity);
            dir[1] = true;
        } else {
            if(direction == -1) { //left
                hit = Physics2D.Raycast(transform.position, Vector2.left * length, Mathf.Infinity);
                dir[2] = true;
            } else if(direction == 1) { //right
                hit = Physics2D.Raycast(transform.position, Vector2.right * length, Mathf.Infinity);
                dir[3] = true;
            }
        }

        //has hit something
        if(hit.collider != null) {
            if(hammerUse) { //is charged and hammer can be used
                //heavy attack
                tc.transform.position = transform.position;
                hammerObj = Instantiate(throwable, tc.transform);
                hammerObj.transform.position = tc.transform.position;

                StartCoroutine(CreateHammer(hit));
            }

            //lightningPos.position = hit.point;
            //for(int i = 0; i < dir.Length; i++) {
            //    if(dir[i]) {
            //        lightningPos.eulerAngles = dirNo[i];
            //        dir[i] = false;
            //    }
            //}
            //lightning.Play();
        }
    }

    IEnumerator CreateHammer(RaycastHit2D hit) {
        yield return new WaitForEndOfFrame();

        hammerObj.transform.position = hit.point;
        for(int i = 0; i < dir.Length; i++) {
            if(dir[i]) {
                hammerObj.transform.eulerAngles = dirNo[i];
                dir[i] = false;
            }
        }

        hammerUse = false;
    }

    public void AttackHit(Attack Attack) {
        if(overClock <= 95) {
            overClock += 5;
        }
        if(Input.GetKey(KeyCode.DownArrow)) { //Down
            rb.velocity += new Vector2(0, knockbackUp);
            extraOC = true;
            ocGiven = false;
        } else if(!Input.GetKey(KeyCode.DownArrow)) {
            rb.velocity += new Vector2(knockback * -direction, 0);
        }
        bc.enabled = false;
    }

    private void OnCollisionStay2D(Collision2D collision) { //height correction (going up stairs automatically)
        if(atackable) {
            if(collision.collider.tag == "Enemy" && iFrame == false) {
                StartCoroutine(TakeDamage());
            }
        }

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
                    }
                } else {
                    transform.position += new Vector3(sideCorrection, heightCorrection);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(atackable) {
            if(collision.name == "HealWeapon") {
                health -= 20;
                StartCoroutine(TakeDamage());
            } else if(collision.name == "RobotWeapon") {
                health -= 30;
                StartCoroutine(TakeDamage());
            } else if(collision.name == "dashHS") {
                inHS = true;
            }
        }
    }

    IEnumerator TakeDamage() {
        iFrame = true;
        health -= 10;

        sr.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        sr.color = new Color(0.05f, 0.05f, 0.05f);
        yield return new WaitForSeconds(0.5f);
        iFrame = false;
    }
}
