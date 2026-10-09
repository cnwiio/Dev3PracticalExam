using System.Runtime.CompilerServices;
using UnityEngine;

public class GetAudio : MonoBehaviour
{
    //private SoundManager soundManager => FindAnyObjectByType<SoundManager>();
    public void playClick()
    {
        SoundManager.Instance.PlaySFX("Click");
    }

    public void StopAmbient(string name)
    {
        SoundManager.Instance.StopAmbient(name);
    }
}