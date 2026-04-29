using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Camera fpsCam;

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Shoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Vector3 direction = fpsCam.transform.forward;

        projectile.GetComponent<Projectile>().Launch(direction);
    }
}