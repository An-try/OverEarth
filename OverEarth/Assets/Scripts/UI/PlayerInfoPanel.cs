using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    public static PlayerInfoPanel instance;
    
    public Text PlayerInfoText;

    string mainStringColor = "white";
    string positiveStringColor = "lime";
    string negativeStringColor = "red";

    private void Awake()
    {
        if (instance == null) // If instance not exist
        {
            instance = this; // Set up instance as this script
        }
        else //If instance already exists
        {
            Destroy(this); // Destroy this script
        }
    }

    private void Start()
    {
        Manager.instance.onPlayerShipAssigned += StartUpdateShipInfo;
    }

    private void StartUpdateShipInfo()
    {
        InvokeRepeating("UpdateShipInfo", 0, 0.1f);
    }

    private void UpdateShipInfo()
    {
        if (Manager.instance.PlayerShip != null)
        {
            PlayerInfoText.text =
/* Health points     */ $"<color={mainStringColor}>Health: </color>" + OutputCurrentHP() +
/* Defence           */ $"\n<color={mainStringColor}>Defence:</color>" + OutputCurrentDefence() +
/* Speed             */ $"\n<color={mainStringColor}>Speed: </color>" + OutputCurrentSpeed() +
/* Transmission      */ $"\n<color={mainStringColor}>Transmission: </color>" + OutputCurrentTransmission() +
/* Stop force        */ $"\n<color={mainStringColor}>Stop force: </color>" + OutputCurrentStopForce() +
/* Turret auto fire  */ $"\n<color={mainStringColor}>Turret auto fire: </color>" + OutputIfAutoFire();
        }
        else
        {
            PlayerInfoText.text = $"<color={negativeStringColor}>Player ship has been destroyed</color>";
        }
    }

    private string OutputCurrentHP()
    {
        int playerShipHP = (int)Manager.instance.PlayerShip.GetComponent<Ship>().HP;
        return $"<color={positiveStringColor}>" + playerShipHP + "</color>";
    }

    private string OutputCurrentDefence()
    {
        Ship PlayerShipScript = Manager.instance.PlayerShip.GetComponent<Ship>();

        return $"<color={positiveStringColor}>\n Kinetic:     " + PlayerShipScript.kineticDefence + "%" +
                                             "\n Explosion: " + PlayerShipScript.explosionDefence + "%" +
                                             "\n Laser:       " + PlayerShipScript.laserDefence + "%" +
                                             "\n Flame:       " + PlayerShipScript.flameDefence + "%" +
                                             "\n Fragment:  " + PlayerShipScript.fragmentDefence + "%</color>";
    }

    private string OutputCurrentSpeed()
    {
        int PlayerSpeed = (int)Manager.instance.PlayerShip.GetComponent<Rigidbody>().velocity.magnitude;

        if (PlayerSpeed > 0)
        {
            return $"<color={positiveStringColor}>" + PlayerSpeed + "</color>";
        }
        return $"<color={negativeStringColor}>" + PlayerSpeed + "</color>";
    }

    private string OutputCurrentTransmission()
    {
        int PlayerTransmission = Manager.instance.PlayerShip.GetComponent<Ship>().transmission;

        if (PlayerTransmission > 0)
        {
            return $"<color={positiveStringColor}>" + PlayerTransmission + "</color>";
        }
        return $"<color={negativeStringColor}>" + PlayerTransmission + "</color>";
    }

    private string OutputCurrentStopForce()
    {
        int StopForce = (int)PlayerMovement.PlayerShipRigidbody.drag;

        if (StopForce > 0)
        {
            return $"<color={positiveStringColor}>" + StopForce + "</color>";
        }
        return $"<color={negativeStringColor}>" + StopForce + "</color>";
    }

    private string OutputIfAutoFire()
    {
        if (PlayerMovement.instance.autoFire)
        {
            return $"<color={positiveStringColor}>Yes</color>";
        }
        return $"<color={negativeStringColor}>No</color>";
    }
}