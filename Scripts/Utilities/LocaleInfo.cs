using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMedia
{
    public class LocaleInfo : MonoBehaviour
    {
        public Locales locale;

        private AttentionDataManager DataMgr;
        [HideInInspector] public Tendencies localeTendency;
        [HideInInspector] public double localeTendencyRating;

        // Start is called before the first frame update
        void Start()
        {
            DataMgr = FindObjectOfType<AttentionDataManager>();
        }

        // Update is called once per frame
        void Update()
        {
            var localeInfo = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, locale);
            localeTendency = localeInfo.Max().Key;
            localeTendencyRating = localeInfo.Max().Value;
        }
    }
}
