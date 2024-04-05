using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class EnemyScript : MonoBehaviour
{
    public Transform []gunPoint;
    public GameObject enemyBullet;
    public GameObject enemyFlash;
    public GameObject enemyExplosionPrefab;
    public float enemyBulletSpawnTime = 0.5f;
    public float speed = 1f;

    public float health = 10f;
    float barSize = 1f;
    float damage = 0f;

    public Healthbar healthbar;
    public GameObject damageEffect;
    public GameObject coinPrefab;
    public AudioClip bulletSound;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    public AudioSource audioSource;
  

    // Start is called before the first frame update
    void Start()
    {
        enemyFlash.SetActive(false);
        StartCoroutine(EnemyShooting());
        damage = barSize / health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet") 
        {
            audioSource.PlayOneShot(bulletSound);
            DamageHealthBar();
            Destroy(collision.gameObject);
            GameObject damageVfx = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(damageVfx, 0.05f);
            if(health <=0 )
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
            }
          
        }
    }

    void DamageHealthBar()
    {
        if (damage > 0)
        {
            health -= 1;
            barSize = barSize - damage;
            healthbar.SetSize(barSize);
        }
    }

    void EnemyFire()
    {
        for(int i = 0; i < gunPoint.Length; i++)
        {
            Instantiate(enemyBullet, gunPoint[i].position, Quaternion.identity);
        }
       // Instantiate(enemyBullet, gunPoint1.position, Quaternion.identity);
        //Instantiate(enemyBullet, gunPoint2.position, Quaternion.identity);

    }

    IEnumerator EnemyShooting()
    {
        while(true)
        {
            yield return new WaitForSeconds(enemyBulletSpawnTime);
            EnemyFire();
            audioSource.PlayOneShot(bulletSound, 0.5f); 
            enemyFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            enemyFlash.SetActive(false);

        }
    }
}
