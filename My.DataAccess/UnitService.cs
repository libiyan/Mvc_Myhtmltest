using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucsmy.Framework.DAL;
namespace My.DataAccess
{
    public class UnitService
    {
        class UnitOfWorkRepository : UnitOfWorkRepositoryBase
        {
            public UnitOfWorkRepository(string dbKey) : base(dbKey) { }
        }

        public static IUnitOfWorkRepository Finance(string dbkey)
        {
            return new UnitOfWorkRepository(dbkey);
        }
    }
}
