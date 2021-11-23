using System;
using System.Collections;
using System.Collections.Generic;
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
        [HideInInspector] public int localeIndex = 0; // for custom editor
        public string tendency;
        [HideInInspector] public int tendencyIndex = 0; // for custom editor

        private FocusLevel focusLevel;

        // Rating Multipliers - Focused is best, otherwise effect of attention, visible, is less.
        [Header("Rating Multipliers")]
        [Range(0.0f, 1.0f)] [SerializeField] private double onscreenMultiplier = 0.25f;
        [Range(0.0f, 1.0f)] [SerializeField] private double attendedMultiplier = 0.50f;
        [Range(0.0f, 1.0f)] [SerializeField] private double focusedMultiplier = 1.00f;

        [HideInInspector] private double onscreenStart;
        [HideInInspector] private double onscreenEnd;
        [HideInInspector] private double attendedStart;
        [HideInInspector] private double attendedEnd;
        [HideInInspector] private double focusedStart;
        [HideInInspector] private double focusedEnd;

        [HideInInspector] public double onscreenTime = 0f;
        [HideInInspector] public double attendedTime = 0f;
        [HideInInspector] public double focusedTime = 0f;
        [HideInInspector] public double totalTime = 0f;

        [HideInInspector] public double onscreenRating = 0f;
        [HideInInspector] public double attendedRating = 0f;
        [HideInInspector] public double focusedRating = 0f;
        [HideInInspector] public double totalRating = 0f;

        [HideInInspector] public UnityEvent<FocusDataStruct> WriteAttnDataMgr;

        // Start is called before the first frame update
        void Start()
        {
            Focus focus = GetComponent<Focus>();
            if (focus)
            {
                focus.FocusLevelChange.AddListener(MeasuringHandler);
            }
            else Debug.LogWarning("No Focus Component on this GameObject");

            if (FindObjectOfType<ReactiveMesDataManager>())
            {
                ReactiveMesDataManager AttnMgr = FindObjectOfType<ReactiveMesDataManager>();
                WriteAttnDataMgr.AddListener(AttnMgr.ParseInboundStructData);
            }
            else print("Attention Data Manager not found. Have you added the prefab to the scene?");
        }

        private void FixedUpdate()
        {
            SendFocusDataStruct();
        }

        private void OnDisable()
        {
            SendFocusDataStruct();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            // implement debug / visualisation logic here?
            Handles.Label(transform.position, $"{locale}\n{tendency}\n{totalRating}");
        }
#endif

        private void SendFocusDataStruct()
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

        private void MeasuringHandler(GameObject obj, FocusLevel currentFocusLevel, FocusLevel lastFocusLevel)
        {

            if (obj == gameObject)
            {


                var timeNow = Time.timeAsDouble;
                switch (currentFocusLevel)
                {
                    case FocusLevel.offscreen:
                        if (lastFocusLevel == FocusLevel.onscreen)
                        {
                            onscreenEnd = timeNow;
                            onscreenTime += onscreenEnd - onscreenStart;
                        }
                        break;
                    case FocusLevel.onscreen:
                        onscreenStart = timeNow;
                        if (lastFocusLevel == FocusLevel.attended)
                        {
                            attendedEnd = timeNow;
                            attendedTime += attendedEnd - attendedStart;
                        }
                        break;
                    case FocusLevel.attended:
                        attendedStart = timeNow;
                        if (lastFocusLevel == FocusLevel.onscreen)
                        {
                            onscreenEnd = timeNow;
                            onscreenTime += onscreenEnd - onscreenStart;
                        }
                        if (lastFocusLevel == FocusLevel.focused)
                        {
                            focusedEnd = timeNow;
                            focusedTime += focusedEnd - focusedStart;
                        }
                        break;
                    case FocusLevel.focused:
                        focusedStart = timeNow;
                        if (lastFocusLevel == FocusLevel.attended)
                        {
                            attendedEnd = timeNow;
                            attendedTime += attendedEnd - attendedStart;
                        }
                        break;
                    default:
                        break;
                }
                totalTime = onscreenTime + attendedTime + focusedTime;
                onscreenRating = onscreenTime * onscreenMultiplier;
                attendedRating = attendedTime * attendedMultiplier;
                focusedRating = focusedTime * focusedMultiplier;
                totalRating = onscreenRating + attendedRating + focusedRating;
            }
        }
    }
}
