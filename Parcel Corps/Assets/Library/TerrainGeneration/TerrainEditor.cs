using System;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Terrain))]
public class TerrainEditor : MonoBehaviourBase
{
    public AnimationCurve PerlinSlope;

    private int _heightResolution;
    private float[,] _mesh;
    private Terrain _terrain;

    [SerializeField][Range(0f, 1f)] private float _noiseMultiplier = .1f;
    [SerializeField][Range(0.0001f, 0.05f)] private float _perlinStretch = 10f;

    protected override void OnEnable()
    {
        base.OnEnable();
        LogDebug("TerrainEditor enabled");
        if (_terrain == null)
        {
            _terrain = GetComponent<Terrain>();
        }

        PullTerrain();
    }
    protected override void Awake()
    {
        LogDebug("TerrainEditor Awake");
        base.Awake();
        
    }

    private void Start()
    {
    }

    public void OnValidate()
    {
        LogDebug("OnValidate called in TerrainEditor");
        PullTerrain();
        RedrawTerrainMesh();
    }

    private void PullTerrain()
    {
        _heightResolution = _terrain.terrainData.heightmapResolution;
        _mesh = _terrain.terrainData.GetHeights(0, 0, _heightResolution, _heightResolution);
        LogDebug($"Terrain height resolution: {_heightResolution}");
    }

    private void RedrawTerrainMesh()
    {
        LogDebug("Redrawing terrain mesh");
        for (var x = 0; x < _heightResolution; x++)
        {
            for (var y = 0; y < _heightResolution; y++)
            {
                _mesh[x, y] = PerlinSlope.Evaluate(
                    Mathf.PerlinNoise(x * _perlinStretch, y * _perlinStretch) * _noiseMultiplier);
            }
        }
        LogDebug("Setting heights on terrain");
    }
}
