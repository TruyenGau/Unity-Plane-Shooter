using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerBullet;
    public Transform squawnPoint1;
    public Transform squawnPoint2;
    public GameObject flash;
    public float bulletSquawnTime = 1f;
    public AudioSource audioSource; 
    void Start()
    {
        flash.SetActive(false);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Vector3 bulletOffset = new Vector3(0, 1, 0); // Thay đổi giá trị 1 thành khoảng cách bạn muốn viên đạn dịch lên
            //Vector3 bulletStartPosition = transform.position + bulletOffset;
            Instantiate(playerBullet, squawnPoint1.position, Quaternion.identity);
            Instantiate(playerBullet, squawnPoint2.position, Quaternion.identity);

        }
    }
    void Fire()
    {
        Instantiate(playerBullet, squawnPoint1.position, Quaternion.identity);
        Instantiate(playerBullet, squawnPoint2.position, Quaternion.identity);
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(bulletSquawnTime);
           // Fire();
            audioSource.Play();
            flash.SetActive (true);
            yield return new WaitForSeconds(0.1f);
            flash.SetActive (false);
        }
    }
}
