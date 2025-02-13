// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace UnityEditor
{
    [CustomEditor(typeof(LightmapParameters))]
    [CanEditMultipleObjects]
    class LightmapParametersEditor : Editor
    {
        SerializedProperty  m_Resolution;
        SerializedProperty  m_ClusterResolution;
        SerializedProperty  m_IrradianceBudget;
        SerializedProperty  m_IrradianceQuality;
        SerializedProperty  m_BackFaceTolerance;
        SerializedProperty  m_ModellingTolerance;
        SerializedProperty  m_EdgeStitching;
        SerializedProperty  m_SystemTag;
        SerializedProperty  m_IsTransparent;

        SerializedProperty  m_Pushoff;
        SerializedProperty  m_BakedLightmapTag;
        SerializedProperty  m_LimitLightmapCount;
        SerializedProperty  m_LightmapMaxCount;

        SerializedProperty  m_AntiAliasingSamples;

        SavedBool m_RealtimeGISettings;
        SavedBool m_BakedGISettings;
        SavedBool m_GeneralParametersSettings;

        public void OnEnable()
        {
            m_Resolution                = serializedObject.FindProperty("resolution");
            m_ClusterResolution         = serializedObject.FindProperty("clusterResolution");
            m_IrradianceBudget          = serializedObject.FindProperty("irradianceBudget");
            m_IrradianceQuality         = serializedObject.FindProperty("irradianceQuality");
            m_BackFaceTolerance         = serializedObject.FindProperty("backFaceTolerance");
            m_ModellingTolerance        = serializedObject.FindProperty("modellingTolerance");
            m_EdgeStitching             = serializedObject.FindProperty("edgeStitching");
            m_IsTransparent             = serializedObject.FindProperty("isTransparent");
            m_SystemTag                 = serializedObject.FindProperty("systemTag");
            m_AntiAliasingSamples       = serializedObject.FindProperty("antiAliasingSamples");
            m_BakedLightmapTag          = serializedObject.FindProperty("bakedLightmapTag");
            m_Pushoff                   = serializedObject.FindProperty("pushoff");
            m_LimitLightmapCount        = serializedObject.FindProperty("limitLightmapCount");
            m_LightmapMaxCount          = serializedObject.FindProperty("maxLightmapCount");

            m_RealtimeGISettings        = new SavedBool("LightmapParameters.ShowRealtimeGISettings", true);
            m_BakedGISettings           = new SavedBool("LightmapParameters.ShowBakedGISettings", true);
            m_GeneralParametersSettings = new SavedBool("LightmapParameters.ShowGeneralParametersSettings", true);
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // realtime settings
            if (SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Realtime))
            {
                m_RealtimeGISettings.value = EditorGUILayout.FoldoutTitlebar(m_RealtimeGISettings.value, Styles.precomputedRealtimeGIContent, true);
                if (m_RealtimeGISettings.value)
                {
                    ++EditorGUI.indentLevel;

                    EditorGUILayout.LabelField(Styles.enlightenLabel, EditorStyles.boldLabel);

                    ++EditorGUI.indentLevel;

                    EditorGUILayout.PropertyField(m_Resolution, Styles.resolutionContent);
                    EditorGUILayout.Slider(m_ClusterResolution, 0.1F, 1.0F, Styles.clusterResolutionContent);
                    EditorGUILayout.IntSlider(m_IrradianceBudget, 32, 2048, Styles.irradianceBudgetContent);
                    EditorGUILayout.IntSlider(m_IrradianceQuality, 512, 131072, Styles.irradianceQualityContent);
                    EditorGUILayout.Slider(m_ModellingTolerance, 0.0f, 1.0f, Styles.modellingToleranceContent);
                    EditorGUILayout.PropertyField(m_EdgeStitching, Styles.edgeStitchingContent);
                    EditorGUILayout.PropertyField(m_IsTransparent, Styles.isTransparent);
                    EditorGUILayout.PropertyField(m_SystemTag, Styles.systemTagContent);
                    EditorGUILayout.Space();

                    --EditorGUI.indentLevel;
                    --EditorGUI.indentLevel;
                }
            }

            GUILayout.Label(Styles.bakedGIContent, EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(m_AntiAliasingSamples, Styles.antiAliasingSamplesContent);
            const float minPushOff = 0.0001f; // Keep in sync with PLM_MIN_PUSHOFF
            EditorGUILayout.Slider(m_Pushoff, minPushOff, 1.0f, Styles.pushoffContent);
            EditorGUILayout.PropertyField(m_BakedLightmapTag, Styles.bakedLightmapTagContent);
            m_LimitLightmapCount.boolValue = EditorGUILayout.Toggle(Styles.limitLightmapCount, m_LimitLightmapCount.boolValue);
            if (m_LimitLightmapCount.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_LightmapMaxCount, Styles.lightmapMaxCount);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            GUILayout.Label(Styles.generalGIContent, EditorStyles.boldLabel);
            EditorGUILayout.Slider(m_BackFaceTolerance, 0.0f, 1.0f, Styles.backFaceToleranceContent);

            serializedObject.ApplyModifiedProperties();
        }

        internal override void OnHeaderControlsGUI()
        {
            GUILayoutUtility.GetRect(10, 10, 16, 16, EditorStyles.layerMaskField);
            GUILayout.FlexibleSpace();
        }

        private class Styles
        {
            public static readonly GUIContent generalGIContent = EditorGUIUtility.TrTextContent("General Parameters", "Settings used in both Precomputed Realtime Global Illumination and Baked Global Illumination.");
            public static readonly GUIContent precomputedRealtimeGIContent = EditorGUIUtility.TrTextContent("Realtime Global Illumination", "Settings used in Precomputed Realtime Global Illumination where it is precomputed how indirect light can bounce between static objects, but the final lighting is done at runtime. Lights, ambient lighting in addition to the materials and emission of static objects can still be changed at runtime. Only static objects can affect GI by blocking and bouncing light, but non-static objects can receive bounced light via light probes.");  // Reuse the label from the Lighting window
            public static readonly GUIContent resolutionContent = EditorGUIUtility.TrTextContent("Resolution", "Realtime lightmap resolution in texels per world unit. This value is multiplied by the realtime resolution in the Lighting window to give the output lightmap resolution. This should generally be an order of magnitude less than what is common for baked lightmaps to keep the precompute time manageable and the performance at runtime acceptable. Note that if this is made more fine-grained, then the Irradiance Budget will often need to be increased too, to fully take advantage of this increased detail.");
            public static readonly GUIContent clusterResolutionContent = EditorGUIUtility.TrTextContent("Cluster Resolution", "The ratio between the resolution of the clusters with which light bounce is calculated and the resolution of the output lightmaps that sample from these.");
            public static readonly GUIContent irradianceBudgetContent = EditorGUIUtility.TrTextContent("Irradiance Budget", "The amount of data used by each texel in the output lightmap. Specifies how fine-grained a view of the scene an output texel has. Small values mean more averaged out lighting, since the light contributions from more clusters are treated as one. Affects runtime memory usage and to a lesser degree runtime CPU usage.");
            public static readonly GUIContent irradianceQualityContent = EditorGUIUtility.TrTextContent("Irradiance Quality", "The number of rays to cast to compute which clusters affect a given output lightmap texel - the granularity of how this is saved is defined by the Irradiance Budget. Affects the speed of the precomputation but has no influence on runtime performance.");
            public static readonly GUIContent backFaceToleranceContent = EditorGUIUtility.TrTextContent("Backface Tolerance", "The percentage of rays shot from an output texel that must hit front faces to be considered usable. Allows a texel to be invalidated if too many of the rays cast from it hit back faces (the texel is inside some geometry). In that case artefacts are avoided by cloning valid values from surrounding texels. For example, if backface tolerance is 0.0, the texel is rejected only if it sees nothing but backfaces. If it is 1.0, the ray origin is rejected if it has even one ray that hits a backface.");
            public static readonly GUIContent modellingToleranceContent = EditorGUIUtility.TrTextContent("Modelling Tolerance", "Maximum size of gaps that can be ignored for GI.");
            public static readonly GUIContent edgeStitchingContent = EditorGUIUtility.TrTextContent("Edge Stitching", "If enabled, ensures that UV charts (aka UV islands) in the generated lightmaps blend together where they meet so there is no visible seam between them.");
            public static readonly GUIContent systemTagContent = EditorGUIUtility.TrTextContent("System Tag", "Systems are groups of objects whose lightmaps are in the same atlas. It is also the granularity at which dependencies are calculated. Multiple systems are created automatically if the scene is big enough, but it can be helpful to be able to split them up manually for e.g. streaming in sections of a level. The system tag lets you force an object into a different realtime system even though all the other parameters are the same.");
            public static readonly GUIContent bakedGIContent = EditorGUIUtility.TrTextContent("Baked Global Illumination", "Settings used in Baked Global Illumination where direct and indirect lighting for static objects is precalculated and saved (baked) into lightmaps for use at runtime. This is useful when lights are known to be static, for mobile, for low end devices and other situations where there is not enough processing power to use Precomputed Realtime GI. You can toggle on each light whether it should be included in the bake.");  // Reuse the label from the Lighting window
            public static readonly GUIContent antiAliasingSamplesContent = EditorGUIUtility.TrTextContent("Anti-aliasing Samples", "The maximum number of times to supersample a texel to reduce aliasing. Progressive lightmapper supersamples the positions and normals buffers (part of the G-buffer) and hence the sample count is a multiplier on the amount of memory used for those buffers. Progressive lightmapper clamps the value to the [1;16] range.");
            public static readonly GUIContent isTransparent = EditorGUIUtility.TrTextContent("Is Transparent", "If enabled, the object appears transparent during GlobalIllumination lighting calculations. Backfaces are not contributing to and light travels through the surface. This is useful for emissive invisible surfaces.");
            public static readonly GUIContent pushoffContent = EditorGUIUtility.TrTextContent("Pushoff", "The amount to push off geometry for ray tracing, in modelling units. It is applied to all baked light maps, so it will affect direct light, indirect light and AO. Useful for getting rid of unwanted AO or shadowing.");
            public static readonly GUIContent bakedLightmapTagContent = EditorGUIUtility.TrTextContent("Baked Tag", "An integer that lets you force an object into a different baked lightmap even though all the other parameters are the same. This can be useful e.g. when streaming in sections of a level.");
            public static readonly GUIContent limitLightmapCount = EditorGUIUtility.TrTextContent("Limit Lightmap Count", "If enabled, objects with the same baked GI settings will be packed into a specified number of lightmaps. This may reduce the objects' lightmap resolution.");
            public static readonly GUIContent lightmapMaxCount = EditorGUIUtility.TrTextContent("Max Lightmaps", "The maximum number of lightmaps into which objects will be packed.");

            public static readonly GUIContent generalLabel = EditorGUIUtility.TrTextContent("General");
            public static readonly GUIContent progressiveLabel = EditorGUIUtility.TrTextContent("Progressive Lightmapper");
            public static readonly GUIContent enlightenLabel = EditorGUIUtility.TrTextContent("Enlighten");
        }
    }
}
