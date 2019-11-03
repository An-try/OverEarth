using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace OverEarth
{
    /// <summary>
    /// Class for methods that can be used in different scenes of the game
    /// </summary>
    public static class Methods
    {
        public static Transform SearchNearestTarget(Transform originalTransform, List<string> targetTags, out Transform nearestTarget, EntityParameters entityParameter = EntityParameters.None, MinMaxValues minMaxValue = MinMaxValues.MaxValue)
        {
            Turret turretScript = originalTransform.GetComponent<Turret>();

            nearestTarget = null;
            Ship nearestShip = null;

            #region Get targets by tags
            // Get all root game objects with tags.
            List<Transform> targetsByTag = new List<Transform>();
            GameObject[] sceneRoots = SceneManager.GetActiveScene().GetRootGameObjects();

            for (int i = 0; i < sceneRoots.Length; i++)
            {
                for (int j = 0; j < targetTags.Count; j++)
                {
                    if (sceneRoots[i].CompareTag(targetTags[j]))
                    {
                        targetsByTag.Add(sceneRoots[i].transform);
                        break; // Do not check this root for other tags.
                    }
                }
            }
            #endregion

            #region Get targets by affected area
            List<Transform> targetsByAffectedArea = new List<Transform>();

            //float turretRange = turretScript.Range;

            //Vector3 rightTraverse = originalTransform.position + Quaternion.AngleAxis(turretScript.RightTraverse, originalTransform.up) * originalTransform.forward * turretRange;
            //Vector3 leftTraverse = originalTransform.position + Quaternion.AngleAxis(-turretScript.LeftTraverse, originalTransform.up) * originalTransform.forward * turretRange;
            //Vector3 elevation = originalTransform.position + Quaternion.AngleAxis(-turretScript.Elevation, originalTransform.right) * originalTransform.forward * turretRange;
            //Vector3 depression = originalTransform.position + Quaternion.AngleAxis(turretScript.Depression, originalTransform.right) * originalTransform.forward * turretRange;

            //Debug.DrawLine(originalTransform.position, rightTraverse, Color.red);
            //Debug.DrawLine(originalTransform.position, leftTraverse, Color.red);
            ////Debug.DrawLine(originalTransform.position, elevation, Color.green);
            ////Debug.DrawLine(originalTransform.position, depression, Color.black);

            //for (int i = 0; i < targetsByTag.Count; i++)
            //{
            //    Vector3 direction = targetsByTag[i].position - originalTransform.position;
            //    float angleBetweenTurretAndTarget = Vector3.Angle(originalTransform.forward, direction);

            //    Debug.DrawRay(originalTransform.position, originalTransform.forward * 100f, Color.blue);
            //    Debug.DrawRay(originalTransform.position, direction, Color.yellow);

            //    //float angleY = Mathf.Asin(Vector3.Cross(direction.normalized, originalTransform.forward).y) * Mathf.Rad2Deg;
            //    //angleY = Mathf.Abs(angleY);
            //    //if (angleBetweenTurretAndTarget >= 90)
            //    //{
            //    //    angleY = 90 + (90 - angleY);
            //    //}
            //    //Debug.Log(angleY);
            //    Vector3 localTargetPosY = originalTransform.InverseTransformPoint(targetsByTag[i].position);
            //    localTargetPosY.y = 0f; // Put the aiming point at the same vertical with this tower

            //    if (localTargetPosY.x >= 0f) // If the aim point is located above the turret
            //    {

            //    }



            //    Vector3 localTargetPosX = originalTransform.InverseTransformPoint(targetsByTag[i].position);
            //    localTargetPosX.x = 0f; // Put the aiming point at the same vertical with this tower

            //    if (localTargetPosX.y >= 0f) // If the aim point is located above the turret
            //    {

            //    }
            //}
            #endregion

            ///
            targetsByAffectedArea = targetsByTag;
            ///

            #region Get target by distance, or by min/max parameter
            if (entityParameter == EntityParameters.None)
            {
                nearestTarget = GetNearestObject(originalTransform, targetsByAffectedArea);
            }
            else
            {
                if (minMaxValue == MinMaxValues.MinValue)
                {
                    // Get nearest target by parameter.
                    float minParameterValue = Mathf.Infinity;

                    for (int i = 0; i < targetsByAffectedArea.Count; i++)
                    {
                        float parameterValue = targetsByAffectedArea[i].GetComponent<Ship>().GetParameterValue(entityParameter);
                        if (parameterValue < minParameterValue)
                        {
                            minParameterValue = parameterValue;
                            nearestTarget = targetsByAffectedArea[i];
                        }
                    }
                }
                if (minMaxValue == MinMaxValues.MaxValue)
                {
                    float maxParameterValue = 0;

                    for (int i = 0; i < targetsByAffectedArea.Count; i++)
                    {
                        float parameterValue = targetsByAffectedArea[i].GetComponent<Ship>().GetParameterValue(entityParameter);
                        if (parameterValue > maxParameterValue)
                        {
                            maxParameterValue = parameterValue;
                            nearestTarget = targetsByAffectedArea[i];
                        }
                    }
                }
            }
            #endregion

            #region Get nearest part of the target

            if (nearestTarget)
            {
                nearestShip = nearestTarget.GetComponent<Ship>();
                if (nearestShip && nearestShip.DamageableParts.Count > 0)
                {
                    return GetNearestDamageablePart(originalTransform, nearestShip.DamageableParts);
                }
            }

            return nearestTarget; // Set the nearest target if it was found. Otherwise, the nearest target will be null.

            #endregion
        }

        private static Transform GetNearestObject(Transform originalTransform, List<Transform> objectsForSearch)
        {
            Transform nearestObject = null;

            float minDistance = Mathf.Infinity;
            for (int i = 0; i < objectsForSearch.Count; i++)
            {
                float distance = Vector3.Distance(originalTransform.position, objectsForSearch[i].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = objectsForSearch[i];
                }
            }

            return nearestObject;
        }

        private static Transform GetNearestDamageablePart(Transform originalTransform, List<Damageable> damageablesForSearch)
        {
            Transform nearestDamageableObject = null;

            float minDistance = Mathf.Infinity;
            for (int i = 0; i < damageablesForSearch.Count; i++)
            {
                float distance = Vector3.Distance(originalTransform.position, damageablesForSearch[i].Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestDamageableObject = damageablesForSearch[i].transform;
                }
            }

            return nearestDamageableObject;
        }



        /// <summary>
        /// Coroutine to activate some game object with text for a while.
        /// </summary>
        /// <param name="WarningObject"></param>
        /// <param name="message"></param>
        /// <param name="showTime"></param>
        /// <returns></returns>
        public static IEnumerator ShowWarningMessage(GameObject WarningObject, string message, float showTime)
        {
            WarningObject.GetComponent<Text>().text = message; // Set the message to text component on this game object
            WarningObject.SetActive(true); // Activate the game object

            yield return new WaitForSeconds(showTime); // Wait some time

            WarningObject.SetActive(false); // Deactivate the game object
            WarningObject.GetComponent<Text>().text = null; // Delete text on component
        }
    }
}
