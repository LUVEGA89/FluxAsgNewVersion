using Reporting.Service.Core.MenuByUser.Modulo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.MenuByUser.Menu
{
    public class MenuCatalog : Catalog<Menu, string, MenuCritria>
    {
        ModuloCatalog moduloCatalog = new ModuloCatalog();
        protected override string FindPagedItemsProcedure
        {
            get { return "cau.prFindMenuByUser"; }
        }

        protected override Menu LoadItem(IDataReader dr)
        {
            Menu menu = new Menu();
            //dr.NextResult();
            //while (dr.Read())
            //{
            //    menu.AddModulo(this.Find((int)dr["Sequence"]));
            //}
            return menu;
        }

        protected override DbCommand PrepareAddStatement(Menu item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetMenuByUser");
            this.Database.AddInParameter(command, "@IdModulo", DbType.String, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Menu item)
        {
            throw new NotImplementedException();
        }
    }
}
