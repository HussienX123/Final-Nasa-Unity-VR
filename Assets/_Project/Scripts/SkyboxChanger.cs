using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField] private Material[] skyboxes; // Assign skybox materials in Inspector
    private int currentIndex = 0;

    void Start()
    {
        if (skyboxes.Length > 0)
            RenderSettings.skybox = skyboxes[0]; // set default skybox
    }

    void Update()
    {
        // Example: Press space to change skybox
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeSkybox();
        }
    }

    [Button("Change Skybox")]
    public void ChangeSkybox()
    {
        currentIndex = (currentIndex + 1) % skyboxes.Length;
        RenderSettings.skybox = skyboxes[currentIndex];

        // If you want immediate update for skybox lighting
        DynamicGI.UpdateEnvironment();
    }
}
