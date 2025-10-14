// COPYRIGHT 1995-2021 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com

using System;
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		internal static DateTimeOffset FromArcGISDateTime(IntPtr value)
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			var milliseconds = RT_DateTime_toUnixMilliseconds(value, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			errorHandler = ArcGISErrorManager.CreateHandler();

			RT_DateTime_destroy(value, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
		}

		internal static IntPtr ToArcGISDateTime(DateTimeOffset value)
		{
			var milliseconds = value.ToUnixTimeMilliseconds();

			var errorHandler = ArcGISErrorManager.CreateHandler();

			var result = RT_DateTime_fromUnixMilliseconds(milliseconds, errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			return result;
		}

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_DateTime_fromUnixMilliseconds(long time, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern long RT_DateTime_toUnixMilliseconds(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_DateTime_destroy(IntPtr handle, IntPtr errorHandle);
	}
}
