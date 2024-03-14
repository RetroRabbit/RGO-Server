using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.UnitOfWork
{
    public static class GlobalVariables
    {
        private static int UserId;

        public static void SetUserId(int id)
        { 
            UserId = id;
        }
        public static int GetUserId()
        {
            return UserId;
        }
    }
}
