using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;

public class Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public int Type;

    public Particle(Vector2 position, int type)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
        Type = type;
    }

    public void Update()
    {
        Velocity += Acceleration;
        Position += Velocity;
        Acceleration = Vector2.Zero;
    }
}

public class ParticleSimulator : GameWindow
{
    private List<Particle> particles;
    private const int ParticleCount = 500;
    private const int TypeCount = 3;
    private readonly float[,] InteractionMatrix = new float[TypeCount, TypeCount]
    {
        { 0.0f, 0.5f, -0.2f },
        { 0.5f, 0.0f, 0.3f },
        { -0.2f, 0.3f, 0.0f }
    };

    public ParticleSimulator() : base(GameWindowSettings.Default, new NativeWindowSettings
    {
        ClientSize = new Vector2i(800, 600),
        Title = "Atomica"
    })
    {
        particles = new List<Particle>();
        var random = new Random();

        for (int i = 0; i < ParticleCount; i++)
        {
            var position = new Vector2(
                (float)random.NextDouble() * 2 - 1,
                (float)random.NextDouble() * 2 - 1
            );
            int type = random.Next(TypeCount);
            particles.Add(new Particle(position, type));
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color4.Black);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        ApplyForces(particles);
        foreach (var particle in particles)
        {
            particle.Update();
            Render(particle);
        }

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(-1, 1, -1, 1, -1, 1);
        GL.MatrixMode(MatrixMode.Modelview);
    }

    private void ApplyForces(List<Particle> particles)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            for (int j = i + 1; j < particles.Count; j++) // Fixed condition here
            {
                var p1 = particles[i];
                var p2 = particles[j];

                Vector2 direction = p2.Position - p1.Position;
                float distance = direction.Length;

                if (distance < 0.05f || distance > 0.3f) continue;

                // Safeguard for zero-length vectors
                if (distance > 0.0f) direction.Normalize();

                float forceMagnitude = InteractionMatrix[p1.Type, p2.Type] / distance;

                Vector2 force = direction * forceMagnitude;

                p1.Acceleration += force;
                p2.Acceleration -= force;
            }
        }
    }

    private void Render(Particle particle)
    {
        GL.PointSize(2.0f); // Adjust size for visibility
        GL.Begin(PrimitiveType.Points);

        switch (particle.Type)
        {
            case 0: GL.Color3(1.0, 0.0, 0.0); break; // Red
            case 1: GL.Color3(0.0, 1.0, 0.0); break; // Green
            case 2: GL.Color3(0.0, 0.0, 1.0); break; // Blue
        }

        GL.Vertex2(particle.Position);
        GL.End();
    }

    public static void Main()
    {
        using (var simulator = new ParticleSimulator())
        {
            simulator.Run();
        }
    }
}
