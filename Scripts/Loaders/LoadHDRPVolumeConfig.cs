using ReactiveMiseEnScene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ReactiveMiseEnScene
{
    public class LoadHDRPVolumeConfig : MonoBehaviour
    {
        public ReactiveMesSettings.TendencyAlgorithm tendencyAlgorithm;
        public ReactiveMesSettings.RequestType requestType;
        public string localeToParse;

        private Volume volume;
        public List<VolumeProfile> volumeProfiles;
        public VolumeProfile presetVolumeProfile;
        private ReactiveMesDataManager DataMgr;

        // Start is called before the first frame update
        void Start()
        {
            volume = GetComponent<Volume>();

            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            string TendencyForVolProfile;
            switch (requestType)
            {
                // note here: these are max-value derived, so akin to first-past the post... maybe check a threshold?
                // also offer an inversion / min value?
                case ReactiveMesSettings.RequestType.Global:
                    TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                    break;
                case ReactiveMesSettings.RequestType.Locale:
                    TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeToParse);
                    break;
                default:
                    break;
            }

            switch (tendencyAlgorithm)
            {
                case ReactiveMesSettings.TendencyAlgorithm.MaxValue:
                    TendencyForVolProfile = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(TendencyForVolProfile.ToString()));
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.MinValue:
                    TendencyForVolProfile = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(TendencyForVolProfile.ToString()));
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.Proportional:
                    NotImpl();
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.InverseProportion:
                    NotImpl();
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.CompetitorDistribution:
                    NotImpl();
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.Preset:
                    volume.profile = presetVolumeProfile;
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.Random:
                    volume.profile = volumeProfiles[Random.Range(0, volumeProfiles.Count)];
                    break;
                default:
                    goto case ReactiveMesSettings.TendencyAlgorithm.Preset;
            }
        }

        private void NotImpl()
        {
            throw new System.NotImplementedException();
        }
    }
}
