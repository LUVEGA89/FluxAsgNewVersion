using Reporting.Service.Core.Quotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ProveedorServicios
{
    public class ProveedorServicioManager : Catalog<ProveedorServicio, int, ProveedorServicioCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindServiciosProveedores";

        protected override ProveedorServicio LoadItem(IDataReader dr)
        {
            ProveedorServicio proveedorServicio = new ProveedorServicio
            {
                Identifier = (int)dr["Sequence"],
                Nombre = (string)dr["Nombre"]
            };
            
            return proveedorServicio;
        }

        protected override DbCommand PrepareAddStatement(ProveedorServicio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetServicioProveedor");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ProveedorServicioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Activo", DbType.Int32, criteria.Activo);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(ProveedorServicio item)
        {
            throw new NotImplementedException();
        }
    }
}
