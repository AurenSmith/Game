using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMover : MonoBehaviour {
    public LegMover opososite;
    public Transform target;
    public float distance;
    public LayerMask groundLayer;

    public Vector3 halfPoint;
    public int posIndex;
    public float lift;
    public float speed;

    public bool grounded;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        CheckGround();

        if(Vector2.Distance(target.position, transform.position) > distance && posIndex == 0 && opososite.grounded == true) {
            halfPoint = (transform.position + target.position) / 2;
            halfPoint.y += lift;
            posIndex = 1;
        } else if(posIndex == 1) {
            //Vector2.Lerp(target.position, halfPoint, speed * Time.deltaTime);
            target.position = halfPoint;

            float tDistance = target.position.x - transform.position.x;
            if(tDistance < -0.1f || tDistance > 0.1f) {
                posIndex = 2;
                Debug.Log(posIndex);
            }
        } else if(posIndex == 2) {
            //Vector2.Lerp(target.position, transform.position, speed * Time.deltaTime);
            target.position = transform.position;

            if(Vector2.Distance(target.position, transform.position) <= 0.1f) {
                posIndex = 0;
            }
        }

        if(posIndex == 0) {
            grounded = true;
        } else {
            grounded = false;
        }
    }

    public void CheckGround() {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector3.down, 5, groundLayer);
        if(hit.collider != null) {
            Vector3 point = hit.point;
            point.y += 0.25f;
            transform.position = point;
        }
    }
}
