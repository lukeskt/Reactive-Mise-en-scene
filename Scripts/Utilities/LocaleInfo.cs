using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LocaleInfo : MonoBehaviour
    {
        public Locales locale;

        private ReactiveMesDataManager DataMgr;
        [HideInInspector] public string localeTendency;
        [HideInInspector] public double localeTendencyRating;

        // Start is called before the first frame update
        void Start()
        {
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
        }

        // Update is called once per frame
        void Update()
        {
            var localeInfo = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, locale);
            localeTendency = localeInfo.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            localeTendencyRating = localeInfo.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
        }
    }
}
