﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AckSharp
{
	public static class Graphics
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		/// <remarks>draw_line</remarks>
		public static void DrawLine(Vector from, Color? color, ackvar alpha)
		{
			if (color != null)
			{
				var c = color.Value;
				Native.NativeMethods.DrawLine(ref from, ref c, alpha);
			}
			else
			{
				Native.NativeMethods.DrawLine(ref from, IntPtr.Zero, alpha);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		/// <remarks>draw_line3d</remarks>
		public static void DrawLine3D(Vector from, Color? color, ackvar alpha)
		{
			if (color != null)
			{
				var c = color.Value;
				Native.NativeMethods.DrawLine3d(ref from, ref c, alpha);
			}
			else
			{
				Native.NativeMethods.DrawLine3d(ref from, IntPtr.Zero, alpha);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		/// <param name="size"></param>
		/// <remarks>draw_line3d</remarks>
		public static void DrawPoint3D(Vector position, Color color, ackvar alpha, ackvar size)
		{
			Native.NativeMethods.DrawPoint3d(ref position, ref color, alpha, size);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vMin"></param>
		/// <param name="vMax"></param>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		/// <remarks>draw_box3d</remarks>
		public static void DrawBox3D(Vector vMin, Vector vMax, Color color, ackvar alpha)
		{
			Native.NativeMethods.DrawBox3d(ref vMin, ref vMax, ref color, alpha);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="@object"></param>
		/// <remarks>draw_obj</remarks>
		public static void DrawObject(EngineObject @object)
		{
			Native.NativeMethods.DrawObj(@object);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>draw_begin</remarks>
		public static void DrawBegin()
		{
			Native.NativeMethods.DrawBegin();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="position"></param>
		/// <param name="offset"></param>
		/// <param name="size"></param>
		/// <param name="scale"></param>
		/// <param name="color"></param>
		/// <param name="alpha"></param>
		/// <param name="angle"></param>
		/// <remarks>draw_quad</remarks>
		public static void DrawQuad(Bitmap bitmap, Vector position, Vector? offset, Vector? size, Vector? scale, Color? color, ackvar alpha, ackvar angle)
		{
			Vector vOffset = offset ?? new Vector(0, 0, 0);
			Vector vSize = size ?? (bitmap != null ? new Vector(bitmap.Width, bitmap.Height) : new Vector(0, 0));
			Vector vScale = scale ?? new Vector(1, 1);
			Color vColor = color ?? new Color(255, 255, 255);

			Native.NativeMethods.DrawQuad(
				bitmap,
				ref position,
				ref vOffset,
				ref vSize,
				ref vScale,
				ref vColor,
				alpha,
				angle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="color"></param>
		/// <remarks>draw_text</remarks>
		public static void DrawText(double x, double y, Color color, string text)
		{
			Native.NativeMethods.DrawText(text, x, y, ref color);
		}

		public static void DrawText(double x, double y, Color color, string text, params object[] args)
		{
			DrawText(x, y, color, string.Format(text, args));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fontName"></param>
		/// <param name="mode"></param>
		/// <param name="height"></param>
		/// <param name="alpha"></param>
		/// <remarks>draw_textmode</remarks>
		public static void TextMode(string fontName, FontMode mode, ackvar height, ackvar alpha)
		{
			Native.NativeMethods.DrawTextmode(fontName, (ackvar)(int)mode, height, alpha);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="num"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		/// <remarks>bmap_rendertarget</remarks>
		public static bool SetRenderTarget(Bitmap bitmap, ackvar num, ackvar mode)
		{
			return (bool)Native.NativeMethods.BmapRendertarget(bitmap, num, mode);
		}

		/// <summary>
		/// Invokes an effect
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="count"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <remarks>effect</remarks>
		public static void Effect(IntPtr effect, int count, Vector position, Vector velocity)
		{
			if (count >= 1 && effect != IntPtr.Zero)
			{
				Native.NativeMethods.Effect(effect, count, ref position, ref velocity);
			}
		}

		/// <summary>
		/// Invokes an effect
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="count"></param>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <remarks>effect</remarks>
		public static void Effect(ParticleEffect effect, int count, Vector position, Vector velocity)
		{
			if (count >= 1 && effect != null)
			{
				var ptr = CreateParticleEvent(effect);
				Native.NativeMethods.Effect(ptr, count, ref position, ref velocity);
			}
		}

		/// <summary>
		/// Creates a wrapper around the effect delegate and returns a native function pointer to it.
		/// </summary>
		/// <param name="effect"></param>
		/// <returns></returns>
		public static IntPtr CreateParticleEvent(ParticleEffect effect)
		{
			if (effect == null)
				return IntPtr.Zero;
			// Transpose particle pointer into engine object.
			NativeParticleEffect native = (ptr) =>
				{
					Particle particle = EngineObject.Get<Particle>(ptr);
					effect(particle);
				};
			return LiteC.GetMethodPointer(native);
		}
	}

	public delegate void ParticleEffect(Particle particle);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	public delegate void NativeParticleEffect(IntPtr particle);

	public enum FontMode
	{
		Default = 0,
		Bold = 1,
		Italic = 2,
		BoldItalic = 3
	}
}
