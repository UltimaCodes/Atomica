using System;
using OpenTK.Graphics.OpenGL4;

public class Shader : IDisposable
{
    public int Handle { get; private set; }

    public Shader(string vertexSource, string fragmentSource)
    {
        // Compile vertex shader
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        GL.CompileShader(vertexShader);
        CheckShaderCompileStatus(vertexShader);

        // Compile fragment shader
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        GL.CompileShader(fragmentShader);
        CheckShaderCompileStatus(fragmentShader);

        // Link shaders into a program
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);
        CheckProgramLinkStatus(Handle);

        // Delete individual shaders (no longer needed after linking)
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Handle);
    }

    private static void CheckShaderCompileStatus(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == (int)All.False)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new InvalidOperationException($"Shader compilation failed: {infoLog}");
        }
    }

    private static void CheckProgramLinkStatus(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
        if (status == (int)All.False)
        {
            string infoLog = GL.GetProgramInfoLog(program);
            throw new InvalidOperationException($"Program linking failed: {infoLog}");
        }
    }
}
