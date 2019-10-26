using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class IDsManager : Singleton<IDsManager>
    {
        public static List<string> IDs;

        public static string GenerateID()
        {
            string newID = Random.Range(int.MinValue, int.MaxValue).ToString();

            while (IDs.Contains(newID))
            {
                newID = Random.Range(int.MinValue, int.MaxValue).ToString();
            }

            IDs.Add(newID);
            print(newID);
            return newID;
        }

        public static bool RemoveID(string ID)
        {
            if (IDs.Contains(ID))
            {
                IDs.Remove(ID);
                return true;
            }

            return false;
        }
    }
}
