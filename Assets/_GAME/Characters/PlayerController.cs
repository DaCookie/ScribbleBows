using UnityEngine;

namespace Game
{

    ///<summary>
    /// 
    ///</summary>
    //[AddComponentMenu("_GAME/PlayerController")]
    public class PlayerController : MonoBehaviour
    {

        [Header("General Settings")]

        [SerializeField, Range(1, 2)]
        private int m_PlayerID = 1;

        [Header("Movement")]

        [SerializeField]
        private float m_Speed = 6f;

        private void Update()
        {
            Move(Time.deltaTime);
        }

        public void Move(float _DeltaTime)
        {
            float axis = 0f;

            if (Input.GetKey(KeyCode.Q))
            {
                axis = -1f;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                axis = 1f;
            }

            if(axis != 0f)
            {
                transform.position += Vector3.right * axis * m_Speed * _DeltaTime;
            }
        }

    }

}