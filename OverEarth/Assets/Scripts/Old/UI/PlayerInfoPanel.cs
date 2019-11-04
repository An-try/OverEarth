using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        public static PlayerInfoPanel instance; // Singleton for this script

        public Text PlayerInfoText;

        private Ship playerShipScript;

        // Text colors that will be displayed on the player info panel
        private string mainStringColor = "white";
        private string positiveStringColor = "lime";
        private string negativeStringColor = "red";

        private void Awake() // Awake is called when the script instance is being loaded
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

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            // Delegate called when player ship is assigned
            //Manager.instance.onPlayerShipAssigned += StartUpdateShipInfo;
        }

        private void StartUpdateShipInfo()
        {
            //InvokeRepeating("UpdateShipInfo", 0, 0.1f);
            //playerShipScript = Manager.instance.PlayerShip.GetComponent<Ship>(); // Get player ship script
        }

    //    private void UpdateShipInfo()
    //    {
    //        // If player ship exists display the ship's info
    //        if (Manager.instance.PlayerShip)
    //        {
    //            // Display player info
    //            PlayerInfoText.text =
    ///* Health points       */ $"<color={mainStringColor}>Health: </color>" + OutputHP() +
    ///* Defence             */ //$"\n<color={mainStringColor}>Defence:</color>" + OutputDefence() +
    ///* Speed               */ $"\n<color={mainStringColor}>Speed: </color>" + OutputSpeed() +
    ///* Transmission        */ $"\n<color={mainStringColor}>Transmission: </color>" + OutputTransmission() +
    ///* Stop force          */ $"\n<color={mainStringColor}>Stop force: </color>" + OutputStopForce() +
    ///* Rotation stop force */ $"\n<color={mainStringColor}>Rotation stop force: </color>" + OutputRotationStopForce() +
    ///* Turret auto fire    */ $"\n<color={mainStringColor}>Turret auto fire: </color>" + OutputIfAutoFire();
    //        }
    //        else // If player ship does not exist
    //        {
    //            PlayerInfoText.text = $"<color={negativeStringColor}>Player ship has been destroyed</color>"; // Inform player about it
    //        }
    //    }

        //private string OutputHP()
        //{
        //    int playerShipHP = (int)playerShipScript.CurrentDurability; // Get player ship HP
        //    return $"<color={positiveStringColor}>" + playerShipHP + "</color>"; // Return player ship HP
        //}

        //private string OutputDefence()
        //{
        //    // Get defences of player ship and return them
        //    return $"<color={positiveStringColor}>\n Kinetic:     " + playerShipScript.kineticDefence + "%" +
        //                                         "\n Explosion: " + playerShipScript.explosionDefence + "%" +
        //                                         "\n Laser:       " + playerShipScript.laserDefence + "%" +
        //                                         "\n Flame:       " + playerShipScript.flameDefence + "%" +
        //                                         "\n Fragment:  " + playerShipScript.fragmentDefence + "%</color>";
        //}

        //private string OutputSpeed()
        //{
        //    // Get the rigidbody velocity magnitude(length of the vector) of player ship
        //    int playerShipSpeed = (int)PlayerShipController.Instance.ShipVelocityMagnitude;

        //    if (playerShipSpeed > 0)
        //    {
        //        return $"<color={positiveStringColor}>" + playerShipSpeed + "</color>";
        //    }
        //    return $"<color={negativeStringColor}>" + playerShipSpeed + "</color>";
        //}

        //private string OutputTransmission()
        //{
        //    // Get the player ship transmission
        //    int playerShipTransmission = playerShipScript.transmission;

        //    if (playerShipTransmission > 0)
        //    {
        //        return $"<color={positiveStringColor}>" + playerShipTransmission + "</color>";
        //    }
        //    return $"<color={negativeStringColor}>" + playerShipTransmission + "</color>";
        //}

        //private string OutputStopForce()
        //{
        //    // Get the rigidbody drag
        //    int stopForce = (int)PlayerShipController.Instance.ShipDrag;

        //    if (stopForce > 0)
        //    {
        //        return $"<color={positiveStringColor}>" + stopForce + "</color>";
        //    }
        //    return $"<color={negativeStringColor}>" + stopForce + "</color>";
        //}

        //private string OutputRotationStopForce()
        //{
        //    // Get the rigidbody angular drag
        //    int stopRotationForce = (int)PlayerShipController.Instance.ShipAngularDrag;

        //    if (stopRotationForce > 0)
        //    {
        //        return $"<color={positiveStringColor}>" + stopRotationForce + "</color>";
        //    }
        //    return $"<color={negativeStringColor}>" + stopRotationForce + "</color>";
        //}

        //private string OutputIfAutoFire()
        //{
        //    if (PlayerController.Instance.IsAIEnabled) // If the auto fire enabled
        //    {
        //        return $"<color={positiveStringColor}>Yes</color>";
        //    }
        //    return $"<color={negativeStringColor}>No</color>";
        //}
    }
}
