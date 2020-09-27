using UnityEngine;
using UnityEditor;

namespace Game
{

    ///<summary>
    /// 
    ///</summary>
    [CustomEditor(typeof(OrthographicCameraResizer))]
    public class OrthographicCameraResizerEditor : Editor
	{

        private const string CAMERA_PROP = "m_Camera";
        private const string RATIO_PROP = "m_Ratio";
        private const string SIZE_PROP = "m_Size";

        private SerializedProperty m_CameraProp = null;
        private SerializedProperty m_RatioProp = null;
        private SerializedProperty m_SizeProp = null;

        private OrthographicCameraResizer m_CameraResizer = null;

        private void OnEnable()
        {
            m_CameraProp = serializedObject.FindProperty(CAMERA_PROP);
            m_RatioProp = serializedObject.FindProperty(RATIO_PROP);
            m_SizeProp = serializedObject.FindProperty(SIZE_PROP);
        }

        public override void OnInspectorGUI()
        {
            if(CameraResizer.Camera != null && !CameraResizer.Camera.orthographic)
            {
                EditorGUILayout.HelpBox("The target camera's projection must be set to \"Orthographic\"", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(m_CameraProp);

            Vector2 ratio = EditorGUILayout.Vector2Field(m_RatioProp.displayName, m_RatioProp.vector2Value);
            if (ratio != m_RatioProp.vector2Value)
            {
                CameraResizer.Ratio = ratio;
            }

            Vector2 size = EditorGUILayout.Vector2Field(m_SizeProp.displayName, m_SizeProp.vector2Value);
            if (size != m_SizeProp.vector2Value)
            {
                CameraResizer.Size = size;
            }

            CameraResizer.ResizeCamera();
        }

        public OrthographicCameraResizer CameraResizer
        {
            get
            {
                if (m_CameraResizer == null)
                {
                    m_CameraResizer = target as OrthographicCameraResizer;
                }
                return m_CameraResizer;
            }
        }

    }

}