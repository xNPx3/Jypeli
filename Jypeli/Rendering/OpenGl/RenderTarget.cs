﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.OpenGL;

namespace Jypeli.Rendering.OpenGl
{
    public unsafe class RenderTarget
    {
        private GL gl;

        private uint framebufferHandle;
        private uint texturebufferHandle;

        public double Width { get; set; }
        public double Height { get; set; }

        public RenderTarget(uint width, uint height)
        {
            gl = GraphicsDevice.Gl;
            framebufferHandle = gl.GenFramebuffer();
            gl.BindFramebuffer(GLEnum.Framebuffer, framebufferHandle);

            texturebufferHandle = gl.GenTexture();

            gl.BindTexture(GLEnum.Texture2D, texturebufferHandle);

            // Tässä pitää jostain syystä käyttää InternalFormat-enumia, mutta muualla voi käyttää GLEnumia...
            gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgb, width, height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, null);

            gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
            gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Linear);

            gl.FramebufferTexture2D(GLEnum.Framebuffer, GLEnum.ColorAttachment0, GLEnum.Texture2D, texturebufferHandle, 0);

            uint rbo = gl.GenRenderbuffer();
            gl.BindRenderbuffer(GLEnum.Renderbuffer, rbo);
            gl.RenderbufferStorage(GLEnum.Renderbuffer, GLEnum.Depth24Stencil8, width, height);
            gl.FramebufferRenderbuffer(GLEnum.Framebuffer, GLEnum.DepthStencilAttachment, GLEnum.Renderbuffer, rbo);

            gl.BindRenderbuffer(GLEnum.Renderbuffer, 0);

            gl.GetError();

            if(gl.CheckFramebufferStatus(GLEnum.Framebuffer) == GLEnum.FramebufferComplete)
            {
                Debug.WriteLine("Framebuffer is not complete");
            }

            gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        }

        public void Bind()
        {
            gl.BindFramebuffer(GLEnum.Framebuffer, framebufferHandle);
        }

        public void UnBind()
        {
            gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        }

        public void BindTexture()
        {
            gl.BindTexture(GLEnum.Texture2D, texturebufferHandle);
        }

        public void UnBindTexture()
        {
            gl.BindTexture(GLEnum.Texture2D, 0);
        }
    }
}
