using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{

    ///<summary>
    /// 
    ///</summary>
    //[AddComponentMenu("_GAME/PlayerController")]
    [RequireComponent(typeof(HorizontalMovement))]
    public class PlayerController : MonoBehaviour
    {

        #region Properties

        [Header("References")]

        [SerializeField]
        private InputActionAsset m_Actions = null;

        [SerializeField]
        private HorizontalMovement m_Movement = null;

        [Header("General Settings")]

        [SerializeField, Range(1, 2)]
        private int m_PlayerID = 1;

        // Cache

        // The input movement on X axis, whatever the ability of the character to move
        private float m_MovementX = 0f;

        #endregion


        #region Lifecycle

        private void Awake()
        {
            InputActionMap playerMap = m_Actions.FindActionMap(m_PlayerID == 1 ? "Player" : "Player2");
            playerMap.FindAction("Move").started += ctx => { m_MovementX = ctx.ReadValue<Vector2>().x; };
            playerMap.FindAction("Move").canceled += ctx => { m_MovementX = 0f; };

            playerMap.FindAction("Jump").performed += ctx => Jump();
            playerMap.FindAction("Throw").performed += ctx => ThrowItem();

            playerMap.Enable();

            if(m_Movement == null) { m_Movement = GetComponent<HorizontalMovement>(); }
        }

        private void Update()
        {
            m_Movement.Move(m_MovementX, Time.deltaTime);
        }

        #endregion


        #region Public API

        public void Jump()
        {
            Debug.Log("Jump " + name);
        }

        public void ThrowItem()
        {
            Debug.Log("Throw item" + name);
        }

        #endregion

    }

}