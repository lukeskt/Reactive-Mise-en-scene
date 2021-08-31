using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LocaleInfo : MonoBehaviour
    {
        private ReactiveMesDataManager DataMgr;
        public ReactiveMesSettings RMSettings;

        public string locale;
        [HideInInspector] public int localeIndex = 0; // for custom editor
        public string tendency;
        public double tendencyRating;

        // Start is called before the first frame update
        void Start()
        {
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
        }

        // Update is called once per frame
        void Update()
        {
            var localeInfo = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, locale);
            tendency = localeInfo.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            tendencyRating = localeInfo.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
        }
    }
}
