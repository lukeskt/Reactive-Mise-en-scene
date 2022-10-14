using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ReactiveMiseEnScene
{
    public class AttentionTracker : MonoBehaviour
    {
        // Data Sending for Attention Data Manager
        public RemesSettings RMSettings;
        public string locale;
        [HideInInspector] public int localeIndex = 0; // for custom editor
        public string tendency;
        [HideInInspector] public int tendencyIndex = 0; // for custom editor
        [HideInInspector] public UnityEvent<FocusDataStruct> WriteAttnDataMgr;

        // Focus values
        private float focusValue;
        [HideInInspector] public float getFocusValue { get => focusValue; }
        private float cumulativeFocusValue = 0f;
        [HideInInspector] public float getCumulativeFocusValue { get => cumulativeFocusValue; }

        // Set max distance object can be interacted with by camera.
        [Tooltip("Set the distance after which the object will not be considered on-screen.")]
        [Range(0.0f, 1000.0f)][SerializeField] private double distanceThreshold = 50.0f;
        [Tooltip("Specify a camera if issues with focus occur, e.g. if using multiple cams.")]
        public Camera cam;

        private List<Collider> childColliders;
        private List<Renderer> childRenderers;
        private Bounds meshBounds;

        void Start()
        {
            CamSetup();
            meshBounds = GetCombinedRendererBounds();
            childColliders = GetComponentsInChildren<Collider>().ToList();
            if (GetComponent<Collider>() != null) childColliders.Add(GetComponent<Collider>());

            if (FindObjectOfType<RemesDataManager>())
            {
                RemesDataManager AttnMgr = FindObjectOfType<RemesDataManager>();
                WriteAttnDataMgr.AddListener(AttnMgr.ParseInboundStructData);
            }
            else print("Attention Data Manager not found. Have you added the prefab to the scene?");
        }

        private Bounds GetCombinedRendererBounds()
        {
            childRenderers = gameObject.GetComponentsInChildren<Renderer>().ToList();
            if (GetComponent<Renderer>() != null) childRenderers.Add(GetComponent<Renderer>());
            Bounds combinedBounds = childRenderers[0].bounds;
            foreach (Renderer r in childRenderers) combinedBounds.Encapsulate(r.bounds);
            return combinedBounds;
        }

        private void CamSetup()
        {
            if (!cam)
            {
                GameObject Viewer = GameObject.FindGameObjectWithTag("Player");
                if (Viewer)
                {
                    if (Viewer.GetComponent<Camera>()) cam = Viewer.GetComponent<Camera>();
                    else if (Viewer.GetComponentInChildren<Camera>()) cam = Viewer.GetComponentInChildren<Camera>();
                    else if (Viewer.GetComponentInParent<Camera>()) cam = Viewer.GetComponentInParent<Camera>();
                }
                else cam = Camera.main;
            }
        }

        void FixedUpdate()
        {
            // This needs to run in an update loop to handle object movement. Possible perf issues?
            meshBounds = GetCombinedRendererBounds();

            focusValue = GetFocusValue();
            // TODO: bit of a magic number multiplier below to reduce the cumulative value to something less ridiculous - better way to do this?
            cumulativeFocusValue += focusValue * 0.01f;

            SendAttentionData();
        }

        private void OnDisable()
        {
            SendAttentionData();
        }

        private bool ObjectFrustrumCheck()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, meshBounds)) return true;
            else return false;
        }

        private bool ObjectLineOfSightCheck()
        {
            if (Physics.Linecast(cam.transform.position, meshBounds.center, out RaycastHit hit, 1 << gameObject.layer, QueryTriggerInteraction.Ignore)
                && childColliders.Contains(hit.collider))
            {
                Debug.DrawLine(cam.transform.position, meshBounds.center, Color.green);
                return true;
            }
            else
            {
                Debug.DrawLine(cam.transform.position, meshBounds.center, Color.red);
                return false;
            }
        }

        private bool ObjectDistanceCheck()
        {
            if (Vector3.Distance(meshBounds.center, cam.transform.position) < distanceThreshold) return true;
            else return false;
        }

        private float GetFocusValue()
        {
            if (ObjectFrustrumCheck() && ObjectLineOfSightCheck() && ObjectDistanceCheck())
            {
                // TODO: check these magic numbers for mapping here - mostly seem okay but is there a better way?
                float getFocusValue = MapPositionToLinear(GetObjectScreenPosition(), 2, 0, -1f, 1);
                if (getFocusValue > 0) focusValue = getFocusValue;
                else focusValue = 0;
            }
            else
            {
                focusValue = 0f;
            }
            return focusValue;
        }

        private float GetObjectScreenPosition()
        {
            // TODO: compare doing this inversely, i.e. viewporttoworldpoint? - might have better results?
            Vector2 gameObjectViewportPosition = cam.WorldToViewportPoint(meshBounds.center);
            Vector2 viewportCentre = new Vector2(0.5f, 0.5f);
            float dist = Vector2.Distance(gameObjectViewportPosition, viewportCentre);
            return dist;
        }

        private float MapPositionToLinear(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = inputValue - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var mappedValue = toAbs + toMin;

            mappedValue = Mathf.Clamp(mappedValue, toMin, toMax);
            return mappedValue;
        }

        private void SendAttentionData()
        {
            FocusDataStruct attentionData;
            attentionData.name = name;
            attentionData.locale = locale;
            attentionData.tendency = tendency;
            attentionData.attentionRating = cumulativeFocusValue;
            WriteAttnDataMgr.Invoke(attentionData);
        }

#if UNITY_EDITOR
    void OnDrawGizmos()
        {
            // implement debug / visualisation logic here?
            Handles.Label(transform.position, $"{locale}\n{tendency}\n{cumulativeFocusValue}");
        }
#endif
    }
}
