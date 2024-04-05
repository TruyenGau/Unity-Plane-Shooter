using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10f;
    public float padding = 0.8f;
    public GameObject explosion;

    float minX;
    float maxX;
    float minY;
    float maxY;

    public float health = 20f;
    float barFillAmount = 1f;
    float damage = 0;

    public GameObject damageEffect;

    public PlayerHealthScript PlayerHealthScript;
    public CoinCount coinCountScript;
    public GameController gameController;
    public AudioSource audioSource;
    public AudioClip damageSource;
    public AudioClip explosionSound;

    public AudioClip coinSound;

    void Start()
    {
        FindBoundaries();
        damage = barFillAmount/health;
    }

    void FindBoundaries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        float detalX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float detalY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        float newXpos = Mathf.Clamp(transform.position.x +  detalX, minX, maxX);
        float newYpos = Mathf.Clamp(transform.position.y + detalY, minY, maxY);

        transform.position = new Vector2(newXpos, newYpos);
      /*  if(Input.GetMouseButton(0)) {
         Vector2 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = Vector2.Lerp(transform.position, newPos, 10 * Time.deltaTime);
        }
*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyBullet")
        {
            audioSource.PlayOneShot(damageSource, 0.5f);
            DamagePlayerHealth();
            Destroy(collision.gameObject);
            GameObject damageVfx = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(damageVfx, 0.1f);
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
                gameController.GameOver();
                Destroy(gameObject);
                GameObject blast = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(blast, 2f);
            }
     
        }
        if (collision.tag == "Coin"){
            audioSource.PlayOneShot(coinSound, 0.5f);
            Destroy(collision.gameObject);
            coinCountScript.addCount();
        }
    }

    void DamagePlayerHealth()
    {
        if(health > 0)
        {
            health = health - 1;
            barFillAmount = barFillAmount - damage;
            PlayerHealthScript.SetAmount(barFillAmount);
        }
    }
}
