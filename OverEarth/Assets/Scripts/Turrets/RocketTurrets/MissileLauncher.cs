using UnityEngine;

public class MissileLauncher : Turret
{
    public GameObject MissilePrefab;

    public override void SetTurretParameters()
    {
        base.SetTurretParameters();

        turnRate = 10f;
        turretRange = 50000f;
        cooldown = 5f;
        currentCooldown = cooldown;
        rightTraverse = 180f;
        leftTraverse = 180f;
        elevation = 60f;
        depression = 5f;
    }

    public override bool AimedOnEnemy()
    {
        if (NearestTargetWithParameter != null)
        {
            return true;
        }
        return false;
    }

    public override void Shoot()
    {
        //// создание анимации
        //Transform shootAnimation = Instantiate(ShootAnimation);
        //// начальное расположение анимации
        //shootAnimation.transform.position = turretLauncher.transform.position;
        //// направление анимации
        //shootAnimationLeft.forward = turretLauncher.forward;

        //Destroy(shootAnimation.gameObject, 0.3f); // удалить объект анимации выстрела через 0.3 секунды
        
        GameObject missile = Instantiate(MissilePrefab); // Create a new missile

        // Set missile position, rotation and tag equal to this missile launcher
        missile.transform.position = TurretCannon.transform.position;
        missile.transform.rotation = TurretCannon.transform.rotation;
        missile.tag = gameObject.tag;

        currentCooldown = cooldown;

        GetComponent<AudioSource>().Play();
    }
}
