using Reporting.Service.Core.Trafico.Contenedor.Naviera;
using Reporting.Service.Core.Trafico.Contenedor.Seguimiento;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor
{
    public class ContenedorManager : Catalog<Contenedor, int, ContenedorCriteria>
    {
        public ContenedorManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindContenedores";

        protected override Contenedor LoadItem(IDataReader dr)
        {
            Contenedor nuevo = new Contenedor();

            nuevo.Identifier = (int)dr["idContenedor"];
            nuevo.nomContenedor = (string)dr["nomContenedor"];
            nuevo.fechaCrea = (DateTime)dr["fechaCreacion"];
            NavieraManager navi = new NavieraManager();
            nuevo.naviera = navi.Find((string)dr["Parkoiwa2009OCRD_CardCode"]);
            nuevo.statusContenedor_id = int.Parse(dr["statusContenedor_id"].ToString());
            nuevo.seguimiento_id = int.Parse(dr["seguimiento_id"].ToString());
            nuevo.usuario = (string)dr["Usuario"];
            nuevo.tipoCambio = (decimal)dr["tipoCambio"];
            nuevo.flete = (decimal)dr["CostoFlete"];
            nuevo.LlegadaPuerto = dr["LlegadaPuerto"] == DBNull.Value ? "NA" : ((DateTime)dr["LlegadaPuerto"]).ToString("MM/dd/yyyy");
            nuevo.LibresAlmacenaje = dr["LibresAlmacenaje"] == DBNull.Value ? "NA" : ((int)dr["LibresAlmacenaje"]).ToString();
            nuevo.LibresDemoras = dr["LibresDemoras"] == DBNull.Value ? "NA" : ((string)dr["LibresDemoras"]).ToString();
            nuevo.FleteMarino = dr["FleteMarino"] == DBNull.Value ? "" : ((decimal)dr["FleteMarino"]).ToString();
            if (nuevo.LlegadaPuerto != "NA" && nuevo.LibresAlmacenaje != "NA")
            {
                DateTime Fecha = (DateTime)dr["LlegadaPuerto"];
                nuevo.LibresAlmacenaje = Fecha.AddDays((int)dr["LibresAlmacenaje"]).ToString("MM/dd/yyyy");

            }

            if (nuevo.LlegadaPuerto != "NA" && nuevo.LibresDemoras != "NA")
            {
                DateTime Fecha = (DateTime)dr["LlegadaPuerto"];
                nuevo.LibresDemoras = Fecha.AddDays(int.Parse((string)dr["LibresDemoras"])).ToString("MM/dd/yyyy");

            }

            return nuevo;
        }

        protected override DbCommand PrepareAddStatement(Contenedor item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddContenedor");
            this.Database.AddInParameter(cmd, "@nomContenedor", DbType.String, item.nomContenedor);
            this.Database.AddInParameter(cmd, "@fechaCreacion", DbType.Date, item.fechaCrea);
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, item.naviera.Identifier);
            this.Database.AddInParameter(cmd, "@statusContenedor_id", DbType.Int16, item.statusContenedor_id);
            this.Database.AddInParameter(cmd, "@usuario", DbType.String, item.usuario);
            this.Database.AddInParameter(cmd, "@cambio", DbType.Decimal, item.tipoCambio);
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetContenedor");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Contenedor item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpContenedor");
            this.Database.AddInParameter(cmd, "@id", DbType.Int16, item.Identifier);
            if (item.statusContenedor_id != 0)
                this.Database.AddInParameter(cmd, "@status", DbType.Int16, item.statusContenedor_id);
            if (item.tipoCambio != 0)
                this.Database.AddInParameter(cmd, "@cambio", DbType.Decimal, item.tipoCambio);
            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ContenedorCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            //Busqueda por seguimientoID
            if (criteria.seguimiento_id != 0)
                this.Database.AddInParameter(cmd, "@Seguimiento", DbType.Int16, criteria.seguimiento_id);
            //Busqueda por nombre parcial "LIKE"
            if (criteria.nomParcial != "" && criteria.nomParcial != null)
                this.Database.AddInParameter(cmd, "@NombreParcial", DbType.String, criteria.nomParcial);
            //Busqueda por nombre completo
            if (criteria.nomCompleto != "" && criteria.nomCompleto != null)
                this.Database.AddInParameter(cmd, "@NombreCompleto", DbType.String, criteria.nomCompleto);
            //Busqueda por rango de status
            if (criteria.statusIni != 0 && criteria.statusFin != 0)
            {
                this.Database.AddInParameter(cmd, "@StatusIni", DbType.Int16, criteria.statusIni);
                this.Database.AddInParameter(cmd, "@StatusFin", DbType.Int16, criteria.statusFin);
            }
            //Busqueda por status especifico 
            if (criteria.status != 0)
                this.Database.AddInParameter(cmd, "@Status", DbType.Int16, criteria.status);
            //Busqueda por rango de fechas
            if (criteria.fecIni.ToShortDateString() != "01/01/0001" && criteria.fecFin.ToShortDateString() != "01/01/0001")
            {
                this.Database.AddInParameter(cmd, "@fecIni", DbType.Date, criteria.fecIni);
                this.Database.AddInParameter(cmd, "@fecFin", DbType.Date, criteria.fecFin);
            }
            return cmd;
        }

        public DataTable GetDetallesExcel(DateTime Del, DateTime Al, int Estado, int Tipo)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLlegadasDetalleExcel");
            if (Del.ToShortDateString() != "01/01/0001" && Al.ToShortDateString() != "01/01/0001")
            {
                this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
                this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            }
            if (Estado != 0)
                this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, Estado);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }

        public List<ContenedorCheckBox> GetListContenedores(ContenedorCriteria criteria)
        {
            List<ContenedorCheckBox> listaContenedores = new List<ContenedorCheckBox>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetContenedorCheckBox");
            if (criteria.seguimiento_id != 0)
                this.Database.AddInParameter(cmd, "@Seguimiento", DbType.Int16, criteria.seguimiento_id);
            //Busqueda por nombre parcial "LIKE"
            if (criteria.nomParcial != "" && criteria.nomParcial != null)
                this.Database.AddInParameter(cmd, "@NombreParcial", DbType.String, criteria.nomParcial);
            //Busqueda por nombre completo
            if (criteria.nomCompleto != "" && criteria.nomCompleto != null)
                this.Database.AddInParameter(cmd, "@NombreCompleto", DbType.String, criteria.nomCompleto);
            //Busqueda por rango de status
            if (criteria.statusIni != 0 && criteria.statusFin != 0)
            {
                this.Database.AddInParameter(cmd, "@StatusIni", DbType.Int16, criteria.statusIni);
                this.Database.AddInParameter(cmd, "@StatusFin", DbType.Int16, criteria.statusFin);
            }
            //Busqueda por status especifico 
            if (criteria.status != 0)
                this.Database.AddInParameter(cmd, "@Status", DbType.Int16, criteria.status);
            //Busqueda por rango de fechas
            if (criteria.fecIni.ToShortDateString() != "01/01/0001" && criteria.fecFin.ToShortDateString() != "01/01/0001")
            {
                this.Database.AddInParameter(cmd, "@fecIni", DbType.Date, criteria.fecIni);
                this.Database.AddInParameter(cmd, "@fecFin", DbType.Date, criteria.fecFin);
            }

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                listaContenedores.Add(new ContenedorCheckBox
                {
                    Identifier = (int)dr["idContenedor"],
                    nomContenedor = (string)dr["nomContenedor"]
                });
            }

            return listaContenedores;
        }
    }
}
