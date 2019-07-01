using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InputHelp : MonoBehaviour
{
    public Text helpText;
    //List<string> Names = new List<string>();

    //void Awake()
    //{
    //    Names.Add("RotateRightLeft");
    //    Names.Add("RotateUpDown");
    //    Names.Add("RotateSide");
    //    Names.Add("RotateUpDown");
    //    Names.Add("RotateSide");
    //}

    void Start()
    {
        ReadAxes();
    }

    public void ReadAxes()
    {
        helpText.text += "Управление:\n" +
            "Увеличение/уменьшение скорости:   <color=blue>+/-</color>" +
            "\nСтрейф вверх/вниз:                               <color=blue>Left_Shift/Left_Ctrl</color>" +
            "\nСтрейф влево/вправо:                           <color=blue>Z/X</color>" +
            "\nПоворот влево/вправо:                          <color=blue>A/D</color>" +
            "\nПоворот вниз/вверх:                              <color=blue>W/S</color>" +
            "\nПоворот вокруг оси:                              <color=blue>Q/E</color>" +
            "\nТорможение:                                          <color=blue>Space</color>" +
            "\nАвтоогонь:                                              <color=blue>F</color>" +
            "\nИнвентарь:                                              <color=blue>I</color>" +
            "\nУправление:                                           <color=blue>P</color>" +
            "\n---------------------------------------------------------" +
            "\nДополнительно:                                     <color=blue>H/J</color>" +
            "\nПоказать/скрыть курсор:                     <color=blue>LeftAlt</color>" +
            "\nВыход:                                                     <color=blue>Esc</color>" +
            "\nЧтобы включить/выключить какое-нибудь видео, откройте инвентарь(I) и кликните курсором по \"таблу\" в космосе" +
            "\nПереключить камеру между станцией и кораблем:     <color=blue>L</color>" +
            "\nЧтобы заспавнить врага, откройте инвентарь(I) и нажмите курсором на белый квадрат в космосе";

        //var InputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

        //SerializedObject obj = new SerializedObject(InputManager);

        //SerializedProperty axisArray = obj.FindProperty("m_Axes");

        //if (axisArray.arraySize == 0)
        //    Debug.Log("No axes");

        //for(int i = 0; i < axisArray.arraySize; i++)
        //{
        //    var axis = axisArray.GetArrayElementAtIndex(i);

        //    string name = axis.FindPropertyRelative("m_Name").stringValue;
        //    string positiveButton = axis.FindPropertyRelative("positiveButton").stringValue;
        //    string negativeButton = axis.FindPropertyRelative("negativeButton").stringValue;

        //    helpText.text += string.Format($"{name,-50}{positiveButton,-50}{negativeButton,-50}\n");
        //}
    }

    //public enum InputType
    //{
    //    KeyOrMouseButton,
    //    MouseMovement,
    //    JoystickAxis
    //}
}