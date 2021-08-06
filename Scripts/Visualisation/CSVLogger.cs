using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using SimpleJSON;

namespace ReactiveMedia
{
    public class CSVLogger : MonoBehaviour
    {
        // Maybe better to do this via JSON?
        // Could have hierarchy of Locales + Tendencies with Objects inside them?
        // Global Tendency: RATING, Locales w/ Tendencies + Ratings?, Then Objects?
        // But time-series data might not be possible?
        // Maybe we want to log at each decision point rather than over time?

        private GameObject thePlayer; // Do we want to log position?
        private AttentionDataManager DataMgr;

        // CSV Config
        private StreamWriter csvFile;
        private string deviceID;
        private string userID;
        private long sessionID;

        private void Start()
        {
            thePlayer = GameObject.FindGameObjectWithTag("Player");
            DataMgr = FindObjectOfType<AttentionDataManager>();

            deviceID = SystemInfo.deviceUniqueIdentifier;
            userID = AnalyticsSessionInfo.userId;
            sessionID = AnalyticsSessionInfo.sessionCount;

        // CSV Config
        string directory = $"{Application.dataPath}/logfiles/";
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss").Replace("/", "-").Replace(" ", "_");
            string csvFileName = $"{directory}{dateTime}_{deviceID}_{userID}_{sessionID}.csv";
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            csvFile = File.CreateText(csvFileName);
            // CSV Write Header Text
            string csvHeader = "Object Name, Locale, Tendency, Object Attention Rating, Locale Rating, Global Rating"; // TBC
            csvFile.WriteLine(csvHeader);
        }

        private void FixedUpdate()
        {
            foreach (var obj in DataMgr.attentionObjects)
            {
                // this doesn't work, get the right values...?
                csvFile.WriteLine($"{obj.name}, {obj.locale}, {obj.tendency}, {obj.attentionRating}, {String.Join(", ", DataMgr.GetLocaleTendency(DataMgr.attentionObjects, obj.locale))}, {String.Join(", ", DataMgr.GetGlobalTendency(DataMgr.attentionObjects))}");
            }
        }

        private void JSONTest()
        {
            var beep = @"{
            ""version"": ""1.0"",
            ""data"": {
                ""sampleArray"": [
                    ""string value"",
                    5,
                        {
                        ""name"": ""sub object""
                        }
                    ]
                }
            }";
            JSON.Parse(beep);
        }

        private void DataMgrToJSON()
        {
            var globalTendency = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);

            var JSONString = $@"
                                {{
                                    ""Global Tendency"": ""{globalTendency}"",
                                    ""Locales"": 
                                        {{
                                        }}
                                }}
                              ";
        }

        private void OnDestroy()
        {
            // Close out csv logfile on quit.
            csvFile.Close();
            // Send it over network to logging server?
            SendToLoggingServer();
        }

        private void SendToLoggingServer()
        {
            throw new NotImplementedException();
        }
    }
}
