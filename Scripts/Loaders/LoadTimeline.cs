using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ReactiveMiseEnScene
{
    [RequireComponent(typeof(PlayableDirector))]
    public class LoadTimeline : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;
        [HideInInspector] public int localeIndex = 0; // for custom editor
        [HideInInspector] public int tendencyIndex = 0; // for custom editor

        public ReactiveMesSettings.TendencyAlgorithm tendencyAlgorithm = ReactiveMesSettings.TendencyAlgorithm.MaxValue;

        Playable timeline;
        PlayableDirector timelineDirector;

        // times if we need them
        private float startTime;
        public float timeToWait;

        // Start is called before the first frame update
        void Start()
        {
            startTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private bool CheckTimeSinceStart(float timeToWait)
        {
            float nowTime = Time.time;
            if (nowTime - startTime >= timeToWait)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}