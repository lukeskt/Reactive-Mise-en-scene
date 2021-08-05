using ReactiveMedia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // WATCH OUT FOR LINQ PERF ISSUES
using UnityEngine;
using UnityEngine.Events;

namespace ReactiveMedia
{
    public struct AttnDataStruct
    {
        public string name;
        public Tendencies tendency;
        public Locales locale;
        public double attentionRating;
    }

    // below would be potential alt impl for using with reactivemediasettings SO.
    //public struct AttnDataStructAlt
    //{
    //    public string name;
    //    public string tendency;
    //    public string locale;
    //    public double attentionRating;
    //}

    [System.Serializable]
    public class AttentionDataManager : MonoBehaviour
    {
        public static AttentionDataManager attentionStore;

        public List<AttnDataStruct> attentionObjects = new List<AttnDataStruct>();

        private void Awake()
        {
            // Singleton adapted from: https://videlais.com/2021/02/20/singleton-global-instance-pattern-in-unity/
            if (attentionStore && attentionStore != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                attentionStore = this;
            }
        }

        public void ParseInboundStructData (AttnDataStruct attnData)
        {
            // On receipt of data, ensure in struct and put into list.
            if (!attentionObjects.Any(attnStruct => attnStruct.name == attnData.name))
            {
                attentionObjects.Add(attnData);
            }
            // TODO: check this works, need to say if exists check if values greater and replace, otherwise don't.
            else if (attentionObjects.Any(attnStruct => attnStruct.name == attnData.name) && 
                attnData.attentionRating > attentionObjects.Find(attnStruct => attnStruct.name == attnData.name).attentionRating)
            {
                attentionObjects.Remove(attentionObjects.Find(attnStruct => attnStruct.name == attnData.name));
                attentionObjects.Add(attnData);
            }
        }

        public double GetTendencyRating (List<AttnDataStruct> attnStructs, Tendencies tendency)
        {
            List<AttnDataStruct> tendencyStructs = attnStructs.FindAll(attnStruct => attnStruct.tendency.Equals(tendency));
            double tendencyAttnRating = 0f;
            tendencyStructs.ForEach(tendencyStruct => tendencyAttnRating += tendencyStruct.attentionRating);
            return tendencyAttnRating;
        }

        public AttnDataStruct GetObjStructDetails (List<AttnDataStruct> attnStructs, string objectName)
        {
            AttnDataStruct attnStruct = attnStructs.Find(attnStruct => attnStruct.name.Equals(objectName));
            return attnStruct;
        }

        public Dictionary<Tendencies, double> GetLocaleTendency (List<AttnDataStruct> attnStructs, Locales locale)
        {
            Dictionary<Tendencies, double> locationTendencies = new Dictionary<Tendencies, double>();
            // get tendency of location by getting all objs in location then check strongest tendency.
            List<AttnDataStruct> locationStructs = attnStructs.FindAll(attnData => attnData.locale.Equals(locale));

            foreach (var tendency in Enum.GetNames(typeof(Tendencies)))
            {
                double tendencyRating = GetTendencyRating(locationStructs, (Tendencies)Enum.Parse(typeof(Tendencies), tendency));
                locationTendencies.Add((Tendencies)Enum.Parse(typeof(Tendencies), tendency), tendencyRating);
            }
            return locationTendencies;
        }

        public Dictionary<Tendencies, double> GetGlobalTendency (List<AttnDataStruct> attnStructs)
        {
            Dictionary<Tendencies, double> globalTendencies = new Dictionary<Tendencies, double>();
            foreach (var tendency in Enum.GetNames(typeof(Tendencies)))
            {
                globalTendencies.Add((Tendencies)Enum.Parse(typeof(Tendencies), tendency), 0f);
            }
            foreach (var location in Enum.GetNames(typeof(Locales)))
            {
                Dictionary<Tendencies, double> locationTendency = GetLocaleTendency(attnStructs, (Locales)Enum.Parse(typeof(Locales), location));
                globalTendencies = globalTendencies.Concat(locationTendency).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
            }
            return globalTendencies;
        }
    }
}
