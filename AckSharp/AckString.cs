﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AckSharp
{
	public sealed partial class AckString : EngineObject
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="initalValue"></param>
		/// <remarks>str_create</remarks>
		public AckString(string initalValue)
			: base(ObjectType.String, true, Native.NativeMethods.StrCreate(initalValue))
		{

		}

		public AckString(IntPtr handle)
			: base(ObjectType.String, false, handle)
		{

		}

		/// <summary>
		/// Copies the value into the string.
		/// </summary>
		/// <param name="value"></param>
		/// <remarks>str_cpy</remarks>
		public void SetString(string value)
		{
			CheckValid();
			Native.NativeMethods.StrCpy(this.InternalPointer, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="font"></param>
		/// <returns></returns>
		/// <remarks>str_width</remarks>
		public ackvar GetWidth(Font font)
		{
			CheckValid();
			return Native.NativeMethods.StrWidth(this.Characters, font);
		}
	}
}
