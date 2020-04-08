using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Transform playerTransform, limiteCamEsq, limiteCamDir, limiteCamCima, limiteCamBaixo;
    public float speedCam;
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
    void camController() {
        float posicaoCamX = playerTransform.position.x;
        float posicaoCamY = playerTransform.position.y;

        if (cam.transform.position.x < limiteCamEsq.position.x && playerTransform.position.x < limiteCamEsq.position.x) {
            posicaoCamX = limiteCamEsq.position.x;
        } else if (cam.transform.position.x > limiteCamDir.position.x && playerTransform.position.x > limiteCamDir.position.x) {
            posicaoCamX = limiteCamDir.position.x;
        }

        if (cam.transform.position.y < limiteCamBaixo.position.y && playerTransform.position.y < limiteCamBaixo.position.y) {
            posicaoCamY = limiteCamBaixo.position.y;
        } else if (cam.transform.position.y > limiteCamCima.position.y && playerTransform.position.y > limiteCamCima.position.y) {
            posicaoCamY = limiteCamCima.position.y;
        }

        Vector3 posCam = new Vector3(posicaoCamX, posicaoCamY, cam.transform.position.z);
        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }
    void LateUpdate() {
        camController();
    }
}

