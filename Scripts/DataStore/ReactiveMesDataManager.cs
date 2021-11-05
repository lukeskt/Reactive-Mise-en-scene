using System;
using System.Collections.Generic;
using System.Linq; // WATCH OUT FOR LINQ PERF ISSUES
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public struct FocusDataStruct
    {
        public string name;
        public string tendency;
        public string locale;
        public double attentionRating;
    }

    [System.Serializable]
    public class ReactiveMesDataManager : MonoBehaviour
    {
        public static ReactiveMesDataManager dataMgr;
        [SerializeField] private ReactiveMesSettings RMesSettings;

        public List<FocusDataStruct> reactiveObjects = new List<FocusDataStruct>();

        private void Awake()
        {
            // Singleton adapted from: https://videlais.com/2021/02/20/singleton-global-instance-pattern-in-unity/
            if (dataMgr && dataMgr != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                dataMgr = this;
            }
        }

        public void ParseInboundStructData (FocusDataStruct attnData)
        {
            // On receipt of data, ensure in struct and put into list.
            if (!reactiveObjects.Any(attnStruct => attnStruct.name == attnData.name))
            {
                reactiveObjects.Add(attnData);
            }
            // TODO: check this works, need to say if exists check if values greater and replace, otherwise don't.
            else if (reactiveObjects.Any(attnStruct => attnStruct.name == attnData.name) && 
                attnData.attentionRating > reactiveObjects.Find(attnStruct => attnStruct.name == attnData.name).attentionRating)
            {
                reactiveObjects.Remove(reactiveObjects.Find(attnStruct => attnStruct.name == attnData.name));
                reactiveObjects.Add(attnData);
            }
        }

        public double GetTendencyRating (List<FocusDataStruct> attnStructs, string tendency)
        {
            List<FocusDataStruct> tendencyStructs = attnStructs.FindAll(attnStruct => attnStruct.tendency.Equals(tendency));
            double tendencyAttnRating = 0f;
            tendencyStructs.ForEach(tendencyStruct => tendencyAttnRating += tendencyStruct.attentionRating);
            return tendencyAttnRating;
        }

        public FocusDataStruct GetObjStructDetails (List<FocusDataStruct> attnStructs, string objectName)
        {
            FocusDataStruct attnStruct = attnStructs.Find(attnStruct => attnStruct.name.Equals(objectName));
            return attnStruct;
        }

        public Dictionary<string, double> GetLocaleTendency (List<FocusDataStruct> attnStructs, string locale)
        {
            Dictionary<string, double> locationTendencies = new Dictionary<string, double>();
            // get tendency of location by getting all objs in location then check strongest tendency.
            List<FocusDataStruct> locationStructs = attnStructs.FindAll(attnData => attnData.locale.Equals(locale));

            foreach (var tendency in RMesSettings.Tendencies)
            {
                double tendencyRating = GetTendencyRating(locationStructs, tendency);
                locationTendencies.Add(tendency, tendencyRating);
            }
            return locationTendencies;
        }

        public Dictionary<string, double> GetGlobalTendency (List<FocusDataStruct> focusStructs)
        {
            Dictionary<string, double> globalTendencies = new Dictionary<string, double>();
            foreach (var tendency in RMesSettings.Tendencies)
            {
                globalTendencies.Add(tendency, 0f);
            }
            foreach (var location in RMesSettings.Locales)
            {
                Dictionary<string, double> locationTendency = GetLocaleTendency(focusStructs, location);
                globalTendencies = globalTendencies.Concat(locationTendency).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
            }
            return globalTendencies;
        }
    }
}
