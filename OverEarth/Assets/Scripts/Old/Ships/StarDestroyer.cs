using System.Collections;
using UnityEngine;

namespace OverEarth
{
    public class StarDestroyer : Ship
    {
        private protected override void Start()
        {
            base.Start();

            for (int i = 0; i < DamageableParts.Count; i++)
            {
                DamageableParts[i].SetDefaultParameters();
            }
        }
    }
}
