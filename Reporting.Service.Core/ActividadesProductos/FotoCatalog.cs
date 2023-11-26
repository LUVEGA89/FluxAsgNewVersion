using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class FotoCatalog : Catalog<ActividadProductosFotos,int,FotoCriteria>
    {
        public FotoCatalog()
        {
        }
        public FotoCatalog(string database)
            :base(database)
        {
        }

        //protected override string FindPagedItemsProcedure => throw new NotImplementedException();
        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadProductosFotos"; }
        }
        protected override ActividadProductosFotos LoadItem(IDataReader dr)
        {
            ActividadProductosFotos foto = new ActividadProductosFotos();
            foto.Identifier = (int)dr["Identificador"];
            foto.Actividad = (int)dr["Actividad"];
            foto.Foto = (string)dr["Foto"];
            foto.Estatus = (bool)dr["Estatus"];
            return foto;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(ActividadProductosFotos item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividadProductosImagen");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(ActividadProductosFotos item)
        {
            throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindPagedItemsStatement(FotoCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@IdActividad", DbType.Int32, Criteria.ActividadFoto);
            return cmd;
        }
    }
}
