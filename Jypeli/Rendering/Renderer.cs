﻿#region MIT License
/*
 * Copyright (c) 2009 University of Jyväskylä, Department of Mathematical
 * Information Technology.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

/*
 * Authors: Tero Jäntti, Tomi Karppinen, Janne Nikkanen.
 */

using System;
using System.Drawing.Drawing2D;
using System.Linq;
using Jypeli.Devices;
using Jypeli.Rendering;

using Matrix = System.Numerics.Matrix4x4;
using Vector3 = System.Numerics.Vector3;

namespace Jypeli
{
    /// <summary>
    /// Luokka, joka sisältää metodeita kuvioiden ja tekstuurien piirtämiseen 2D-tasossa.
    /// </summary>
    /// <remarks>
    /// Toteutus on yksinkertainen ja siten hidas. Jos on paljon samankaltaisia piirrettäviä
    /// kohteita, niin käytä mielummin Batch-luokkia.
    /// </remarks>
    public static class Renderer
    {
        /// <summary>
        /// Onko valaistus käytössä
        /// </summary>
        public static bool LightingEnabled { get; set; }

        /// <summary>
        /// Piirtää kuvan
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="matrix"></param>
        /// <param name="wrapSize"></param>
        public static void DrawImage(Matrix parentTransformation, Image texture, TextureCoordinates tex, Vector position, Vector size, float angle)
        {
            Graphics.CustomBatch.AddImage(parentTransformation, texture, tex, position, size, angle);
        }
        /*
        private static void DrawImageTexture( Image texture, Matrix matrix, GraphicsDevice device, VertexPositionTexture[] tempVertices )
        {
            Effect effect = Graphics.GetTextureEffect( ref matrix, texture.XNATexture, LightingEnabled );
            Graphics.SetSamplerState();

			foreach ( EffectPass pass in effect.CurrentTechnique.Passes )
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    tempVertices, 0, tempVertices.Length,
                    textureTriangleIndices, 0, textureTriangleIndices.Length / 3
                    );
            }

            Graphics.ResetSamplerState();
        }
        */



        /// <summary>
        /// Piirtää kuvion niin, että tekstuuri täyttää sen.
        /// </summary>
        public static void DrawShape( Shape shape, ref Matrix transformation, ref Matrix textureTransformation, Image texture, Vector textureWrapSize, Color color )
        {
            throw new System.Exception("TODO");
            //DrawImage( texture, ref textureTransformation, textureWrapSize );
        }

        /// <summary>
        /// Piirtää tekstiä ruudulle
        /// </summary>
        /// <param name="text">Teksti</param>
        /// <param name="position">Paikka</param>
        /// <param name="font">Fontti</param>
        /// <param name="color">Tekstin väri</param>
        public static void DrawText(string text, Vector position, Font font, Color color, Vector scale)
        {
            Vector textSize = font.SpriteFont.MeasureString(text);
            Graphics.FontRenderer.Begin();
            font.SpriteFont.DrawText(Graphics.FontRenderer, text, position - new Vector(textSize.X/2, 0), color.ToSystemDrawing(), scale);
        }

        /// <summary>
        /// Piirtää tekstiä ruudulle
        /// </summary>
        /// <param name="text">Teksti</param>
        /// <param name="position">Paikka</param>
        /// <param name="font">Fontti</param>
        /// <param name="colors">Tekstin kirjainten väri</param>
        public static void DrawText(string text, Vector position, Font font, Color[] colors, Vector scale)
        {
            Vector textSize = font.SpriteFont.MeasureString(text);
            Graphics.FontRenderer.Begin();
            font.SpriteFont.DrawText(Graphics.FontRenderer, text, position - new Vector(textSize.X / 2, 0), colors.ConvertAll((c) => c.ToSystemDrawing()).ToArray(), scale);
        }

        /// <summary>
        /// Piirtää tekstiä ruudulle
        /// </summary>
        /// <param name="text">Teksti</param>
        /// <param name="transformation">Transformaatiomatriisi</param>
        /// <param name="font">Fontti</param>
        /// <param name="color">Tekstin väri</param>
        public static void DrawText(string text, ref Matrix transformation, Vector position, Font font, Color color, Vector scale)
        {
            Graphics.FontRenderer.Begin(ref transformation);
            font.SpriteFont.DrawText(Graphics.FontRenderer, text, position, color.ToSystemDrawing(), scale);
        }

        /// <summary>
        /// Piirtää tekstiä ruudulle
        /// </summary>
        /// <param name="text">Teksti</param>
        /// <param name="transformation">Transformaatiomatriisi</param>
        /// <param name="font">Fontti</param>
        /// <param name="colors">Tekstin kirjainten väri</param>
        public static void DrawText(string text, ref Matrix transformation, Vector position, Font font, Color[] colors, Vector scale)
        {
            Graphics.FontRenderer.Begin(ref transformation);
            font.SpriteFont.DrawText(Graphics.FontRenderer, text, position, colors.ConvertAll((c) => c.ToSystemDrawing()).ToArray(), scale);
        }

        /// <summary>
        /// Piirtää säteen ruudulle
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="matrix"></param>
        /// <param name="color"></param>
        public static void DrawRaySegment( RaySegment segment, ref Matrix matrix, Color color )
        {

            Vector endPoint = segment.Origin + segment.Direction * segment.Length;

            VertexPositionColorTexture[] colorVertices = new VertexPositionColorTexture[2];
            colorVertices[0] = new VertexPositionColorTexture(new Vector3( (float)segment.Origin.X, (float)segment.Origin.Y, 0 ), color, Vector.Zero);
            colorVertices[1] = new VertexPositionColorTexture(new Vector3( (float)endPoint.X, (float)endPoint.Y, 0 ), color, Vector.Zero);
        }

        /// <summary>
        /// Piirtää suorakulmion.
        /// </summary>
        internal static void DrawRectangle( ref Matrix matrix, Color color )
        {
            Vector[] vertices = new Vector[]
            {
                new Vector(-0.5, 0.5),
                new Vector(-0.5, -0.5),
                new Vector(0.5, -0.5),
                new Vector(0.5, 0.5),
            };
            Renderer.DrawPolygon( vertices, ref matrix, color );
        }

        internal static void DrawFilledShape( ShapeCache cache, ref Matrix matrix, Vector position, Vector size, float rotation, Color color )
        {
            Graphics.CustomBatch.AddShape(matrix, cache, color, position, size, rotation);
        }

        /// <summary>
        /// Piirtää monikulmion ruudulle
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="matrix"></param>
        /// <param name="color"></param>
        public static void DrawPolygon( Vector[] vertices, ref Matrix matrix, Color color )
        {
            if ( vertices.Length < 3 )
                throw new ArgumentException( "Polygon must have at least three vertices" );
            /*
            var device = Game.GraphicsDevice;

            VertexPositionColor[] colorVertices = new VertexPositionColor[vertices.Length];
            for ( int i = 0; i < colorVertices.Length; i++ )
            {
                Vector p = vertices[i];
                colorVertices[i] = new VertexPositionColor(
                    new XnaV3( (float)p.X, (float)p.Y, 0 ),
                    color.AsXnaColor()
                    );
            }

            int n = colorVertices.Length;
            Int16[] indices = new Int16[2 * n];
            for ( int i = 0; i < ( n - 1 ); i++ )
            {
                indices[2 * i] = (Int16)i;
                indices[2 * i + 1] = (Int16)( i + 1 );
            }
            indices[2 * ( n - 1 )] = (Int16)( n - 1 );
            indices[2 * ( n - 1 ) + 1] = (Int16)0;

            Effect effect = Graphics.GetColorEffect( ref matrix, LightingEnabled );
            Graphics.SetSamplerState();
            foreach ( EffectPass pass in effect.CurrentTechnique.Passes )
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    colorVertices, 0, colorVertices.Length,
                    indices, 0, indices.Length - 1
                    );

            }
            */
        }

        /*
        internal static void DrawVertices( Vector[] vertices, Matrix matrix, Color color )
        {
            VertexPositionColor[] pointVertices = new VertexPositionColor[vertices.Length];
            for ( int i = 0; i < pointVertices.Length; i++ )
            {
                Vector p = vertices[i];
                pointVertices[i] = new VertexPositionColor(
                    new XnaV3( (float)p.X, (float)p.Y, 0 ),
                    Color.Red.AsXnaColor()
                    );
            }

            var device = Game.GraphicsDevice;
            device.RenderState.PointSize = 2;

            BasicEffect effect = Graphics.BasicColorEffect;
            effect.World = matrix;
            Graphics.SetSamplerState();
            foreach ( var pass in effect.CurrentTechnique.Passes )
            {
                pass.Apply();
                device.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.Lines,
                    pointVertices, 0, pointVertices.Length
                    );
            }
            Graphics.ResetSamplerState();
        }*/
    }
}

