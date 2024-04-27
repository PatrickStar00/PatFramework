namespace PAT.Resource
{
    public enum ResourceType
    {
        Document,
        Language,
        Prefab,
        Model_Material,
        UI,
        UI_Material,
        Scene,
        Scene_Material,
        Atlas,
        Texture,
        Mat_Texture,
        Shader,
        Version,
        Audio,
        Bytes,
        AssetBundle,
        DownloadImage,
        Video,
        Font,
        AnimatorController,//不包含UI
        Default,    //终止标记,保证它在最后面
    }


    public enum PackStrategy
    {
        PackTogether,
        PackSingle,
        None
    }

    public enum PackStratgyType
    {
        None,
        String,
        Bytes
    }
}