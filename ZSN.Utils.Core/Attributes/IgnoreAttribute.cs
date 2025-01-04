using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZSN.Utils.Core.Attributes
{
    public class IgnoreAttribute : Attribute, IFilterMetadata
    {
    }
}
