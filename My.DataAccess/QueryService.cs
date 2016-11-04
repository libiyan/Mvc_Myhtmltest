using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucsmy.Framework.DAL;
namespace My.DataAccess
{
    public static class QueryService
    {
        public static IQueryHelper Platform
        {
            get { return new QueryHelper("PlatformEntities"); }
        }

        public static IQueryHelper QianTu
        {
            get { return new QueryHelper("QianTuEntities"); }
        }
    }
}
