using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool paused;
    float roty = 45, rotx = 10;
    [SerializeField]
    AudioClip gunSound;


    [SerializeField]
    LayerMask raycastLayers;//this is used to choose what objects get hit by the raycast
    
    
    // Update is called once per frame
    void Update()
    {
        //Aim
        if (Input.GetKeyDown(KeyCode.Escape)) paused = !paused;

        if (paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            roty += (Input.GetAxis("Mouse X") * 3) % 360;
            rotx -= (Input.GetAxis("Mouse Y") * 3) % 360;
            rotx = Mathf.Clamp(rotx, -90, 90);
            
            transform.eulerAngles = new Vector3(rotx, roty, 0);
        }

        //raycast
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, raycastLayers);

        print(hit);
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().PlayOneShot(gunSound);
            if (hit.rigidbody != null && hit.transform.tag == "Enemy")
            {
                HitEnemy(hit);
            }
        }
    }

    void HitEnemy(RaycastHit hit)
    {
        Transform enemy = hit.transform;
        enemy.GetComponent<Rigidbody>().AddForceAtPosition((enemy.position - transform.position).normalized * 100, hit.point);
        enemy.tag = "DeadEnemy";
        enemy.GetComponent<Rigidbody>().freezeRotation = false;
        GameObject enemy2 = Instantiate(Resources.Load("RedBoi"), new Vector3(Random.Range(-5f, 5f), 5f, Random.Range(-5f, 5f)), new Quaternion()) as GameObject;
        enemy2.transform.Rotate(0, Random.Range(0, 360), 0);
    }
}
