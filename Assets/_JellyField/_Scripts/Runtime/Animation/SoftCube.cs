using DG.Tweening;
using Runtime.Manager;
using Runtime.ScriptTableObject;
using UnityEngine;

namespace Runtime.Animation
{
    public class SoftCube : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;
        private Mesh _mesh;

        public float upDownFactor = 0.1f;
        public float upDownSpeed = 6f; 

        public float leftFactor = 1f;
        public float leftSpeed = 5f;
        public float leftOffset = 1f;

        public float strectFactor = -0.1f;
        public float strectSpeed = 2f;


        private Vector3 _originalScale;
        void Start()
        {
            _mesh = new Mesh();
            _mesh.name = "NewMesh";
            _mesh.vertices = NewVertCube();
            _mesh.triangles = NewTrisCube();
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            meshFilter.mesh = _mesh;
            SetStatsAnimation(AnimationManager.Instance.defaultSetting);
            _originalScale = transform.localScale;
        }
        private Vector3[] NewVertCube(float up = 0f, float left = 0f, float stretch = 0f)
        {
            return new Vector3[]
            {
                //Bottom
                new Vector3(-1,0,1),
                new Vector3(1,0,1),
                new Vector3(1,0,-1),
                new Vector3(-1,0,-1),

                //Top
                new Vector3(-1 - stretch + left, 2 + up, 1 + stretch),
                new Vector3(1 + stretch + left, 2 + up, 1 + stretch),
                new Vector3(1 + stretch + left, 2 + up, -1 - stretch),
                new Vector3(-1 - stretch + left, 2 + up, -1 - stretch),
            };
        } 
        private int[] NewTrisCube()
        {
            return new int[]
            {
                //Bottom
                2,1,0,
                2,0,3,

                //Top
                4,5,6,
                4,6,7,

                //front
                2,7,6,
                2,3,7,

                //back
                4,0,1,
                4,1,5,

                //left
                0,4,3,
                3,4,7,

                //right
                1,2,6,
                1,6,5,
            };
        }

        void FixedUpdate()
        {
            _mesh.vertices = NewVertCube(Mathf.Sin(Time.realtimeSinceStartup * upDownSpeed) * upDownFactor,
                Mathf.Sin(Time.realtimeSinceStartup * leftSpeed + leftOffset) * leftFactor,
                Mathf.Sin(Time.realtimeSinceStartup * strectSpeed) * strectFactor
            );
        }

        public void KillDoTween()
        {
            DOTween.Kill(gameObject);
            SetStatsAnimation(AnimationManager.Instance.defaultSetting);
        }
        public void ShakeLightOnce()
        {
            DOTween.Kill(gameObject);
            SetStatsAnimation(AnimationManager.Instance.shakeLightOnce);
            // transform.DOShakePosition(1f, 0, 0, 0).OnComplete(() =>
            // {
            //     SetStatsAnimation(AnimationManager.Instance.defaultSetting);
            // });
        }
        public void ScaleUp()
        {
            transform.DOScale(_originalScale, 0.2f).SetEase(Ease.OutBounce);
        }
        public void ScaleDown()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutBounce);
        }
        private void SetStatsAnimation(AnimationSetting setting)
        {
            upDownFactor = setting.upDownFactor;
            upDownSpeed = setting.upDownSpeed;
            leftFactor = setting.leftFactor;
            leftSpeed = setting.leftSpeed;
            leftOffset = setting.leftOffset;
            strectFactor = setting.strectFactor;
            strectSpeed = setting.strectSpeed;
        }
    }
}