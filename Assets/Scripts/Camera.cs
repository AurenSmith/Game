using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public Transform player;

    private Vector3 offset = new Vector3(0, 1f, -10);

    public float smoothSpeed = 4f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void LateUpdate() {
        Vector3 pos = player.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, pos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }
}