using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Transform playerTransform, limiteCamEsq, limiteCamDir, limiteCamCima, limiteCamBaixo;
    public float speedCam;
    private Camera cam;

    [Header("Audio")]
    public AudioSource sfxSource, musicSource;

    public AudioClip sfxJump, sfxAtack, sfxMoeda, sfxMorteInimigo, sfxDano;
    public AudioClip musicEnd, musicGameOver;
    public AudioClip[]  sfxStep;

    public enum gameState{
        TITULO, GAMEPLAY, END, GAMEOVER
    }

    public gameState currentState;
    public GameObject panelTitulo, panelOver, panelEnd;

    public int moedasColetadas, vida;
    public Text mooedasTxt;
    public Image[] coracoes;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        hearthController();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == gameState.TITULO && Input.GetKeyDown(KeyCode.Space)) {
            currentState = gameState.GAMEPLAY;
            panelTitulo.SetActive(false);
        } else if (currentState == gameState.GAMEOVER && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (currentState == gameState.END && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

    public void playSFX(AudioClip sfxClip, float volume) {
        sfxSource.PlayOneShot(sfxClip,volume);
    }

    void hearthController() {
        foreach(Image h in coracoes) {
            h.enabled = false;
        }
        for (int i=0; i< vida; i++) {
            coracoes[i].enabled = true;
        }
    }

    public void getHit() {
        vida--;
        hearthController();
        if (vida < 1) {
            playerTransform.gameObject.SetActive(false);
            panelOver.SetActive(true);
            trocarMusica(musicGameOver);
            currentState = gameState.GAMEOVER;
        }
    } 

    public void getCoin() {
        moedasColetadas++;
        mooedasTxt.text = moedasColetadas.ToString();
    } 

    void LateUpdate() {
        camController();
    }

    public void theEnd() {
        currentState = gameState.END;
        trocarMusica(musicEnd);
        panelEnd.SetActive(true);
    }

    public void trocarMusica(AudioClip music) {
        musicSource.clip = music;
        musicSource.Play();
    }
}

