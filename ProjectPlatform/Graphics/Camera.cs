using System;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Graphics
{
    public sealed class Camera
    {//from https://www.youtube.com/watch?v=yUSB_wAVtE8 and own previous project
        public static readonly float MinZ = 1f;
        public static readonly float MaxZ = 2048f;

        private Vector2 _position;
        private readonly float _baseZ;
        private readonly float _z;

        private readonly float _aspectRatio;
        private readonly float _fieldOfView;

        public Vector2 Position => _position;

        public Matrix View { get; private set; }

        public Matrix Projection { get; private set; }

        public Camera(Screen screen)
        {
            if(screen is null)
            {
                throw new ArgumentNullException(nameof(screen));
            }

            _aspectRatio = (float)screen.Width / screen.Height;
            _fieldOfView = MathHelper.PiOver2;

            _position = new Vector2(-screen.Width/2f,-screen.Height/2f);
            _baseZ = -GetZFromHeight(screen.Height);
            _z = _baseZ;

            UpdateMatrices();
        }

        public void UpdateMatrices()
        {
            View = Matrix.CreateLookAt(new Vector3(0,0, _z), Vector3.Zero, Vector3.Down);
            Projection = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, MinZ, MaxZ);
        }

        public float GetZFromHeight(float height)
        {
            return (0.5f * height) / MathF.Tan(0.5f * _fieldOfView);
        }

        public float GetHeightFromZ()
        {
            return _z * MathF.Tan(0.5f * _fieldOfView) * 2f;
        }

        public void MoveTo(Vector2 position)
        {
            _position = position;
        }
        

        public void GetExtents(out float width, out float height)
        {
            height = GetHeightFromZ();
            width = height * _aspectRatio;
        }

        public void GetExtents(out float left, out float right, out float bottom, out float top)
        {
            GetExtents(out float width, out float height);

            left = _position.X - width * 0.5f;
            right = left + width;
            bottom = _position.Y - height * 0.5f;
            top = bottom + height;
        }

        public void GetExtents(out Vector2 min, out Vector2 max)
        {
            GetExtents(out float left, out float right, out float bottom, out float top);
            min = new Vector2(left, bottom);
            max = new Vector2(right, top);
        }

    }
}
