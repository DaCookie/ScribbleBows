using UnityEngine;

namespace Game
{

    ///<summary>
    /// Makes an entity move on the horizontal axis, using a custom raycast-based obstacles detection.
    /// 
    /// In order to make event callbacks work as expected, you must call Move() each update, even with a movement value of 0. This will
    /// ensure this component to trigger OnBeginMove and OnEndMove at the appropriate moment.
    ///</summary>
    //[AddComponentMenu("_GAME/HorizontalMovement")]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class HorizontalMovement : MonoBehaviour
    {

        #region Properties

        private const float COLLISION_DETECTION_Y_OFFSET = 0.1f;
        private const float OBSTACLE_OFFSET = 0.01f;

        [Header("References")]

        [SerializeField]
        private CapsuleCollider2D m_Collider = null;

        [Header("Settings")]

        [SerializeField]
        [Tooltip("The speed (in units per second) of the entity")]
        private float m_Speed = 6f;

        [SerializeField]
        private LayerMask m_ObstaclesMask = ~0;

        [Header("Events")]

        [SerializeField]
        [Tooltip("Called when the entity starts moving")]
        private VoidEvent m_OnBeginMove = new VoidEvent();

        [SerializeField]
        [Tooltip("Called each frame while the entity is moving")]
        private VoidEvent m_OnUpdateMove = new VoidEvent();

        [SerializeField]
        [Tooltip("Called when the entity stops moving")]
        private VoidEvent m_OnEndMove = new VoidEvent();

        private bool m_IsMoving = false;

        #endregion


        #region Lifecycle

        /// <summary>
        /// Called when the script is loaded.
        /// </summary>
        private void Awake()
        {
            if(m_Collider == null) { m_Collider = GetComponent<CapsuleCollider2D>(); }
        }

        #endregion


        #region Public API

        /// <summary>
        /// Makes the entity move in the given direction.
        /// Note that in order to trigger event callbacks as expected, this method should be called each frame, even if the given direction is 0.
        /// </summary>
        /// <param name="_DirectionX">The direction which you want the entity to move.</param>
        /// <param name="_DeltaTime">The elapsed time since the last frame.</param>
        public void Move(float _DirectionX, float _DeltaTime)
        {
            // If the given direction is 0, end movement
            if (_DirectionX == 0f)
            {
                NotifyEndMovement();
                return;
            }

            // Check if there's an obstacle ahead using a capsule cast, which emulates the next movement of the entity
            float distance = m_Speed * _DeltaTime;
            RaycastHit2D hitInfo = Physics2D.CapsuleCast((Vector2)transform.position + CollisionOffset, CollisionSize, CollisionDirection, 0f, Vector2.right * _DirectionX, distance, m_ObstaclesMask);

            float finalMovement = 0f;
            // If there's an obstacle ahead, move to the hit point
            if (hitInfo.collider != null)
            {
                if (hitInfo.distance > 0f)
                {
                    finalMovement = Mathf.Sign(_DirectionX) * Mathf.Max(0f, hitInfo.distance - OBSTACLE_OFFSET);
                }
            }
            // Else, if there's no obstacle ahead, just move forward
            else
            {
                finalMovement = _DirectionX * distance;
            }

            // If the entity can't move because of an obstacle right aside it, end movement
            if (finalMovement == 0f)
            {
                NotifyEndMovement();
                return;
            }

            // Apply and update movement
            transform.position += Vector3.right * finalMovement;
            NotifyUpdateMovement();
        }

        #endregion


        #region Private API

        /// <summary>
        /// Calls OnBeginMove() event if the entity just starts moving, and calls OnUpdateMove() anyway.
        /// </summary>
        private void NotifyUpdateMovement()
        {
            if (!m_IsMoving)
            {
                m_IsMoving = true;
                m_OnBeginMove.Invoke();
            }

            m_OnUpdateMove.Invoke();
        }

        /// <summary>
        /// Calls OnEndMove() event if the entity just stops moving.
        /// </summary>
        private void NotifyEndMovement()
        {
            if(m_IsMoving)
            {
                m_IsMoving = false;
                m_OnEndMove.Invoke();
            }
        }

        #endregion


        #region Accessors

        /// <summary>
        /// Gets the offset of the collider, or Vector2.zero if the collider is not defined.
        /// </summary>
        private Vector2 CollisionOffset
        {
            get { return m_Collider != null ? m_Collider.offset : Vector2.zero; }
        }

        /// <summary>
        /// Gets the size of the collider, or Vector2.zero if the collider is not defined.
        /// Note that the Y component of the collider's size is decreased by a small offset, in order to not detect the floor as an obstacle.
        /// </summary>
        private Vector2 CollisionSize
        {
            get { return m_Collider != null ? new Vector2(m_Collider.size.x, Mathf.Max(0f, m_Collider.size.y - COLLISION_DETECTION_Y_OFFSET)) : Vector2.one; }
        }

        /// <summary>
        /// Gets the capsule collider direction, or assumes it's vertical if the collider is not defined.
        /// </summary>
        private CapsuleDirection2D CollisionDirection
        {
            get { return m_Collider != null ? m_Collider.direction : CapsuleDirection2D.Vertical; }
        }

        /// <summary>
        /// Called when the entity starts moving.
        /// </summary>
        public VoidEvent OnBeginMove
        {
            get { return m_OnBeginMove; }
        }

        /// <summary>
        /// Called each frame while the entity is moving.
        /// </summary>
        public VoidEvent OnUpdateMove
        {
            get { return m_OnUpdateMove; }
        }

        /// <summary>
        /// Called when the entity stops moving.
        /// </summary>
        public VoidEvent OnEndMove
        {
            get { return m_OnEndMove; }
        }

        #endregion

    }

}