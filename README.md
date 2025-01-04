---

# Atomica

Atomica is a particle simulation engine designed to create fluid, dynamic, and visually stunning particle effects. It allows for highly customizable particle systems with rainbow color transitions, realistic physics simulations, and smooth, organic movement. This project leverages C# and OpenTK to create interactive and responsive particle systems, perfect for games or any graphics application.

## Features

- **Rainbow Particle Colors:** Smooth transitions between rainbow colors based on particle lifespan.
- **Fluid Motion:** Realistic particle movement with gravity, drag, and randomized sway for fluid-like effects.
- **Customizable Particle Properties:** Control lifespan, size, opacity, speed, and more.
- **Texture-Based Particles:** Use custom textures for each particle for greater flexibility in effects.
- **Efficient Rendering:** Optimized for performance with the ability to update only visible particles.

## Getting Started

### Prerequisites

- .NET 6.0 or later
- OpenTK 4.9.3 (for graphics rendering and game loop management)

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/Atomica.git
    cd Atomica
    ```

2. Install the necessary dependencies using your preferred package manager (e.g., NuGet).

    ```bash
    dotnet restore
    ```

3. Run the project:

    ```bash
    dotnet run
    ```

### Customizing Particles

The `ParticleData` struct contains properties that can be easily adjusted to modify the appearance and behavior of particles. You can control the start and end colors, lifespan, opacity, size, and more.

```csharp
public struct ParticleData
{
    public float lifespan = 2f;
    public Color colorStart = Color.Red;
    public Color colorEnd = Color.Purple;
    public float opacityStart = 1f;
    public float opacityEnd = 0f;
    public float sizeStart = 32f;
    public float sizeEnd = 4f;
    public float speed = 100f;
    public float angle = 0f;
}
```

You can create custom particles using the `Particle` class by providing a position and `ParticleData`.

```csharp
ParticleData data = new ParticleData
{
    colorStart = Color.Blue,
    colorEnd = Color.Green,
    lifespan = 3f,
    speed = 150f
};
Particle particle = new Particle(new Vector2(100, 100), data);
```

### Controlling Particle Effects

In the `Particle` class, particles are updated based on their lifespan and interact with the environment using gravity and drag effects.

```csharp
public void Update()
{
    _lifespanLeft -= Globals.TotalSeconds;
    if (_lifespanLeft <= 0f)
    {
        isFinished = true;
        return;
    }

    // Apply gravity, drag, and smooth color transitions
    _velocity.Y += 9.8f * Globals.TotalSeconds;
    _velocity *= _dragFactor;
    _position += (_velocity + new Vector2(sway, 0)) * Globals.TotalSeconds;

    // Update opacity, scale, and position
    _opacity = MathHelper.Clamp(MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount), 0, 1);
    _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;
}
```

## Contributing

Feel free to fork this repository and submit pull requests. Any improvements, bug fixes, or suggestions are welcome!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---
