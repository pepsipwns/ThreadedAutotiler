using Godot;
using System;

/// <summary>
/// This is a NoiseHandler singleton that should be added to autoload.
/// Its a basic noise handler that uses FastNoiseLite to generate noise.
/// </summary>
public partial class NoiseHandler : Node
{
    public FastNoiseLite Noise;
    public static NoiseHandler Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        GenerateNoise();
    }

    public void GenerateNoise()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        Noise = new FastNoiseLite
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Seed = 1000,
            Frequency = 0.014f
        };
    }

    public float GetNoise(float x, float y)
    {
        return Noise.GetNoise2D(x, y);
    }
}
