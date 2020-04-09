using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatIA : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D batRb;
    private Animator batAnimator;

    public GameObject hitbox;

    public float speed;
    public float timeToFly;
    private int h;
    public bool isLookLeft;
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        batRb = GetComponent<Rigidbody2D>();
        batAnimator = GetComponent<Animator>();

        StartCoroutine("slimeFly");
    }

    // Update is called once per frame
    void Update()
    {

		if ( h > 0 && isLookLeft) {
			Flip();
		} else if (h < 0 && !isLookLeft) {
			Flip();
		}
        batRb.velocity = new Vector2(h*speed, batRb.velocity.y);

    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "hitBox") {
            h=0;
            StopCoroutine("slimeFly");
			_GameController.playSFX(_GameController.sfxMorteInimigo,0.2f);
			batAnimator.SetTrigger("dead");
            Destroy(hitbox);
		}
	}

    IEnumerator slimeFly() {
        if (h == 1) h = -1;
        else h = 1;

        yield return new WaitForSeconds(timeToFly);
        StartCoroutine("slimeFly");
    }

    void onDead() {
        Destroy(this.gameObject);
    }

    void Flip() {
		isLookLeft = !isLookLeft;
		float x = transform.localScale.x * -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
