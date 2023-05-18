using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public Transform[] sideEnemySpawner;
    public SideEnemy sideEnemy;

    GameObject myObj;
    GameObject newObj;

    private bool spawning = false;

    // Start is called before the first frame update
    void Start() {
        newObj = Instantiate(sideEnemy.gameObject, sideEnemySpawner[0]);
        newObj.GetComponent<SideEnemy>().spawnNo = 0;

        myObj = Instantiate(sideEnemy.gameObject, sideEnemySpawner[1]);
        myObj.GetComponent<SideEnemy>().spawnNo = 1;
    }

    // Update is called once per frame
    void Update() {
        if(spawning == false) {
            if(myObj.GetComponent<SideEnemy>().dead == true) {
                spawning = true;
                int no = myObj.GetComponent<SideEnemy>().spawnNo;
                StartCoroutine(Spawn(no));
            } else if(newObj.GetComponent<SideEnemy>().dead == true) {
                spawning = true;
                int no = newObj.GetComponent<SideEnemy>().spawnNo;
                StartCoroutine(Spawn(no));
            }
        }   
    }

    IEnumerator Spawn(int no) {
        if(no == 1) {
            yield return new WaitForSeconds(0.1f);
            Destroy(myObj);
            yield return new WaitForSeconds(1f);
            myObj = Instantiate(sideEnemy.gameObject, sideEnemySpawner[no]);
            myObj.GetComponent<SideEnemy>().spawnNo = 1;
            spawning = false;
        } else if(no == 0) {
            yield return new WaitForSeconds(0.1f);
            Destroy(newObj);
            yield return new WaitForSeconds(1f);
            newObj = Instantiate(sideEnemy.gameObject, sideEnemySpawner[no]);
            newObj.GetComponent<SideEnemy>().spawnNo = 0;
            spawning = false;
        }
    }
}
