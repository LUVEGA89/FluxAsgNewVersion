using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.Seguimiento
{
    public class SeguimientoManager : Catalog<Seguimiento, int, SeguimientoCriteria>
    {
        public SeguimientoManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindContenedorSeguimiento";

        protected override Seguimiento LoadItem(IDataReader dr)
        {
            Seguimiento nuevo = new Seguimiento();
            nuevo.Identifier = int.Parse(dr["idSeguimiento"].ToString());
            nuevo.embarcada = DBNull.Value.Equals(dr["embarcada"]) ? DateTime.MinValue : (DateTime)dr["embarcada"];
            nuevo.llegadaPuerto = DBNull.Value.Equals(dr["llegadaPuerto"]) ? DateTime.MinValue : (DateTime)dr["llegadaPuerto"];
            nuevo.salidaPuerto = DBNull.Value.Equals(dr["salidaPuerto"]) ? DateTime.MinValue : (DateTime)dr["salidaPuerto"];
            nuevo.llegadaPantaco = DBNull.Value.Equals(dr["llegadaPantaco"]) ? DateTime.MinValue : (DateTime)dr["llegadaPantaco"];
            nuevo.salidaPantaco = DBNull.Value.Equals(dr["salidaPantaco"]) ? DateTime.MinValue : (DateTime)dr["salidaPantaco"];
            nuevo.libTransito = DBNull.Value.Equals(dr["libTransito"]) ? DateTime.MinValue : (DateTime)dr["libTransito"];
            nuevo.libDespacho = DBNull.Value.Equals(dr["libDespacho"]) ? DateTime.MinValue : (DateTime)dr["libDespacho"];
            nuevo.usuario = (string)dr["Usuario"];
            return nuevo;
        }

        protected override DbCommand PrepareAddStatement(Seguimiento item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetContenedorSeguimiento");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Seguimiento item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpContenedorSeguimiento");
            this.Database.AddInParameter(cmd, "@id", DbType.Int16, item.Identifier);

            if (item.embarcada.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@embarcada", DbType.Date, item.embarcada);

            if (item.llegadaPuerto.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@llegaPuerto", DbType.Date, item.llegadaPuerto);

            if (item.salidaPuerto.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@salePuerto", DbType.Date, item.salidaPuerto);

            if (item.llegadaPantaco.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@llegaPantaco", DbType.Date, item.llegadaPantaco);

            if (item.salidaPantaco.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@salePantaco", DbType.Date, item.salidaPantaco);

            if (item.libTransito.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@libTransito", DbType.Date, item.libTransito);

            if (item.libDespacho.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@libDespacho", DbType.Date, item.libDespacho);
            if (item.usuario != null && item.usuario != "")
                this.Database.AddInParameter(cmd, "@Usuario", DbType.String, item.usuario);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(SeguimientoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            //Busqueda por rango de fechas
            if (criteria.fecIni.ToShortDateString() != "01/01/0001" && criteria.fecFin.ToShortDateString() != "01/01/0001")
            {
                this.Database.AddInParameter(cmd, "@InicioPuerto", DbType.Date, criteria.fecIni);
                this.Database.AddInParameter(cmd, "@FinPuerto", DbType.Date, criteria.fecFin);
            }
            return cmd;
        }
    }
}
