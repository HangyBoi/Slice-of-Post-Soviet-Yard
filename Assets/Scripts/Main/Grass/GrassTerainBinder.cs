using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class GrassTerainBinder : MonoBehaviour
{
    [Tooltip("The Terrain object to sample data from.")]
    public Terrain targetTerrain;

    [Tooltip("The Renderer component of your grass object/prefab. If using Terrain's Paint Details, this might need adjustment.")]
    public Renderer grassRenderer;

    [Tooltip("Alternatively, assign the Grass Material directly if not targeting a specific Renderer.")]
    public Material grassMaterial;

    [Header("Shader Property Names")]
    // These should EXACTLY match the Reference names in the Shader Graph
    public string terrainPositionName = "_V0_Terrain_Position";
    public string terrainSizeName = "_V0_Terrain_Size";
    public string splatMapName = "_V0_Terrain_Splat_Map";
    public string layerAlbedoPrefix = "_V0_Terrain_Layer_Albedo"; // e.g., Terrain Layer Albedo0, Terrain Layer Albedo1
    public string layerTilingPrefix = "_V0_Terrain_Layer_Tiling";   // e.g., Terrain Layer Tiling0, Terrain Layer Tiling1

    private MaterialPropertyBlock matPropertyBlock; // More efficient for many instances

    private void Start()
    {
        // Initialize the Property Block if targeting a specific renderer

        if (grassRenderer != null)
        {
            matPropertyBlock = new MaterialPropertyBlock();
        }

        UpdateTerrainData();
    }

    // Call this function if the terrain textures/layers change during gameplay
    public void UpdateTerrainData()
    {
        if (targetTerrain == null)
        {
            Debug.LogError("GrassTerrainBinder: Target Terrain is not assigned!");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        if (terrainData == null)
        {
            Debug.LogError("GrassTerrainBinder: Target Terrain has no TerrainData!");
            return;
        }

        // Get the material to modify
        Material materialToSet = null;
        if (grassRenderer != null)
        {
            // Option 1: Use Material Property Block (Good for instanced materials on many renderers)
            // Make sure your grass shader supports GPU Instancing
            grassRenderer.GetPropertyBlock(matPropertyBlock);

            // Option 2: Modify the specific instance of the material (creates an instance if not already)
            // materialToSet = grassRenderer.material;

            // Option 3: Modify the shared material (affects ALL objects using this material)
            // materialToSet = grassRenderer.sharedMaterial;
        }
        else if (grassMaterial != null)
        {
            // Use the directly assigned material asset
            materialToSet = grassMaterial;
        }

        if (materialToSet == null && matPropertyBlock == null)
        {
            Debug.LogError("GrassTerrainBinder: No valid grass Renderer or Material assigned!");
            return;
        }

        // 1. Get Terrain Position and Size
        Vector3 terrainPosition = targetTerrain.transform.position;
        Vector3 terrainSize = terrainData.size;

        // 2. Get Splat Map (Alphamap)
        // NOTE: Unity terrains can have multiple alphamap textures if > 4 layers.
        // We assume <= 4 layers and use the first alphamap.
        Texture2D splatMap = terrainData.alphamapTextures.Length > 0 ? terrainData.alphamapTextures[0] : null;

        // 3. Get Terrain Layers (Textures and Tiling)
        TerrainLayer[] layers = terrainData.terrainLayers;

        // --- Set Properties ---
        if (matPropertyBlock != null) // Using Property Block
        {
            matPropertyBlock.SetVector(terrainPositionName, terrainPosition);
            matPropertyBlock.SetVector(terrainSizeName, terrainSize);
            if (splatMap != null) matPropertyBlock.SetTexture(splatMapName, splatMap);


            for (int i = 0; i < layers.Length; i++)
            {
                // Limit to 4 layers for this basic example matching typical splatmap RGBA
                if (i >= 4) break;

                if (layers[i] != null && layers[i].diffuseTexture != null)
                {
                    matPropertyBlock.SetTexture(layerAlbedoPrefix + i, layers[i].diffuseTexture);

                    // Pass tiling as Vector4 (X=TileX, Y=TileY, Z=Unused, W=Unused) for simplicity in shader graph
                    matPropertyBlock.SetVector(layerTilingPrefix + i, new Vector4(layers[i].tileSize.x, layers[i].tileSize.y, 0, 0));
                }
                else // Handle missing layers gracefully - Set to a default texture and reset tiling
                {
                    matPropertyBlock.SetTexture(layerAlbedoPrefix + i, Texture2D.blackTexture);
                    materialToSet.SetVector(layerTilingPrefix + i, Vector4.zero);
                }
            }

            grassRenderer.SetPropertyBlock(matPropertyBlock);

        }
        else if (materialToSet != null) // Modifying Material directly
        {
            materialToSet.SetVector(terrainPositionName, terrainPosition);
            materialToSet.SetVector(terrainSizeName, terrainSize);
            if (splatMap != null) materialToSet.SetTexture(splatMapName, splatMap);

            for (int i = 0; i < layers.Length; i++)
            {
                if (i >= 4) break; // Limit to 4 layers

                if (layers[i] != null && layers[i].diffuseTexture != null)
                {
                    materialToSet.SetTexture(layerAlbedoPrefix + i, layers[i].diffuseTexture);
                    materialToSet.SetVector(layerTilingPrefix + i, new Vector4(layers[i].tileSize.x, layers[i].tileSize.y, 0, 0));
                }
                else // Handle missing layers gracefully - Set to a default texture and reset tiling
                {
                    materialToSet.SetTexture(layerAlbedoPrefix + i, Texture2D.blackTexture);
                    materialToSet.SetVector(layerTilingPrefix + i, Vector4.zero);
                }
            }
        }

#if UNITY_EDITOR
        // Helpful for seeing updates in Edit mode if using ExecuteAlways
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(gameObject); // Or the material asset if modifying that
            if (grassRenderer) UnityEditor.SceneView.RepaintAll();
        }
#endif

        Debug.Log("Terrain data sent to grass material/block.");
    }


}
