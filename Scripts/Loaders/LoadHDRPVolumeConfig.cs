using ReactiveMedia;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ReactiveMedia
{
    public class LoadHDRPVolumeConfig : MonoBehaviour
    {
        public TendencyAlgorithm tendencyAlgorithm;
        public RequestType requestType;
        public Locales localeToParse;

        private Volume volume;
        public List<VolumeProfile> volumeProfiles;
        public VolumeProfile presetVolumeProfile;
        private AttentionDataManager DataMgr;

        // Start is called before the first frame update
        void Start()
        {
            volume = GetComponent<Volume>();

            switch (tendencyAlgorithm)
            {
                case TendencyAlgorithm.MaxValue:
                    DataMgr = FindObjectOfType<AttentionDataManager>();
                    Dictionary<Tendencies, double> TendenciesFromDataMgr = new Dictionary<Tendencies, double>();
                    Tendencies MaxKey;
                    switch (requestType)
                    {
                        // note here: these are max-value derived, so akin to first-past the post... maybe check a threshold?
                        // also offer an inversion / min value?
                        case RequestType.Locale:
                            TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeToParse);
                            break;
                        case RequestType.Global:
                            TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                            break;
                        default:
                            break;
                    }
                    MaxKey = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(MaxKey.ToString()));
                    break;
                case TendencyAlgorithm.MinValue:
                    NotImpl();
                    break;
                case TendencyAlgorithm.Proportional:
                    NotImpl();
                    break;
                case TendencyAlgorithm.InverseProportion:
                    NotImpl();
                    break;
                case TendencyAlgorithm.CompetitorDistribution:
                    NotImpl();
                    break;
                case TendencyAlgorithm.Preset:
                    volume.profile = presetVolumeProfile;
                    break;
                case TendencyAlgorithm.Random:
                    volume.profile = volumeProfiles[Random.Range(0, volumeProfiles.Count)];
                    break;
                default:
                    goto case TendencyAlgorithm.Preset;
            }
        }

        private void NotImpl()
        {
            throw new System.NotImplementedException();
        }
    }
}
