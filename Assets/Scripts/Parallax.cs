using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Transform background;
    private Camera cam;
    public float speed;
    private Vector3 previewCamPosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        previewCamPosition = cam.transform.position;
    }

    void LateUpdate() {
        float parallaxX = previewCamPosition.x - cam.transform.position.x;
        float bgTargetX = background.position.x + parallaxX;

        Vector3 bgPosition = new Vector3(bgTargetX, background.position.y, background.position.z);
        background.position = Vector3.Lerp(background.position, bgPosition, speed * Time.deltaTime);

        previewCamPosition = cam.transform.position;
    
    }
}
