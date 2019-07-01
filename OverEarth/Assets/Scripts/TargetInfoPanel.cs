using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfoPanel : MonoBehaviour
{
    public Text TargetNameText;
    public Text TargetInfoText;

    private void Update()
    {
        if (Manager.instance.LastSelectedTarget)
        {
            TargetNameText.text = OutputTargetName();

            TargetInfoText.text =
                "Distance: " + OutputDistanceToTarget() +
                "\nHP: " + OutputTargetHP() +
                "\nDefence:" + OutputTargetDefence();
        }
        else
        {
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
        if (Manager.instance.PlayerShip)
        {
            return "<color=lime>" + (int)Vector3.Distance(Manager.instance.LastSelectedTarget.transform.position,
                                                          Manager.instance.PlayerShip.transform.position) + "</color>";
        }
        return "";
    }

    private string OutputTargetHP()
    {
        if (Manager.instance.LastSelectedTarget.GetComponent<Ship>())
        {
            return "<color=lime>" + Manager.instance.LastSelectedTarget.GetComponent<Ship>().HP.ToString() + "</color>";
        }
        return "";
    }

    private string OutputTargetDefence()
    {
        if (Manager.instance.LastSelectedTarget.GetComponent<Ship>())
        {
            return "<color=lime>\n Kinetic:     " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().kineticDefence + "%" +
                               "\n Explosion: " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().explosionDefence + "%" +
                               "\n Laser:       " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().laserDefence + "%" +
                               "\n Flame:       " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().flameDefence + "%" +
                               "\n Fragment:  " + Manager.instance.LastSelectedTarget.GetComponent<Ship>().fragmentDefence + "%</color>";
        }
        return "";
    }
}
