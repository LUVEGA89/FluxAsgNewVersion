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
    public class SKUComparacionCatalog : Catalog<ActividadProductosComparacion, int, SKUComparacionCriteria>
    {
        public SKUComparacionCatalog()
        {
        }
        public SKUComparacionCatalog(string database)
            : base(database)
        {
        }

        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadProductosComparacion"; }
        } 
        
        protected override ActividadProductosComparacion LoadItem(IDataReader dr)
        {
            ActividadProductosComparacion productosComparacion = new ActividadProductosComparacion();
            productosComparacion.Identifier = (int)dr["Identificador"];
            productosComparacion.Actividad = (int)dr["Actividad"];
            productosComparacion.PrecioLocal = (decimal)dr["PrecioLocal"];
            productosComparacion.TipoPrecio = (string)dr["TipoPrecio"];
            productosComparacion.MinPiezas = (int)dr["MinPiezas"];
            productosComparacion.PrecioCompetencia = (decimal)dr["PrecioCompetencia"];
            productosComparacion.Modelo = (string)dr["Modelo"];
            productosComparacion.Marca = (string)dr["Marca"];
            productosComparacion.Estatus = (bool)dr["Estatus"];
            return productosComparacion;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(ActividadProductosComparacion item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetActividadProductosComparacion");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            return cmd;
            //throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindPagedItemsStatement(SKUComparacionCriteria Criteria)
        {
            DbCommand cmd=base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, Criteria.IdActividad);
            return cmd;
        }
        protected override DbCommand PrepareUpdateStatement(ActividadProductosComparacion item)
        {
            throw new NotImplementedException();
        }
    }
}
