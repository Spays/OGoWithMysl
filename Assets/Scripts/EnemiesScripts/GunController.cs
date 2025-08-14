using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;  // Префаб снаряда
    public Transform firePoint;      // Точка, откуда вылетает пуля
    public float bulletForce = 10f;  // Сила выстрела
    public float fireRate = 0.2f;    // Задержка между выстрелами

    private float nextFireTime;

    void Update()
    {
        /*
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        */
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse); // firePoint.up — направление вверх локального объекта
    }
}