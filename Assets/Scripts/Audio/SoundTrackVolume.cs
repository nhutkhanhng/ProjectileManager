using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrackVolume : MonoBehaviour
{
    public LayerMask layers;
    public SoundTrack soundTrack;

    void OnEnable()
    {
        soundTrack = GetComponentInParent<SoundTrack>();
    }

    [ContextMenu("TEst")]
    public void Test(Collider other)
    {
            soundTrack.PushTrack(this.name);
    }

    [ContextMenu("UnTest")]
    public void UnTest(Collider other)
    {
            soundTrack.PopTrack();
    }

}
