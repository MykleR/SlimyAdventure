using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Audio/Sound")]
public class Sound : ScriptableObject
{
    new public string name="New Sound";
    public AudioClip clip;

    [Range(0,2)]
    public float volume=.5f;
    [Range(0, 5)]
    public float pitch=1;
    public bool loop = false;
    public bool next = false;
    public Sound nextSound;

    [HideInInspector]
    public AudioSource source;
}
