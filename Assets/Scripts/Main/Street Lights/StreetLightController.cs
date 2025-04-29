using UnityEngine;
using System.Collections.Generic; // Required for using Lists

/// <summary>
/// Controls a list of street lights, turning them on/off based on the
/// rotation of a designated Sun object (Directional Light).
/// Optionally includes fade-in and flickering effects.
/// </summary>
public class StreetLightController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the Directional Light representing the Sun here.")]
    public Transform sunTransform;

    [Tooltip("Assign the parent GameObject containing all the street Spot Lights.")]
    public GameObject streetLightParent;

    // List to hold references to the actual Light components
    private List<Light> streetLights = new List<Light>();

    [Header("Activation Settings")]
    [Tooltip("The threshold for the Sun's forward vector's Y component. When below this value (sun pointing downwards/below horizon), lights turn on.")]
    public float activationThresholdY = 0f; // Sun pointing level or down activates lights

    [Tooltip("Check the sun's rotation relative to World space? If false, checks local space (less common for sun).")]
    public bool useWorldSpaceForward = true;

    [Header("Light Effect Settings")]
    [Tooltip("The target intensity for the street lights when they are on.")]
    public float targetIntensity = 10f;

    [Tooltip("How long (in seconds) it takes for the lights to fade in to full intensity. Set to 0 for instant on.")]
    public float fadeInDuration = 2.0f;

    [Tooltip("Enable a flickering effect after the lights have faded in.")]
    public bool enableFlicker = true;

    [Tooltip("Minimum intensity during flicker.")]
    public float flickerMinIntensity = 8f;

    [Tooltip("Maximum intensity during flicker.")]
    public float flickerMaxIntensity = 12f;

    [Tooltip("How quickly the light flickers.")]
    public float flickerSpeed = 15f;

    // Internal state variables
    private bool lightsAreOn = false;
    private float currentFadeTime = 0f;
    // Store unique seeds for Perlin noise per light for varied flickering
    private List<float> flickerSeeds = new List<float>();

    void Awake()
    {
        // --- Find and Store Lights ---
        if (streetLightParent != null)
        {
            // Get all Light components in the children of the specified parent
            streetLights.AddRange(streetLightParent.GetComponentsInChildren<Light>());
        }
        else
        {
            Debug.LogWarning("Street Light Parent is not assigned!", this);
        }

        if (streetLights.Count == 0)
        {
            Debug.LogWarning("No Light components found under the Street Light Parent!", this);
        }
        else
        {
            // Initialize lights to be off and generate flicker seeds
            foreach (Light light in streetLights)
            {
                if (light != null)
                {
                    light.enabled = false;
                    light.intensity = 0;
                    // Add a unique random seed for each light's flicker
                    flickerSeeds.Add(Random.Range(0f, 100f));
                }
            }
        }

        // --- Initial Check ---
        if (sunTransform == null)
        {
            Debug.LogError("Sun Transform is not assigned!", this);
            enabled = false; // Disable this script if sun isn't set
            return;
        }

        // Set initial state based on starting sun position
        CheckSunPositionAndUpdateLights(true); // Force update on awake
    }

    void Update()
    {
        if (sunTransform == null || streetLights.Count == 0)
        {
            return; // Don't do anything if references are missing
        }

        CheckSunPositionAndUpdateLights(false); // Normal update check

        // Handle intensity fading and flickering *if* lights are supposed to be on
        if (lightsAreOn)
        {
            HandleLightEffects();
        }
    }

    /// <summary>
    /// Checks the sun's orientation and decides if lights should be on or off.
    /// </summary>
    /// <param name="forceUpdate">If true, forces lights to update state even if condition hasn't changed.</param>
    void CheckSunPositionAndUpdateLights(bool forceUpdate)
    {
        // Get the relevant forward direction of the sun
        Vector3 sunForward = useWorldSpaceForward ? sunTransform.forward : sunTransform.localPosition.normalized; // Use local if not world

        // Check if the sun is below the horizon threshold
        bool shouldBeOn = sunForward.y > activationThresholdY;

        // State Change Detection: Check if the desired state is different from the current state
        if (shouldBeOn && (!lightsAreOn || forceUpdate))
        {
            // --- Turn Lights ON ---
            if (!lightsAreOn) // Only reset fade time if it's a new activation
            {
                currentFadeTime = 0f;
            }
            lightsAreOn = true;
            foreach (Light light in streetLights)
            {
                if (light != null) light.enabled = true;
            }
            // Intensity is handled in HandleLightEffects
        }
        else if (!shouldBeOn && (lightsAreOn || forceUpdate))
        {
            // --- Turn Lights OFF ---
            lightsAreOn = false;
            currentFadeTime = 0f; // Reset fade time
            foreach (Light light in streetLights)
            {
                if (light != null)
                {
                    light.intensity = 0f; // Turn off intensity immediately
                    light.enabled = false; // Disable component
                }
            }
        }
    }

    /// <summary>
    /// Manages the fade-in and flickering effects for the lights when they are active.
    /// </summary>
    void HandleLightEffects()
    {
        // --- Fade In Logic ---
        if (fadeInDuration > 0 && currentFadeTime < fadeInDuration)
        {
            currentFadeTime += Time.deltaTime;
            float fadeRatio = Mathf.Clamp01(currentFadeTime / fadeInDuration);
            float currentIntensity = Mathf.Lerp(0f, targetIntensity, fadeRatio);

            foreach (Light light in streetLights)
            {
                if (light != null) light.intensity = currentIntensity;
            }
        }
        // --- Post-Fade Logic (Steady or Flicker) ---
        else
        {
            if (enableFlicker)
            {
                // --- Flicker Logic ---
                for (int i = 0; i < streetLights.Count; i++)
                {
                    Light light = streetLights[i];
                    if (light != null)
                    {
                        // Use Perlin noise for a smoother, more natural flicker
                        // Incorporate the unique seed for variation between lights
                        float noise = Mathf.PerlinNoise(flickerSeeds[i] + Time.time * flickerSpeed, flickerSeeds[i]);
                        light.intensity = Mathf.Lerp(flickerMinIntensity, flickerMaxIntensity, noise);
                    }
                }
            }
            else
            {
                // --- Steady Intensity Logic ---
                // Ensure all lights are at target intensity if flicker is off and fade is done
                foreach (Light light in streetLights)
                {
                    if (light != null && light.intensity != targetIntensity)
                    {
                        light.intensity = targetIntensity;
                    }
                }
            }
        }
    }
}