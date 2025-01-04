using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace ZSN.Utils.Core.Helpers
{
    public class PathHelper
    {
        public static string Combine(params string[] paths)
        {
            var newPaths = new string[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                var p = paths[i];
                if (i!=0 && p.StartsWith('/'))
                {
                    p = p.Substring(1);
                }

                newPaths[i] = p;
            }
            return Path.Combine(newPaths);
        }
    }
}
