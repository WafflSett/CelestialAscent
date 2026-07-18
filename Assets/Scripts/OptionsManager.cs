using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private AudioManager audioManager;
    public bool IsAudioEnabled = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GetComponentInParent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
