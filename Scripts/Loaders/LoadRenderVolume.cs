using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace ReactiveMiseEnScene
{
    [RequireComponent(typeof(Volume), (typeof(Collider)))]
    public class LoadRenderVolume : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;
        public ReactiveMesSettings.SingleResultTendencyAlgorithm algorithm;
        public ReactiveMesSettings.RequestType requestType;
        //public string presetTendency;
        //[HideInInspector] public int tendencyIndex = 0; // for custom editor
        public string localeRequest;
        [HideInInspector] public int localeIndex = 0; // for custom editor

        [Tooltip("WARNING EXPERIMENTAL FEATURE!\nIf Continuous is checked, update the tendency-based volume profile every frame.")]
        public bool continuous = false;

        private Volume volume;
        public List<VolumeProfile> volumeProfiles;
        public VolumeProfile presetVolumeProfile;
        private ReactiveMesDataManager DataMgr;

        // Start is called before the first frame update
        void Start()
        {
            volume = GetComponent<Volume>();
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            RenderVolumeLoader(localeRequest);
        }

        private void Update()
        {
            if (continuous)
            {
                RenderVolumeLoader(localeRequest); // change this to the local locale?
            }
        }

        private void RenderVolumeLoader (string localeToRequest)
        {
            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            string TendencyForVolProfile;
            switch (requestType)
            {
                // note here: these are max-value derived, so akin to first-past the post... maybe check a threshold?
                // also offer an inversion / min value?
                case ReactiveMesSettings.RequestType.Global:
                    TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.reactiveObjects);
                    break;
                case ReactiveMesSettings.RequestType.Locale:
                    TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.reactiveObjects, localeToRequest);
                    break;
                default:
                    break;
            }

            switch (algorithm)
            {
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency:
                    TendencyForVolProfile = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(TendencyForVolProfile));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondStrongest:
                    var SortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(SortedTendencies[1].Key));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondWeakest:
                    var UnsortTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).ToList();
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(UnsortTendencies[1].Key));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.WeakestTendency:
                    TendencyForVolProfile = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(TendencyForVolProfile));
                    break;
                //case ReactiveMesSettings.SingleResultTendencyAlgorithm.Preset:
                //    volume.profile = presetVolumeProfile;
                //    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    volume.profile = volumeProfiles[Random.Range(0, volumeProfiles.Count)];
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency;
            }
        }
    }
}
