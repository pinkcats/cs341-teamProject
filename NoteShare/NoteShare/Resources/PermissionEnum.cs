using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteShare.Resources
{
    public enum PermissionEnum
    {
        VIEWER = 0,
        USER = 1,
        ADMIN = 2
    }

    public static class Permissions {
        public static PermissionEnum GetPermissionFromValue(int value)
        {
            return (PermissionEnum)value;
        }
    }
}