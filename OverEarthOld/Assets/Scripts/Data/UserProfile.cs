using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OverEarth
{
    [Serializable]
    public class UserProfile
    {
        public string Name = "";
        public Ship Ship = null;
        public Station Station = null;
        public Scene Scene;

        public void RewriteWithProfile(UserProfile newUserProfile)
        {
            this.Ship = newUserProfile.Ship;
            this.Station = newUserProfile.Station;
            this.Scene = newUserProfile.Scene;
        }
    }
}
