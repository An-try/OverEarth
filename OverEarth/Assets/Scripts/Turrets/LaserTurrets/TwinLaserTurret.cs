using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinLaserTurret : Turret
{
    public GameObject RightLaserBeam;
    public GameObject LeftLaserBeam;
    
    public float hitDuration;

    public override void SetTurretParameters()
    {
        base.SetTurretParameters();

        turnRate = 30f;
        turretRange = 50000f;
        cooldown = 0.1f;
        currentCooldown = cooldown;
        
        rightTraverse = 180f;
        leftTraverse = 180f;
        elevation = 60f;
        depression = 5f;

        hitDuration = 0.1f;
    }

    public override void Shoot()
    {
        RightLaserBeam.GetComponent<Laser>().laserLength = turretRange;
        RightLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration);
        LeftLaserBeam.GetComponent<Laser>().laserLength = turretRange;
        LeftLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration);

        currentCooldown = cooldown + hitDuration;

        GetComponent<AudioSource>().Play();
    }
}
