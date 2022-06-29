using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace StoryFiles
{
    [CustomEditor(typeof(RotateBillboard)), CanEditMultipleObjects]
    public class RotateBillboardEditor : Editor
    {
        RotateBillboard billboard;

        ArcHandle maxArcHandle = new ArcHandle();
        ArcHandle minArcHandle = new ArcHandle();

        float impulse = 1f;

        private void OnEnable()
        {
            billboard = ((RotateBillboard)this.target);

            maxArcHandle.SetColorWithRadiusHandle(Color.white, 0.1f);
            maxArcHandle.radiusHandleSizeFunction = null;

            minArcHandle.SetColorWithRadiusHandle(Color.red, 0.1f);
            minArcHandle.radiusHandleSizeFunction = null;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            billboard.target = (Transform)EditorGUILayout.ObjectField("Target", billboard.target, typeof(Transform), true);

            billboard.useLimits = EditorGUILayout.Toggle("Allow Rotate Limits", billboard.useLimits);
            if (billboard.useLimits)
            {
                //billboard.relativeToPosition = EditorGUILayout.Toggle("Relative To Current Position", billboard.relativeToPosition);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Angle");
                billboard.minAngle = EditorGUILayout.FloatField(billboard.minAngle);
                EditorGUILayout.MinMaxSlider(ref billboard.minAngle, ref billboard.maxAngle, 0, 360);
                billboard.maxAngle = EditorGUILayout.FloatField(billboard.maxAngle);
                EditorGUILayout.EndHorizontal();


            }
            EditorUtility.SetDirty(target);
        }

        private void OnSceneGUI()
        {
            if (billboard.useLimits)
            {
                var angle = billboard.maxAngle - billboard.minAngle;
                maxArcHandle.angle = angle;
                maxArcHandle.radius = impulse;

                Vector3 forward = Vector3.forward;
                if (billboard.relativeToPosition)
                {
                    forward = billboard.transform.forward;
                }


                var normalDirection = billboard.maxAngle % 360 > 180 ? Vector3.forward : Vector3.back;

                //if (billboard.minAngle > 180)
                //{
                //    forward *= -1;
                //}

                Vector3 handleDirection = Quaternion.Euler(0, billboard.minAngle, 0) * forward;
                Vector3 handleNormal = Vector3.Cross(handleDirection, Vector3.back);
                Matrix4x4 handleMatrix = Matrix4x4.TRS(
                    billboard.transform.position,
                    Quaternion.LookRotation(handleDirection, handleNormal),
                    Vector3.one
                );


                using (new Handles.DrawingScope(handleMatrix))
                {
                    // draw the handle
                    EditorGUI.BeginChangeCheck();
                    maxArcHandle.DrawHandle();
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(billboard, "Change Projectile Properties");

                        angle = maxArcHandle.angle;
                        billboard.maxAngle = billboard.minAngle + angle;
                        billboard.maxAngle = Mathf.Clamp(billboard.maxAngle, billboard.minAngle, 360 + billboard.minAngle);

                    }
                }

                //angle = billboard.maxAngle - billboard.minAngle;
                //minArcHandle.angle = angle;
                //minArcHandle.radius = impulse;

                //if (billboard.maxAngle > 180)
                //{
                //    forward *= -1;
                //}

                ////normalDirection = billboard.maxAngle % 360 > 180 ? Vector3.forward : -Vector3.forward;
                //handleDirection = Quaternion.Euler(0, billboard.maxAngle, 0) * forward;

                //handleNormal = Vector3.Cross(handleDirection, Vector3.back);
                //handleMatrix = Matrix4x4.TRS(
                //    billboard.transform.position + new Vector3(0, 0.1f, 0),
                //    Quaternion.LookRotation(handleDirection, handleNormal),
                //    Vector3.one
                //);

                //using (new Handles.DrawingScope(handleMatrix))
                //{
                //    // draw the handle
                //    EditorGUI.BeginChangeCheck();
                //    minArcHandle.DrawHandle();
                //    if (EditorGUI.EndChangeCheck())
                //    {
                //        Undo.RecordObject(billboard, "Change Projectile Properties");

                //        angle = minArcHandle.angle;
                //        billboard.minAngle = billboard.maxAngle + angle;
                //        billboard.minAngle = Mathf.Clamp(billboard.minAngle, 0, billboard.maxAngle);
                //    }
                //}
            }
        }
    }
}