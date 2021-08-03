using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ReactiveMedia
{
    public class LoadMocapScene : MonoBehaviour
    {
        public OldLoaderMode loaderMode = OldLoaderMode.Tendency;

        Playable mocapTimeline;
        PlayableDirector mocapDirector;

        // Start is called before the first frame update
        void Start()
        {
            // Load the models, audio, prefabs, timeline, etc.
            // Start the timeline.
        }
    }
}