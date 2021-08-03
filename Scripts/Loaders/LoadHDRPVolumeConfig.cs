using ReactiveMedia;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ReactiveMedia
{
    public class LoadHDRPVolumeConfig : MonoBehaviour
    {
        public OldLoaderMode loaderMode;
        public RequestType requestType;
        public Locales localeToParse;

        private Volume volume;
        public List<VolumeProfile> volumeProfiles;
        public VolumeProfile presetVolumeProfile;
        AttentionDataManager DataMgr;
        //private Load_Init LoaderInit;

        // Start is called before the first frame update
        void Start()
        {
            volume = GetComponent<Volume>();
            //LoaderInit = gameObject.transform.parent.gameObject.GetComponentInChildren<Load_Init>();
            switch (loaderMode)
            {
                case OldLoaderMode.Preset:
                    volume.profile = presetVolumeProfile;
                    break;
                case OldLoaderMode.Random:
                    volume.profile = volumeProfiles[Random.Range(0, volumeProfiles.Count)];
                    break;
                case OldLoaderMode.Tendency:
                    // Add listener to response events
                    DataMgr = FindObjectOfType<AttentionDataManager>();
                    switch (requestType)
                    {
                        // note here: these are max-value derived, so akin to first-past the post... maybe check a threshold?
                        // also offer an inversion / min value?
                        case RequestType.Locale:
                            var LocaleTendencies = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeToParse);
                            var LocaleMaxKey = LocaleTendencies.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                            volume.profile = volumeProfiles.Find(profile => profile.name.Contains(LocaleMaxKey.ToString()));
                            break;
                        case RequestType.Global:
                            var GlobalTendencies = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                            var GlobalMaxKey = GlobalTendencies.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                            volume.profile = volumeProfiles.Find(profile => profile.name.Contains(GlobalMaxKey.ToString()));
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
