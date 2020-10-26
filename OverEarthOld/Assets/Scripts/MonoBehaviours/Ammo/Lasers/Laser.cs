﻿using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any lasers in the game. This script is attached to the cannons of laser turrets. 
    /// </summary>
    public class Laser : Ammo
    {
        [HideInInspector] public float LaserLength;
        
        private LineRenderer _lineRenderer; // Component that draws a line
        
        private float _laserHitDuration;
        private float _currentHitDuration;
        
        private void Awake() // Awake is called when the script instance is being loaded
        {
            _lineRenderer = GetComponentInChildren<LineRenderer>(); // Get line renderer in the children's game object of this game object
        }

        private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
        {
            if (_currentHitDuration > 0) // If there is a laser duration
            {
                // Turn on the line renderer and draw a laser
                _lineRenderer.enabled = true;
                DrawLaserAndDealDamage();
                _currentHitDuration -= Time.deltaTime; // Decrease laser hit duration
            }
            else // If the laser duration time is up
            {
                _lineRenderer.enabled = false; // Turn off the line renderer
            }
        }

        public void SetHitDuration(float hitDuration)
        {
            _laserHitDuration = hitDuration;
            _currentHitDuration = hitDuration;
        }

        private void DrawLaserAndDealDamage()
        {
            // In this line renderer there are two points: starter point, from where laser starts and second point where laser hit something
            _lineRenderer.SetPosition(0, transform.position); // Set first laser point to this game object position
            
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) // If raycast hit something
            {
                _lineRenderer.SetPosition(1, hit.point); // Set the second point of the line renderer to the hit point

                // Deal a laser damage. Laser has a duration and each cycle of duration has several parts
                // Amount of this parts is calculates like this: HIT_DURATION / TIME_BETWEEN_EACH_DURATION_PART
                // For example: 0.1 / 0.02(Fixed update time) = 5 parts
                // Thus, the damage formula looks like this: DAMAGE_PER_HIT / (HIT_DURATION / TIME_BETWEEN_EACH_DURATION_PART)
                float damage = _damage / (_laserHitDuration / Time.fixedDeltaTime);
                DoDamage(damage, hit.collider);
            }
            else // If raycast hit nothing
            {
                // Set the imaginary point in front of the turret
                Vector3 laserTargetPoint = new Vector3(transform.position.x + transform.forward.x * LaserLength,
                                                       transform.position.y + transform.forward.y * LaserLength,
                                                       transform.position.z + transform.forward.z * LaserLength);

                _lineRenderer.SetPosition(1, laserTargetPoint); // Set the second point of the line renderer to the imaginary point
            }
        }
    }
}