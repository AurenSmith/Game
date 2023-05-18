using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEnemyEffect : MonoBehaviour {
    public Rigidbody2D rb;
    public BoxCollider2D bc;

    public int force = 10;
    public float smoothSpeed = 2f;

    private Vector2 pos;

    private bool spawned = false;

    // Start is called before the first frame update
    void Start() {
        bc.isTrigger = true;
        pos = transform.position;
        Vector2 speed = new Vector2(Random.Range(-force, force), Random.Range(-force, force) / 2);
        rb.AddForce(speed * 2);

        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update() {

    }

    private void LateUpdate() {
        if(spawned && bc.isTrigger) {
            Vector2 distance = Vector2.Lerp(pos, transform.localPosition, smoothSpeed);
            transform.position = distance;

            if(new Vector2(transform.position.x, transform.position.y) == pos) {
                Die();
            }
        }
    }

    IEnumerator Spawn() {
        yield return new WaitForSeconds(0.05f);
        bc.isTrigger = false;
        yield return new WaitForSeconds(1f);
        spawned = true;
        yield return new WaitForSeconds(1f);
        bc.isTrigger = true;
        yield return new WaitForSeconds(smoothSpeed);
        Die();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(spawned) {
            if(collision.name == "Healing Enemy") {
                Die();
            }
        }
    }

    void Die() {
        Destroy(this.gameObject);
    }
}
