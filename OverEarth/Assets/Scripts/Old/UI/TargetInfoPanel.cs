using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    public class TargetInfoPanel : MonoBehaviour
    {
        public Text TargetNameText;
        public Text TargetInfoText;

        private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
        {
            if (Manager.instance.LastSelectedTarget) // If there is any remembered target
            {
                TargetNameText.text = OutputTargetName(); // Display target name

                // Display target info
                TargetInfoText.text =
                    "Distance: " + OutputDistanceToTarget() +
                    "\n" + OutputTargetHP();// +
                    //"\n" + OutputTargetDefence();
            }
            else // If there is no remembered target
            {
                // Inform player about it
                TargetNameText.text = "<color=red>NO TARGET</color>";
                TargetInfoText.text = "";
            }
        }

        private string OutputTargetName()
        {
            return Manager.instance.LastSelectedTarget.name;
        }

        private string OutputDistanceToTarget()
        {
            if (Manager.instance.PlayerShip) // If player ship exists
            {
                // Return the distance from the target and player ship
                return "<color=lime>" + (int)Vector3.Distance(Manager.instance.LastSelectedTarget.transform.position,
                                                              Manager.instance.PlayerShip.transform.position) + "</color>";
            }
            return "";
        }

        private string OutputTargetHP()
        {
            if (Manager.instance.LastSelectedTarget.GetComponent<Ship>()) // If the target has a Ship script
            {
                // Return ship's HP
                return "HP: <color=lime>" + Manager.instance.LastSelectedTarget.GetComponent<Ship>().CurrentDurability.ToString() + "</color>";
            }
            return "";
        }

        //private string OutputTargetDefence()
        //{
        //    if (Manager.instance.LastSelectedTarget.GetComponent<Ship>()) // If the target has a Ship script
        //    {
        //        // Return ship's defences
        //        return "Defence:" +
        //            "<color=lime>\n Kinetic:     " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().kineticDefence + "%" +
        //            "\n Explosion: " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().explosionDefence + "%" +
        //            "\n Laser:       " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().laserDefence + "%" +
        //            "\n Flame:       " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().flameDefence + "%" +
        //            "\n Fragment:  " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().fragmentDefence + "%</color>";
        //    }
        //    return "";
        //}
    }
}
