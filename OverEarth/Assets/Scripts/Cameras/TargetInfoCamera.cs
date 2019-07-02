using UnityEngine;

/// <summary>
/// Class for camera that follows current selected target by player.
/// </summary>
public class TargetInfoCamera : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0f, 0f, -30f);

    private void Awake() // Awake is called when the script instance is being loaded
    {
        // Subscribe on the delegate that is called when new selected target assigned
        Manager.instance.onCurrentSelectedTargetAssigned += ChangeTargetForCamera;
    }

    private void ChangeTargetForCamera()
    {
        transform.SetParent(Manager.instance.CurrentSelectedTarget.transform); // Parent this camera to the new selected target
        // Set position with offset of this camera to the new selected target
        transform.position = Manager.instance.CurrentSelectedTarget.transform.position + cameraOffset;
    }

    private void OnDestroy() // Called when destroying this script
    {
        Manager.instance.onCurrentSelectedTargetAssigned -= ChangeTargetForCamera; // Unsubscribe from delegate
    }
}
