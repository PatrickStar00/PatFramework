using UnityEngine;

public class ShaderUtils
{
    public static Shader LoadShader(string sResourcePath)
    {
// #if UNITY_EDITOR
        return Shader.Find(sResourcePath);
// #else
//         return ResourceAssemblyHelper.LoadAsset(sResourcePath,ResourceType.Shader) as Shader;
// #endif
    }
}
