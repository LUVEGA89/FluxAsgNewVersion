using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.PDF
{
    public class PdfManagerD : DataRepository
    {
        public IList<Productos> FindProductos()
        {
            List<Productos> Productos = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductoSIVEPDF");
            
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Productos.Add(new Productos
                {
                    Sequence = (int)dr["Sequence"],
                    Familia = (string)dr["Familia"],
                    Categoria = (string)dr["Categoria"],
                    Tipo = (string)dr["Tipo"],
                    Clasificacion = (string)dr["Clasificacion"],
                    Stock = (int)dr["Stock"],
                    Sku = (string)dr["Sku"]

                });
            }
            return Productos;
        }
        public void AddProducto(int Producto, string RegistradoPor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddProductoPDF");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Producto);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
           
        }
        public void AddLista(int Lista, int Tipo, string RegistradoPor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddListaPDF");
            this.Database.AddInParameter(cmd, "@Lista", DbType.Int32, Lista);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }
        public void UpdatePrecio(int Producto, decimal Precio, string RegistradoPor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdatePreciosPDF");
            this.Database.AddInParameter(cmd, "@Producto", DbType.Int32, Producto);
            this.Database.AddInParameter(cmd, "@Precio", DbType.Currency, Precio);

            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }
        public void DelProductoPlantilla(int Sequence, string RegistradoPor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDelProductoPDF");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }
        public void DelPlantilla(string RegistradoPor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDelPlantillaPDF");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);

        }
        

        public IList<Productos> FindProductosPDF(string RegistradoPor)
        {
            List<Productos> Productos = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductosPDF");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Productos.Add(new Productos
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Sku = (string)dr["Sku"],
                    ListaPrecio = FindListasPreciosPDF((string)dr["Sku"], RegistradoPor)

                });
            }
            return Productos;
        }
        public IList<ListasPrecios> FindListasPDF(string RegistradoPor)
        {
            List<ListasPrecios> Productos = new List<ListasPrecios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetListasPDF");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Productos.Add(new ListasPrecios
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = dr["Lista"].ToString(),
                    Tipo = dr["Tipo"].ToString()

                });
            }
            return Productos;
        }
        public List<ListasPreciosTiendas> FindPriceList(string Usuario)
        {
            List<ListasPreciosTiendas> listas = new List<ListasPreciosTiendas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPricesListPDFTiendas");
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                listas.Add(new ListasPreciosTiendas
                {
                    Sequence = (int)dr["Sequence"],
                    NombreTienda = (string)dr["NombreTienda"],
                    Usuario = (string)dr["Usuario"],
                    IdListaPreciosSAP = (int)dr["IdListaPreciosSAP"],
                    NombreListaPreciosSAP = (string)dr["NombreListaPreciosSAP"]
                });
            }
            return listas;
        }
        public IList<ListasPrecios> FindListasPreciosPDF(string Sku, string RegistradoPor)
        {
            List<ListasPrecios> Productos = new List<ListasPrecios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPricesListPDF");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Productos.Add(new ListasPrecios
                {
                    Nombre = dr["Lista"].ToString(),
                    Precio = (decimal)dr["Precio"],
                    PrecioActualizado = (decimal)dr["PrecioActualizado"],
                    PrecioSinIVA = (decimal)dr["PrecioSinIVA"]
                });
            }
            return Productos;
        }
        public DataTable FillPlantillaPDF(string RegistradoPor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductPlantillaPDF");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
    }
}
