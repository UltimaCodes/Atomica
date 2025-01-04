using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Program
{
    static void Main()
    {
        var nativeSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(800, 600),
            Title = "Newtonian Particle Simulator"
        };

        using var window = new ParticleSimulator(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }
}

class Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Mass;

    public Particle(Vector2 position, Vector2 velocity, float mass)
    {
        Position = position;
        Velocity = velocity;
        Mass = mass;
    }
}

class ParticleSimulator : GameWindow
{
    private readonly List<Particle> _particles = new();
    private int _vao, _vbo;
    private Shader _shader;

    private float _gravitationalConstant = 1f;
    private float _timeStep = 0.016f;

    public ParticleSimulator(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings)
        : base(gameSettings, nativeSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

        // Dynamically generate GLSL shader code
        string vertexShaderCode = @"
        #version 330 core
        layout(location = 0) in vec2 position;
        void main()
        {
            gl_Position = vec4(position, 0.0, 1.0);
        }
        ";

        string fragmentShaderCode = @"
        #version 330 core
        out vec4 color;
        void main()
        {
            color = vec4(1.0, 0.0, 0.0, 1.0); // Set the color of particles to red
        }
        ";

        // Create the shader using the dynamically generated GLSL code
        _shader = new Shader(vertexShaderCode, fragmentShaderCode);
        _shader.Use();

        // Initialize particles
        for (int i = 0; i < 500; i++)
        {
            var position = new Vector2(
                Random.Shared.Next(-400, 400) / 100f,
                Random.Shared.Next(-300, 300) / 100f
            );
            var velocity = new Vector2(
                Random.Shared.Next(-50, 50) / 100f,
                Random.Shared.Next(-50, 50) / 100f
            );
            _particles.Add(new Particle(position, velocity, Random.Shared.Next(1, 10)));
        }

        // VAO and VBO setup
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        GL.BufferData(BufferTarget.ArrayBuffer, _particles.Count * 2 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (!IsFocused) return;

        var keyboard = KeyboardState;

        // Handle keyboard input
        if (keyboard.IsKeyDown(Keys.Escape))
            Close();

        // Increase/decrease gravitational constant
        if (keyboard.IsKeyDown(Keys.Up))
            _gravitationalConstant += 0.1f;
        if (keyboard.IsKeyDown(Keys.Down))
            _gravitationalConstant -= 0.1f;

        // Decrease/increase time step
        if (keyboard.IsKeyDown(Keys.Left))
            _timeStep = Math.Max(0.001f, _timeStep - 0.001f);
        if (keyboard.IsKeyDown(Keys.Right))
            _timeStep += 0.001f;

        // Spawn new particle when Spacebar is pressed
        if (keyboard.IsKeyDown(Keys.Space))
        {
            // Randomly generate a new particle
            var position = new Vector2(
                Random.Shared.Next(-400, 400) / 100f,
                Random.Shared.Next(-300, 300) / 100f
            );
            var velocity = new Vector2(
                Random.Shared.Next(-50, 50) / 100f,
                Random.Shared.Next(-50, 50) / 100f
            );
            _particles.Add(new Particle(position, velocity, Random.Shared.Next(1, 10)));
        }

        // Simulate particle movement
        Simulate((float)args.Time);
    }

    private void Simulate(float deltaTime)
    {
        // Update particle positions based on Newtonian gravity
        for (int i = 0; i < _particles.Count; i++)
        {
            for (int j = 0; j < _particles.Count; j++)
            {
                if (i == j) continue;

                var direction = _particles[j].Position - _particles[i].Position;
                float distance = direction.Length;
                if (distance < 0.01f) continue;

                float forceMagnitude = _gravitationalConstant * (_particles[i].Mass * _particles[j].Mass) / (distance * distance);
                var force = direction.Normalized() * forceMagnitude;

                _particles[i].Velocity += force / _particles[i].Mass * _timeStep;
            }
        }

        foreach (var particle in _particles)
        {
            particle.Position += particle.Velocity * _timeStep;
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Update particle positions in VBO
        var particlePositions = new float[_particles.Count * 2];
        for (int i = 0; i < _particles.Count; i++)
        {
            particlePositions[i * 2] = _particles[i].Position.X;
            particlePositions[i * 2 + 1] = _particles[i].Position.Y;
        }

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, particlePositions.Length * sizeof(float), particlePositions);

        // Render particles
        _shader.Use();
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Points, 0, _particles.Count);
        GL.BindVertexArray(0);

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        _shader.Dispose();
    }
}
