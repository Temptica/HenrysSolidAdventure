using System;
using Microsoft.Xna.Framework;

namespace HenrySolidAdventure.Graphics
{
    public sealed class Camera
    {
        public readonly static float MinZ = 1f;
        public readonly static float MaxZ = 2048f;

        private Vector2 position;
        private float baseZ;
        private float z;

        private float aspectRatio;
        private float fieldOfView;

        private Matrix view;
        private Matrix proj;

        private int zoom;

        public Vector2 Position
        {
            get { return position; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return proj; }
        }

        public Camera(Screen screen)
        {
            if(screen is null)
            {
                throw new ArgumentNullException("screen");
            }

            aspectRatio = (float)screen.Width / screen.Height;
            fieldOfView = MathHelper.PiOver2;

            position = new Vector2(-screen.Width/2f,-screen.Height/2f);
            baseZ = -GetZFromHeight(screen.Height);
            z = baseZ;

            UpdateMatrices();

            zoom = 1;
        }

        public void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(new Vector3(0,0, z), Vector3.Zero, Vector3.Down);
            proj = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, MinZ, MaxZ);
        }

        public float GetZFromHeight(float height)
        {
            return (0.5f * height) / MathF.Tan(0.5f * fieldOfView);
        }

        public float GetHeightFromZ()
        {
            return z * MathF.Tan(0.5f * fieldOfView) * 2f;
        }

        public void MoveTo(Vector2 position)
        {
            this.position = position;
        }
        

        public void GetExtents(out float width, out float height)
        {
            height = GetHeightFromZ();
            width = height * aspectRatio;
        }

        public void GetExtents(out float left, out float right, out float bottom, out float top)
        {
            GetExtents(out float width, out float height);

            left = position.X - width * 0.5f;
            right = left + width;
            bottom = position.Y - height * 0.5f;
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
