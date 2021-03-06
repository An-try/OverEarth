﻿using UnityEngine;

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

    public override bool AimedAtEnemy()
    {
        // Missile launcher always "aimed on enemy" if there is the nearest target
        if (NearestTargetWithParameter != null)
        {
            return true;
        }
        return false;
    }

    public override void Shoot()
    {
        GameObject missile = Instantiate(MissilePrefab); // Create a new missile
        
        missile.transform.position = TurretCannons.transform.position; // Set missile position to this turret cannons position
        missile.transform.rotation = TurretCannons.transform.rotation; // Set missile rotation to this turret cannons rotation
        missile.tag = gameObject.tag; // Set missile tag equal to this turret

        currentCooldown = cooldown; // Add a cooldown to this turret

        GetComponent<AudioSource>().Play(); // Play an shoot sound
    }
}
