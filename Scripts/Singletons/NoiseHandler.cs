using Godot;
using System;

public partial class NoiseHandler : Node
{
    public FastNoiseLite Noise;
    public static NoiseHandler Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        GenerateNoise();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

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
