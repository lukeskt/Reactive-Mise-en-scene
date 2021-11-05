using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ReactiveMiseEnScene
{
    public class TimelineTrackMuteUnmute : MonoBehaviour
    {
        // From: https://forum.unity.com/threads/mute-unmute-a-track-via-scripting.509604/
        PlayableDirector m_PlayableDirector;
        TimelineAsset m_TimelineAsset;
        // Start is called before the first frame update
        void Start()
        {
            m_PlayableDirector = FindObjectOfType<PlayableDirector>();
            m_TimelineAsset = (TimelineAsset)m_PlayableDirector.playableAsset;
        }
        public void MuteTrack(int inNum)
        {
            TrackAsset theTrack = m_TimelineAsset.GetOutputTrack(inNum);
            theTrack.muted = true;
            double t0 = m_PlayableDirector.time;
            m_PlayableDirector.RebuildGraph();
            m_PlayableDirector.time = t0;
            m_PlayableDirector.Play();
        }
        public void UnMuteTrack(int inNum)
        {
            TrackAsset theTrack = m_TimelineAsset.GetOutputTrack(inNum);
            theTrack.muted = false;
            double t0 = m_PlayableDirector.time;
            m_PlayableDirector.Stop();
            m_PlayableDirector.time = t0;
            m_PlayableDirector.Play();
            //You can also use this function ,but It will easily make your program crash especially when your timelineasset is huge.
            //TrackAsset theTrack = m_TimelineAsset.GetOutputTrack(inNum);
            //theTrack.muted = false;
            //double t0 = m_PlayableDirector.time;
            //m_PlayableDirector.RebuildGraph();
            //m_PlayableDirector.time = t0;
            //m_PlayableDirector.Play();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                MuteTrack(2);
            if (Input.GetKeyDown(KeyCode.U))
                UnMuteTrack(2);
        }
    }
}
