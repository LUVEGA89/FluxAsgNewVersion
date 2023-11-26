using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Resporting.Service.Core.Proveedor
{
    public class ProveedorArchivoManager : Catalog<ProveedorArchivo, int, ProveedorArchivoFilter>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override ProveedorArchivo LoadItem(IDataReader dr)
        {
            ProveedorArchivo item = new ProveedorArchivo();

            return item;
        }

        protected override DbCommand PrepareAddStatement(ProveedorArchivo item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(ProveedorArchivo item)
        {
            throw new NotImplementedException();
        }
        /*public void AddArchivos(ProveedorArchivo item)
        {
            foreach(var archivo in item.Items)
            {
                DbCommand command = this.Database.GetStoredProcCommand("prAddProveedorArchivo");
                this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, item.IdProveedor);
                this.Database.AddInParameter(command, "@NombreArchivo", DbType.String, archivo.NombreArchivo);
                this.Database.AddInParameter(command, "@RutaArchivo", DbType.String, archivo.RutaArchivo);
                this.Database.AddInParameter(command, "@TipoArchivo", DbType.Int32, archivo.TipoArchivo);
                this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.RegistradoPor);
                this.Database.ExecuteNonQuery(command);
            }
            
        }*/
    }
}