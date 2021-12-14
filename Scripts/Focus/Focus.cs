using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ReactiveMiseEnScene
{
    public enum FocusLevel
    {
        offscreen,
        onscreen,
        attended,
        focused
    }

    public class Focus : MonoBehaviour
    {
        // Focus Levels
        [HideInInspector] public FocusLevel currentFocusLevel;
        [HideInInspector] public FocusLevel lastFocusLevel;

        // Focus Level Thresholds
        [Header("Thresholds")]
        // Set max distance object can be interacted with by camera.
        [Range(0.0f, 1000.0f)] [SerializeField] private double distanceThreshold = 50.0f;
        // Viewport thresholds from 0f to 1f - not ideal but better than screen res thresholds, more consistent.
        [Range(0.0f, 1.0f)] [SerializeField] private double focusedThreshold = 0.1f;
        [Range(0.0f, 1.0f)] [SerializeField] private double attendedThreshold = 0.25f;

        // Focus Level Events: This Object, it's current focus level, bool of true if same as last?
        [Header("Events")]
        public UnityEvent<GameObject, FocusLevel, FocusLevel> FocusLevelChange;
        public UnityEvent<GameObject, FocusLevel> FocusLevelStay;

        [Tooltip("Specify a camera if issues with focus occur, e.g. if using multiple cams.")]
        public Camera cam;
        private List<Collider> childColliders;
        private Bounds meshBounds;

        void Start()
        {
            CamSetup();
            meshBounds = GetCombinedRendererBounds();
            childColliders = GetComponentsInChildren<Collider>().ToList();
            if(GetComponent<Collider>() != null) childColliders.Add(GetComponent<Collider>());
        }

        private Bounds GetCombinedRendererBounds()
        {
            List<Renderer> rr = gameObject.GetComponentsInChildren<Renderer>().ToList();
            if (GetComponent<Renderer>() != null) rr.Add(GetComponent<Renderer>());
            Bounds combinedBounds = rr[0].bounds;
            foreach (Renderer r in rr) combinedBounds.Encapsulate(r.bounds);
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

        void Update()
        {
            meshBounds = GetCombinedRendererBounds(); // Needs to be updated to handle movement.
            GetFocusLevel();
        }

        private bool ObjectFrustrumCheck()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, meshBounds)) return true;
            else return false;
        }

        private bool ObjectLineOfSightCheck()
        {
            if (Physics.Linecast(cam.transform.position, meshBounds.center, out RaycastHit hit, 1 << 0, QueryTriggerInteraction.Ignore) // Physics.AllLayers replaced with 1 << 10 for focus? 1 << 0 = default
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

        private void GetFocusLevel()
        {
            lastFocusLevel = currentFocusLevel;
            if(ObjectFrustrumCheck() && ObjectLineOfSightCheck() && ObjectDistanceCheck())
            {
                float objectScreenCentreDistance = GetObjectScreenPosition();
                switch (objectScreenCentreDistance)
                {
                    case var _ when objectScreenCentreDistance < focusedThreshold:
                        currentFocusLevel = FocusLevel.focused;
                        break;
                    case var _ when objectScreenCentreDistance >= focusedThreshold && objectScreenCentreDistance < attendedThreshold:
                        currentFocusLevel = FocusLevel.attended;
                        break;
                    case var _ when objectScreenCentreDistance >= attendedThreshold:
                        currentFocusLevel = FocusLevel.onscreen;
                        break;
                    default:
                        currentFocusLevel = FocusLevel.offscreen;
                        break;
                }
            }
            else currentFocusLevel = FocusLevel.offscreen;

            if (currentFocusLevel == lastFocusLevel) FocusLevelStay.Invoke(gameObject, currentFocusLevel);
            else FocusLevelChange.Invoke(gameObject, currentFocusLevel, lastFocusLevel);
        }

        private float GetObjectScreenPosition()
        {
            // TODO: compare doing this inversely, i.e. viewporttoworldpoint? - might have better results?
            Vector2 gameObjectViewportPosition = cam.WorldToViewportPoint(meshBounds.center);
            Vector2 viewportCentre = new Vector2(0.5f, 0.5f);
            float dist = Vector2.Distance(gameObjectViewportPosition, viewportCentre);
            return dist;
        }

        private void OnDisable()
        {
            // Event Cleanup?
            currentFocusLevel = FocusLevel.offscreen;
            FocusLevelChange.Invoke(gameObject, currentFocusLevel, lastFocusLevel);
        }
    }
}
