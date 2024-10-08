using System.Diagnostics;
using GameCore.Service;
using MessagePack;
using MessagePack.Resolvers;

namespace GameCore.AOTGeneration
{
    public static class Startup
    {
        private static bool _initialized = false;

        internal static void Initialize()
        {
            if (_initialized) return;
            MessagePackInitialize();
#if MAGICONION_CLIENT
            MagicOnionInitialize();
#endif
            MessagePackSerializer.DefaultOptions = MessagePackSerializer.DefaultOptions
                .WithResolver(StaticCompositeResolver.Instance);
            _initialized = true;
        }

        private static void MessagePackInitialize()
        {
            StaticCompositeResolver.Instance.Register(
                MessagePack.Resolvers.GeneratedResolver.Instance,
                MessagePack.Resolvers.StandardResolver.Instance
            );
        }
#if MAGICONION_CLIENT
        private static void MagicOnionInitialize()
        {
            StaticCompositeResolver.Instance.Register(
                // Add: Use MessagePack formatter resolver generated by the source generator.
                MagicOnionGeneration.Resolver,
                MessagePack.Resolvers.GeneratedResolver.Instance,
                BuiltinResolver.Instance,
                PrimitiveObjectResolver.Instance
            );
        }

#endif

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EditorInitialize()
        {
            Initialize();
        }

#endif
    }
}