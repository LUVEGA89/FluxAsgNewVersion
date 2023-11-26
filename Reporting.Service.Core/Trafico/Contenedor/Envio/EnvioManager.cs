using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.Envio
{
    public class EnvioManager : Catalog<Envio, int, EnvioCriteria>
    {
        public EnvioManager()
            :base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindEnvios";

        protected override Envio LoadItem(IDataReader dr)
        {
            Envio nuevo = new Envio();

            nuevo.Identifier = int.Parse(dr["DocNum"].ToString());
            nuevo.Proveedor = (string)dr["CardName"];
            nuevo.Importe = (decimal)dr["DocTotal"];
            return nuevo;
        }

        protected override DbCommand PrepareAddStatement(Envio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEnvio");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Envio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(EnvioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@FecIni", DbType.Date, criteria.Inicio);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.Date, criteria.Fin);
            
            return cmd;
        }

        //Función para obtener el listado de pedidos del envio
        public List<Pedido> GetArtículosVentas(int DocNum)
        {
            List<Pedido> pedidos = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEnvioPedido");
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int16, DocNum);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Pedido nuevo = new Pedido();
                nuevo.Identifier = (string)dr["ItemCode"];
                nuevo.nombre = (string)dr["ItemName"];
                var valor = dr["Quantity"].ToString().Split('.');
                nuevo.cantidad = int.Parse(valor[0]);
                nuevo.precio = (decimal)dr["Price"];
                nuevo.TotalImporte = (decimal)dr["Importe"];
                nuevo.tipoCambio = (string)dr["DocCur"];
                nuevo.nom = (string)dr["NOM"];
                nuevo.fechaVencimiento = (DateTime)dr["U_FechaVencimiento"];
                nuevo.arancel = int.Parse(dr["U_Fraccionarancelari"].ToString());
                pedidos.Add(nuevo);
            }
            return pedidos;
        }

        //Función para obtener el listado de anexos del envio
        public List<ContenedorAnexo.ContenedorAnexo> getAnexos(int DocNum, string SKU)
        {
            List<ContenedorAnexo.ContenedorAnexo> anexos = new List<ContenedorAnexo.ContenedorAnexo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnexos");
            if(DocNum!=0)
                this.Database.AddInParameter(cmd, "@DocNum", DbType.Int16, DocNum);
            else
                this.Database.AddInParameter(cmd, "@SKU", DbType.String, SKU);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                ContenedorAnexo.ContenedorAnexo nuevo = new ContenedorAnexo.ContenedorAnexo();
                nuevo.path = (string)dr["path"];
                nuevo.archivo = (string)dr["archivo"];
                nuevo.ext = (string)dr["ext"];
                nuevo.subPath = (string)dr["subPath"];

                anexos.Add(nuevo);
            }
            return anexos;
        }

        //Función el detalle del artículo
        public DetallePedido GetDetalleArticulo(string SKU)
        {
            DetallePedido articulo = new DetallePedido();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleProductoChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, SKU);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                articulo.Identifier = (string)dr["Articulo"];
                articulo.familia = (string)dr["Familia"];
                articulo.subfamilias = (string)dr["SubFamilias"];
                articulo.empaque = (string)dr["Empaque"];
                articulo.inner = (int)dr["Inner"];
                articulo.master = (int)dr["Master"];
                articulo.accesorios = (string)dr["Accesorios"];
            }
            return articulo;
        }

        //Función que obtiene los SKUS para el reporte
        public List<Pedido> ReporteSKUS(int Tipo)
        {
            List<Pedido> pedidos = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetReporteSKUS");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int16, Tipo);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Pedido nuevo = new Pedido();
                nuevo.Identifier = (string)dr["ItemCode"];
                if(Tipo == 1)
                {
                    nuevo.contenedor = (string)dr["nomContenedor"];
                    nuevo.status = (string)dr["nomEstado"];
                }
                nuevo.nombre = (string)dr["ItemName"];
                nuevo.fechaVencimiento = (DateTime)dr["U_FechaVencimiento"];
                nuevo.nom = (string)dr["U_NOM"];
                nuevo.fechaCertificado = (DateTime)dr["U_Fechacertificado"];
                nuevo.certificado = (string)dr["U_Certificado"];
                nuevo.envios = (string)dr["Envios"];
                var valor = dr["Can Envio"].ToString().Split('.');
                nuevo.cantEnvio = int.Parse(valor[0]);
                nuevo.ordCompra = (string)dr["Ord. Compra"];
                valor = dr["Can Orden Compra"].ToString().Split('.');
                nuevo.cantidad = int.Parse(valor[0]);
                valor = dr["Stock Massriv"].ToString().Split('.');
                nuevo.stock = int.Parse(valor[0]);
                valor = dr["U_maximo"].ToString().Split('.');
                nuevo.U_Maximo = int.Parse(valor[0]);
                valor = dr["U_minimo"].ToString().Split('.');
                nuevo.U_Minimo = int.Parse(valor[0]);
                pedidos.Add(nuevo);
            }
            return pedidos;
        }
    }
}
