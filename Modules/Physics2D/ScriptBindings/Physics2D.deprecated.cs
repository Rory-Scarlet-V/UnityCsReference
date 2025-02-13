// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;

namespace UnityEngine
{
    partial class CircleCollider2D
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("CircleCollider2D.center has been deprecated. Use CircleCollider2D.offset instead (UnityUpgradable) -> offset", true)]
        public Vector2 center { get { return Vector2.zero; } set {} }
    }

    partial class BoxCollider2D
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("BoxCollider2D.center has been deprecated. Use BoxCollider2D.offset instead (UnityUpgradable) -> offset", true)]
        public Vector2 center { get { return Vector2.zero; } set {} }
    }

    partial class Joint2D
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Joint2D.collideConnected has been deprecated. Use Joint2D.enableCollision instead (UnityUpgradable) -> enableCollision", true)]
        public bool collideConnected { get { return enableCollision; } set { enableCollision = value; } }
    }

    partial class AreaEffector2D
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("AreaEffector2D.forceDirection has been deprecated. Use AreaEffector2D.forceAngle instead (UnityUpgradable) -> forceAngle", true)]
        public float forceDirection { get { return forceAngle; } set { forceAngle = value; } }
    }

    partial class PlatformEffector2D
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("PlatformEffector2D.oneWay has been deprecated. Use PlatformEffector2D.useOneWay instead (UnityUpgradable) -> useOneWay", true)]
        public bool oneWay { get { return useOneWay; } set { useOneWay = value; } }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("PlatformEffector2D.sideFriction has been deprecated. Use PlatformEffector2D.useSideFriction instead (UnityUpgradable) -> useSideFriction", true)]
        public bool sideFriction { get { return useSideFriction; } set { useSideFriction = value; } }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("PlatformEffector2D.sideBounce has been deprecated. Use PlatformEffector2D.useSideBounce instead (UnityUpgradable) -> useSideBounce", true)]
        public bool sideBounce { get { return useSideBounce; } set { useSideBounce = value; } }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("PlatformEffector2D.sideAngleVariance has been deprecated. Use PlatformEffector2D.sideArc instead (UnityUpgradable) -> sideArc", true)]
        public float sideAngleVariance { get { return sideArc; } set { sideArc = value; } }
    }

    partial class Physics2D
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.raycastsHitTriggers is deprecated. Use Physics2D.queriesHitTriggers instead. (UnityUpgradable) -> queriesHitTriggers", true)]
        public static bool raycastsHitTriggers { get { return false; } set {} }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.raycastsStartInColliders is deprecated. Use Physics2D.queriesStartInColliders instead. (UnityUpgradable) -> queriesStartInColliders", true)]
        public static bool raycastsStartInColliders { get { return false; } set {} }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.deleteStopsCallbacks is deprecated.(UnityUpgradable) -> changeStopsCallbacks", true)]
        public static bool deleteStopsCallbacks { get { return false; } set {} }

        [Obsolete("Physics2D.changeStopsCallbacks is deprecated and will always return false.", false)]
        public static bool changeStopsCallbacks { get { return false; } set {} }

        [Obsolete("Physics2D.minPenetrationForPenalty is deprecated. Use Physics2D.defaultContactOffset instead. (UnityUpgradable) -> defaultContactOffset", false)]
        public static float minPenetrationForPenalty { get { return defaultContactOffset; } set { defaultContactOffset = value; } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.autoSimulation is deprecated. Use Physics2D.simulationMode instead.", false)]
        public static bool autoSimulation { get { return simulationMode != SimulationMode2D.Script; } set { simulationMode = value ? SimulationMode2D.FixedUpdate : SimulationMode2D.Script; } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.colliderAwakeColor is deprecated. This options has been moved to 2D Preferences.", true)]
        public static Color colliderAwakeColor { get { return Color.magenta; } set { } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.colliderAsleepColor is deprecated. This options has been moved to 2D Preferences.", true)]
        public static Color colliderAsleepColor { get { return Color.magenta; } set { } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.colliderContactColor is deprecated. This options has been moved to 2D Preferences.", true)]
        public static Color colliderContactColor { get { return Color.magenta; } set { } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.colliderAABBColor is deprecated. All Physics 2D colors moved to Preferences. This is now known as 'Collider Bounds Color'.", true)]
        public static Color colliderAABBColor { get { return Color.magenta; } set { } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.contactArrowScale is deprecated. This options has been moved to 2D Preferences.", true)]
        public static float contactArrowScale { get { return 0.2f; } set { } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.alwaysShowColliders is deprecated. It is no longer available in the Editor or Builds.", true)]
        public static bool alwaysShowColliders { get; set; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.showCollidersFilled is deprecated. It is no longer available in the Editor or Builds.", true)]
        public static bool showCollidersFilled { get; set; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.showColliderSleep is deprecated. It is no longer available in the Editor or Builds.", true)]
        public static bool showColliderSleep { get; set; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.showColliderContacts is deprecated. It is no longer available in the Editor or Builds.", true)]
        public static bool showColliderContacts { get; set; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Physics2D.showColliderAABB is deprecated. It is no longer available in the Editor or Builds.", true)]
        public static bool showColliderAABB { get { return false; } set { } }
    }
}
