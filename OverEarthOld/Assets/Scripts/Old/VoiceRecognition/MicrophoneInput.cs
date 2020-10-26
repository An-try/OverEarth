using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This script is no longer used.
/// This script requires AudioSource on this game object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    private AudioSource audioSource;

    private string selectedMicrophone; // Name of the selected microphone

    public AudioMixerGroup audioMixerGroupMaster;
    public AudioMixerGroup audioMixerGroupMicrophone;

    public bool useMicrophone;

    public int deviceIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get AudioSource component on this object

        if (useMicrophone)
        {
            if (Microphone.devices[deviceIndex] != null) // If a sound device with a specified index is connected to the computer
            {
                selectedMicrophone = Microphone.devices[deviceIndex]; // Get the name of this device
                audioSource.outputAudioMixerGroup = audioMixerGroupMicrophone; // Set the audio mixer to the audio source
                // Start to record a voice with selected microphone, looping and frequency
                audioSource.clip = Microphone.Start(selectedMicrophone, true, 1, AudioSettings.outputSampleRate);
            }
            else // If a sound device with a specified index does not exist
            {
                useMicrophone = false;
            }
        }

        audioSource.Play(); // Play the recorded sound
    }
}
