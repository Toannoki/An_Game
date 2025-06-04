using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepSound;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        { 
            footstepSound.enabled = true;
        }
        else
        {
            footstepSound.enabled = false;
        }
    }
}
