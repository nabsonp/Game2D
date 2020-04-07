using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Transform playerTransform;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate() {
        Vector3 posCam = new Vector3(playerTransform.position.x, playerTransform.position.y, cam.transform.position.z);
        cam.transform.position = posCam;
    }
}
