using UnityEditor;

namespace FriedSynapse.FlowEnt
{
#if UNITY_EDITOR
    public static class GizmosCallbacks
    {
        private static void DrawGizmo(IGizmoDrawer drawer)
            => drawer.OnGizmosDrawing();

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        public static void DrawGizmo(TweenAuthoring drawer, GizmoType gizmoType)
            => DrawGizmo(drawer);

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        public static void DrawGizmo(EchoAuthoring drawer, GizmoType gizmoType)
            => DrawGizmo(drawer);

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        public static void DrawGizmo(FlowAuthoring drawer, GizmoType gizmoType)
            => DrawGizmo(drawer);

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        public static void DrawGizmo(AnimationsAuthoring drawer, GizmoType gizmoType)
            => DrawGizmo(drawer);
    }
#endif
}