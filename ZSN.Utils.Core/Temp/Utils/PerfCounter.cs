#region License
/*
 * Yannyo!NT Version: 2.0
 * .NET Framework Version: 2.0
 * Created on 2007-5-31
 *
 * Web: http://www.discuznt.com
 * Copyright (C) 2001 - 2007 Comsenz Technology Inc., All Rights Reserved.
 * This is NOT a freeware, use is subject to license terms.
 *
 */
#endregion

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Yannyo.Common
{
    class PerfCounter
    {

		#region 私有变量

		private long freq;
		private long startTime, stopTime;

		#endregion

        #region 私有方法

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
		
		#endregion

		#region 构造方法

		/// <summary>
		/// 创建类的实例
		/// </summary>
		/// <param name="startTimer">是否立即执行Start()方法</param>
        public PerfCounter(bool startTimer)
		{
			startTime = 0;
			stopTime = 0;

			if (QueryPerformanceFrequency(out freq) == false)
			{
				// 不支持高精度计时
				throw new Win32Exception();
			}

			if (startTimer)
				Start();
		}

		#endregion

		#region 公有方法

		/// <summary>
		/// 停止计数
		/// </summary>
		public void Stop()
		{
			QueryPerformanceCounter(out stopTime);
		}

		/// <summary>
		/// 开始计时
		/// </summary>
		public void Start()
		{
			Thread.Sleep(0);
			QueryPerformanceCounter(out startTime);
		}

		#endregion

		#region 属性

		/// <summary>
		/// 返回运行时间
		/// </summary>
		/// <value>运行时间</value>
		public double Duration
		{
			get { return (double) (stopTime - startTime)/(double) freq; }
		}

		#endregion
    }
}
