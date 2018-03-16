//-----------------------------------------------------------------------
// <copyright file="ARCoreBackgroundRenderer.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore
{
    using System.Collections;
    using System.Collections.Generic;
    using GoogleARCoreInternal;
    using UnityEngine;
    using UnityEngine.XR;

    /// <summary>
    /// Renders the device's camera as a background to the attached Unity camera component.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ARCoreBackgroundRenderer : MonoBehaviour
    {
        /// <summary>
        /// A material used to render the AR background image.
        /// </summary>
        [Tooltip("A material used to render the AR background image.")]
        public Material BackgroundMaterial;

        private Camera m_Camera;

        private ARBackgroundRenderer m_BackgroundRenderer;

        private void OnEnable()
        {
            if (BackgroundMaterial == null)
            {
                Debug.LogError("ArCameraBackground:: No material assigned.");
                return;
            }

            m_Camera = GetComponent<Camera>();
        }

        private void OnDisable()
        {
            Disable();
        }
		/*
		//BINU START
		public static Texture2D SetGrayscale(Texture2D t)
		{
			Texture2D tex_ = new Texture2D(t.width, t.height, TextureFormat.ARGB32, true);
			Color[] colors = t.GetPixels();
			for (int i = 0; i < colors.Length; i++)
				colors[i] = new Color((colors[i].r + colors[i].g + colors[i].b) / 3, (colors[i].r + colors[i].g + colors[i].b) / 3, (colors[i].r + colors[i].g + colors[i].b) / 3);

			tex_.SetPixels(colors);
			tex_.Apply();
			return tex_;
		}
		*/
		//backgroundTexture.filterMode 
		//BINU END

        private void Update()
        {
            if (BackgroundMaterial == null)
            {
                Disable();
                return;
            }

            Texture backgroundTexture = Frame.CameraImage.Texture;
			/*BINU START
			//backgroundTexture;
			Debug.Log ("BINU Render Start ");
			Texture2D x = (Texture2D)  Frame.CameraImage.Texture;
			Texture2D y = SetGrayscale (x);
			Texture backgroundTexture = (Texture)y;
			Debug.Log ("BINU Render END ");
			//BINU END */

            if (backgroundTexture == null)
            {
                Disable();
                return;
            }
			Debug.Log ("BINU Render Continue ");

            const string mainTexVar = "_MainTex";
            const string topLeftRightVar = "_UvTopLeftRight";
            const string bottomLeftRightVar = "_UvBottomLeftRight";
			  
            BackgroundMaterial.SetTexture(mainTexVar, backgroundTexture);

            var uvQuad = Frame.CameraImage.DisplayUvCoords;
            BackgroundMaterial.SetVector(topLeftRightVar,
                new Vector4(uvQuad.TopLeft.x, uvQuad.TopLeft.y, uvQuad.TopRight.x, uvQuad.TopRight.y));
            BackgroundMaterial.SetVector(bottomLeftRightVar,
                new Vector4(uvQuad.BottomLeft.x, uvQuad.BottomLeft.y, uvQuad.BottomRight.x, uvQuad.BottomRight.y));
			//BINU START 
            //m_Camera.projectionMatrix = Frame.CameraImage.GetCameraProjectionMatrix(
              //  m_Camera.nearClipPlane, m_Camera.farClipPlane);

			Matrix4x4 p = Frame.CameraImage.GetCameraProjectionMatrix(m_Camera.nearClipPlane, m_Camera.farClipPlane);
			p.m01 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
			p.m10 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
			m_Camera.projectionMatrix = p;
			//BINU END 


            if (m_BackgroundRenderer == null)
            {
                m_BackgroundRenderer = new ARBackgroundRenderer();
                m_BackgroundRenderer.backgroundMaterial = BackgroundMaterial;
                m_BackgroundRenderer.camera = m_Camera;
                m_BackgroundRenderer.mode = ARRenderMode.MaterialAsBackground;
            }
			//Frame.CameraImage.GetCameraProjectionMatrix(
        }

        private void Disable()
        {
            if (m_BackgroundRenderer != null)
            {
                m_BackgroundRenderer.camera = null;
                m_BackgroundRenderer = null;
            }
        }
    }
}
