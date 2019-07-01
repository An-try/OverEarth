using UnityEngine;

public class PlasmaTurret : Turret
{
    public GameObject ProjectilePrefab;
    public GameObject RightShootPlace;
    public GameObject LeftShootPlace;

    private float bulletForce;
    private float turretScatter;

    public override void SetTurretParameters()
    {
        base.SetTurretParameters();

        turnRate = 30f;
        turretRange = 50000f;
        cooldown = 2f;
        currentCooldown = cooldown;

        rightTraverse = 180f;
        leftTraverse = 180f;
        elevation = 60f;
        depression = 5f;

        bulletForce = 50000f;
        turretScatter = 0.001f;

        ShootPlace = RightShootPlace;
    }

    public override void Shoot()
    {
        // Change shoot place(right or left in turn)
        if (ShootPlace == RightShootPlace)
        {
            ShootPlace = LeftShootPlace;
        }
        else
        {
            ShootPlace = RightShootPlace;
        }

        // Scatter while firing
        Vector3 scatter = new Vector3(Random.Range(-turretScatter, turretScatter),
                                      Random.Range(-turretScatter, turretScatter),
                                      Random.Range(-turretScatter, turretScatter));

        // Creating shoot animation
        GameObject shootAnimation = Instantiate(ShootAnimationPrefab, ShootPlace.transform.position, ShootPlace.transform.rotation, ShootPlace.transform);
        shootAnimation.transform.forward = TurretCannon.transform.forward;
        Destroy(shootAnimation.gameObject, shootAnimation.GetComponent<ParticleSystem>().main.duration);
        // Creating bullet
        GameObject bullet = Instantiate(ProjectilePrefab, ShootPlace.transform.position, ShootPlace.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce((ShootPlace.transform.forward + scatter) * bulletForce);

        currentCooldown = cooldown;
        GetComponent<AudioSource>().Play();
    }
}
