using UnityEngine;

namespace Game
{

    ///<summary>
    /// Resizes an orthographic camera depending on the number of cells you want on X or Y.
    ///</summary>
    [AddComponentMenu("_GAME/Camera Resizer")]
    public class OrthographicCameraResizer : MonoBehaviour
    {

        [SerializeField]
        private Camera m_Camera = null;

        [SerializeField]
        private Vector2 m_Ratio = new Vector2(16f, 9f);

        [SerializeField]
        private Vector2 m_Size = new Vector2(0f, 5f);

        /// <summary>
        /// Ensure the current size has been applied to the Camera's orthographic size.
        /// </summary>
        public void ResizeCamera()
        {
            if (Camera != null)
            {
                Camera.orthographicSize = m_Size.y / 2;
            }
        }

        /// <summary>
        /// Gets/Sets the screen ratio.
        /// </summary>
        public Vector2 Ratio
        {
            get { return m_Ratio; }
            set
            {
                // If the X component of the ratio has changed
                if(m_Ratio.x != value.x)
                {
                    float sizeRatio = m_Size.x / m_Ratio.x;
                    float ratioDiff = value.x - m_Ratio.x;
                    float finalSize = m_Size.x + sizeRatio * ratioDiff;
                    Size = new Vector2(finalSize, m_Size.y);
                }
                else
                {
                    float sizeRatio = m_Size.y / m_Ratio.y;
                    float ratioDiff = value.y - m_Ratio.y;
                    float finalSize = m_Size.y + sizeRatio * ratioDiff;
                    Size = new Vector2(m_Size.x, finalSize);
                }

                m_Ratio = value;
            }
        }

        /// <summary>
        /// Gets/Sets the screen size.
        /// </summary>
        public Vector2 Size
        {
            get { return m_Size; }
            set
            {
                // If the X component of the size has changed
                if (m_Size.x != value.x)
                {
                    m_Size.x = value.x;
                    m_Size.y = m_Size.x / RatioFrac;
                }
                else
                {
                    m_Size.y = value.y;
                    m_Size.x = m_Size.y * RatioFrac;
                }

                if(Camera != null)
                {
                    Camera.orthographicSize = m_Size.y / 2;
                }
            }
        }

        private float RatioFrac
        {
            get { return m_Ratio.x / m_Ratio.y; }
        }

        /// <summary>
        /// Gets the defined camera, or the main camera if it's not assigned.
        /// </summary>
        public Camera Camera
        {
            get
            {
                if(m_Camera == null)
                {
                    m_Camera = Camera.main;
                }
                return m_Camera;
            }
            set
            {
                m_Camera = value;
            }
        }

    }

}