public static class D3MiniGameStartupConfig
{
    public const string GameName = "Runner";
    public const string CdnRoot = "https://qzz2d.qzzres.com/M5_BUILD_TEST";
    public const string ResourceGameName = "Runner";

#if UNITY_TT
    public const string Platform = "tt";
#else
    public const string Platform = "wx";
#endif

    public const string CdnUrl = CdnRoot + "/" + ResourceGameName + "/WebGL/" + Platform + "/webgl";
}
