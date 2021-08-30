using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ReactiveMiseEnScene
{
    [RequireComponent(typeof(Focus))]
    public class FocusMeasures : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;
        public string locale;
        [HideInInspector] public int localeIndex = 0;
        //public List<ReactiveTags.Tendencies> tendencyTags;
        public string tendency;
        [HideInInspector] public int tendencyIndex = 0;

        // Rating Multipliers - Focused is best, otherwise effect of attention, visible, is less.
        [Header("Rating Multipliers")]
        [Range(0.0f, 1.0f)] [SerializeField] private double visibleMultiplier = 0.25f;
        [Range(0.0f, 1.0f)] [SerializeField] private double attendedMultiplier = 0.50f;
        [Range(0.0f, 1.0f)] [SerializeField] private double focusedMultiplier = 1.00f;

        // Lists of interaction events - can arrange to get first, last, longest, shortest, average, total, counts...
        [HideInInspector] public List<double> visibleTimes = new List<double>();
        [HideInInspector] public List<double> attendedTimes = new List<double>();
        [HideInInspector] public List<double> focusedTimes = new List<double>();

        // Count of times in list
        [HideInInspector] public int visibleCount;
        [HideInInspector] public int attendedCount;
        [HideInInspector] public int focusedCount;

        // Sum of all times in list
        [HideInInspector] public double visibleTotalTime;
        [HideInInspector] public double attendedTotalTime;
        [HideInInspector] public double focusedTotalTime;

        // Longest single time in list
        [HideInInspector] public double visibleLongestTime;
        [HideInInspector] public double attendedLongestTime;
        [HideInInspector] public double focusedLongestTime;

        // Compute these from the Time + Value of e.g. Attention (0.33)
        [HideInInspector] public double visibleRating;
        [HideInInspector] public double attendedRating;
        [HideInInspector] public double focusedRating;
        [HideInInspector] public double totalRating;

        private double VisibleStart;
        private double VisibleEnd;

        private double AttendedStart;
        private double AttendedEnd;

        private double FocusedStart;
        private double FocusedEnd;

        // Compare below - scriptableobj or singleton monobehaviour best?
        //[HideInInspector] public UnityEvent<GameObject> WriteToAttnDataStore;
        // singleton call below?
        [HideInInspector] public UnityEvent<FocusDataStruct> WriteAttnDataMgr;

        // Start is called before the first frame update
        void Start()
        {
            Focus focus = GetComponent<Focus>();
            if (focus)
            {
                focus.EnterFocus.AddListener(StartVisibleMeasure);
                //focus.StayVisible.AddListener();
                focus.ExitVisible.AddListener(EndVisibleMeasure);

                focus.EnterAttention.AddListener(StartAttentionMeasure);
                //focus.StayAttention.AddListener();
                focus.ExitAttention.AddListener(EndAttentionMeasure);

                focus.EnterFocus.AddListener(StartFocusMeasure);
                //focus.StayFocus.AddListener();
                focus.ExitFocus.AddListener(EndFocusMeasure);
            }
            else { Debug.LogWarning("No Focus Component on this GameObject"); }

            if(FindObjectOfType<ReactiveMesDataManager>()) 
            {
                ReactiveMesDataManager AttnMgr = FindObjectOfType<ReactiveMesDataManager>();
                WriteAttnDataMgr.AddListener(AttnMgr.ParseInboundStructData);
            }
            else
            {
                print("Attention Data Manager not found. Have you added the prefab to the scene?");
            }
            
        }

        void FixedUpdate()
        {
            SendAttnStructData();
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            // implement debug / visualisation logic here?
            Handles.Label(transform.position, $"{locale}\n{tendency}\n{totalRating}");
#endif
        }

        private void StartVisibleMeasure(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
            {
                VisibleStart = Time.timeAsDouble;
            }
        }

        private void EndVisibleMeasure(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
            {
                VisibleEnd = Time.timeAsDouble;
                double visibleTime = VisibleEnd - VisibleStart;
                visibleTimes.Add(visibleTime);
                visibleCount = visibleTimes.Count;
                // per: https://stackoverflow.com/questions/4703046/sum-of-timespans-in-c-sharp
                visibleTotalTime = visibleTimes.Sum();
                visibleLongestTime = visibleTimes.Max();
                // check this logic...
                visibleRating = visibleTotalTime * visibleMultiplier;
                // is this the best place for this?
                totalRating = visibleRating + attendedRating + focusedRating;
            }
        }

        private void StartAttentionMeasure(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
            {
                AttendedStart = Time.timeAsDouble;
            }
        }
        private void EndAttentionMeasure(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
            {
                AttendedEnd = Time.timeAsDouble;
                double attendTime = AttendedEnd - AttendedStart;
                attendedTimes.Add(attendTime);
                attendedCount = attendedTimes.Count;
                attendedTotalTime = attendedTimes.Sum();
                attendedLongestTime = attendedTimes.Max();
                attendedRating = attendedTotalTime * attendedMultiplier;
            }
        }

        private void StartFocusMeasure(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
            {
                FocusedStart = Time.timeAsDouble;
            }
        }

        private void EndFocusMeasure(GameObject gameObject)
        {
            if (gameObject == this.gameObject)
            {
                FocusedEnd = Time.timeAsDouble;
                double focusTime = FocusedEnd - FocusedStart;
                focusedTimes.Add(focusTime);
                focusedCount = focusedTimes.Count;
                focusedTotalTime = focusedTimes.Sum();
                focusedLongestTime = focusedTimes.Max();
                focusedRating = focusedTotalTime * focusedMultiplier;
            }
        }

        private void SendAttnStructData ()
        {
            // Write data out to the data container/manager.
            // WriteToAttnDataStore.Invoke(gameObject);
            FocusDataStruct focusData;
            focusData.name = name;
            focusData.locale = locale;
            focusData.tendency = tendency;
            focusData.attentionRating = totalRating;
            WriteAttnDataMgr.Invoke(focusData);
        }

        private void OnDisable()
        {
            SendAttnStructData();
        }
    }
}