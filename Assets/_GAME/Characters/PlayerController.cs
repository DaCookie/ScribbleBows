using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{

    ///<summary>
    /// 
    ///</summary>
    //[AddComponentMenu("_GAME/PlayerController")]
    public class PlayerController : MonoBehaviour
    {

        [Header("References")]

        [SerializeField]
        private InputActionAsset m_Actions = null;

        [Header("General Settings")]

        [SerializeField, Range(1, 2)]
        private int m_PlayerID = 1;

        [Header("Movement")]

        [SerializeField]
        private float m_Speed = 6f;

        private void Awake()
        {
            InputActionMap playerMap = m_Actions.FindActionMap(m_PlayerID == 1 ? "Player" : "Player2");
            playerMap.FindAction("Move").performed += OnMove;
            playerMap.FindAction("Jump").performed += OnJump;
            playerMap.FindAction("Throw").performed += OnThrow;
            playerMap.Enable();
        }

        private void Update()
        {
            
        }

        private void OnMove(InputAction.CallbackContext _Context)
        {
            Debug.Log("MOVE " + name + " / " + _Context.ReadValue<Vector2>());
        }

        private void OnJump(InputAction.CallbackContext _Context)
        {
            Debug.Log("JUMP " + name);
        }

        private void OnThrow(InputAction.CallbackContext _Context)
        {
            Debug.Log("THROW " + name);
        }

    }

}