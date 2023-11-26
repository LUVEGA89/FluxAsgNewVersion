using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.ContenedorAnexo
{
    public class ContenedorAnexoManager : Catalog<ContenedorAnexo, int, ContenedorAnexoCriteria>
    {
        public ContenedorAnexoManager()
            :base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindContenedorAnexos";

        protected override ContenedorAnexo LoadItem(IDataReader dr)
        {
            ContenedorAnexo nuevo = new ContenedorAnexo();
            nuevo.Identifier = int.Parse(dr["idAnexo"].ToString());
            nuevo.contenedor_id = int.Parse(dr["contenedor_id"].ToString());
            nuevo.path = (string)dr["path"];
            nuevo.archivo = (string)dr["fileName"];
            nuevo.ext = (string)dr["fileExt"];
            nuevo.dateInsert = (DateTime)dr["dateInsert"];
            nuevo.subPath = (string)dr["subPath"];
            nuevo.usuario = (string)dr["usuario"];
            return nuevo;
        }

        protected override DbCommand PrepareAddStatement(ContenedorAnexo item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddContenedorAnexo");
            this.Database.AddInParameter(cmd, "@idCont", DbType.Int16, item.contenedor_id);
            this.Database.AddInParameter(cmd, "@Ruta", DbType.String, item.path);
            this.Database.AddInParameter(cmd, "@Archivo", DbType.String, item.archivo);
            this.Database.AddInParameter(cmd, "@Ext", DbType.String, item.ext);
            this.Database.AddInParameter(cmd, "@Date", DbType.Date, DateTime.Now);
            this.Database.AddInParameter(cmd, "@subPath", DbType.String, item.subPath);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, item.usuario);

            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetContenedorAnexo");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(ContenedorAnexo item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpContenedorAnexo");
            this.Database.AddInParameter(cmd, "@path", DbType.String, item.path);
            this.Database.AddInParameter(cmd, "@filename", DbType.String, item.archivo);
            this.Database.AddInParameter(cmd, "@ext", DbType.String, item.ext);
            this.Database.AddInParameter(cmd, "@subpath", DbType.String, item.subPath);
            this.Database.AddInParameter(cmd, "@usuario", DbType.String, item.usuario);
            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ContenedorAnexoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            if (criteria.contenedorID != 0)
                this.Database.AddInParameter(cmd, "@idContenedor", DbType.Int16, criteria.contenedorID);
            return cmd;
        }
    }
}
