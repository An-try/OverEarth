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
        RightLaserBeam.GetComponent<Laser>().laserLength = turretRange; // Set the laset lenght to the right laser beam
        RightLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration); // Set the hit duration to the right laser beam

        LeftLaserBeam.GetComponent<Laser>().laserLength = turretRange; // Set the laset lenght to the left laser beam
        LeftLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration); // Set the hit duration to the left laser beam

        currentCooldown = cooldown + hitDuration; // Add a cooldown to this turret

        GetComponent<AudioSource>().Play(); // Play an shoot sound
    }
}
