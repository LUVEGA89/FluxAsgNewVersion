using Reporting.Service.Core.Auditoria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Bitacora
{
    public class BitacoraManager : DataRepository
    {
        public List<BitacoraArea> GetBitacoraAreas()
        {
            List<BitacoraArea> Departamentos = new List<BitacoraArea>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBitacoraArea");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Departamentos.Add(new BitacoraArea
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Nombre = dr["Nombre"].ToString()
                });
            }
            return Departamentos;
        }

        public bool AddRubroBitacora(int Departamento, string Rubro, string Descripcion, string Ejemplo, int Orden, string RegistradoPor)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddBitacoraRequerimiento");
                this.Database.AddInParameter(cmd, "@Requerimiento", DbType.String, Rubro);
                this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
                this.Database.AddInParameter(cmd, "@Ejemplo", DbType.String, Ejemplo);
                this.Database.AddInParameter(cmd, "@Area", DbType.Int32, Departamento);
                this.Database.AddInParameter(cmd, "@Orden", DbType.Int32, Orden);
                this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

                IDataReader dr = this.Database.ExecuteReader(cmd);
                
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public bool DelRubro(int Sequence)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prDelBitacoraRequerimiento");
                this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public List<BitacoraRequerimiento> GetRubros()
        {
            List<BitacoraRequerimiento> rubros = new List<BitacoraRequerimiento>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBitacoraRequerimiento");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                rubros.Add(new BitacoraRequerimiento
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Requerimiento = dr["Requerimiento"].ToString(),
                    Descripcion = dr["Descripcion"].ToString(),
                    Ejemplo = dr["Ejemplo"].ToString(),
                    Orden = int.Parse(dr["Orden"].ToString()),
                    IdDepartamento = int.Parse(dr["IdDepartamento"].ToString()),
                    Nombre = dr["NombreDepartamento"].ToString(),
                    Icono = dr["icono"].ToString(),
                    Color = dr["color"].ToString()
                });
            }
            return rubros;
        }
        public List<BitacoraDetalle> GetDetalleBitacora(string RegistradoPor, int Sucursal)
        {
            List<BitacoraDetalle> rubros = new List<BitacoraDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleBitacora");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.Int32, Sucursal);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                rubros.Add(new BitacoraDetalle
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Nombre = dr["Requerimiento"].ToString(),
                    Solucion = dr["Solucion"].ToString(),
                    Tipo = int.Parse(dr["Tipo"].ToString()), 
                    Departamento = dr["Departamento"].ToString(),
                    Email = dr["Email"].ToString()
                });
            }
            return rubros;
        }

        public Bitacora GetCurrentBitacora(string RegistradoPor, int Sucursal)
        {
            Bitacora bitacora = new Bitacora();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLastBitacora");
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.Int32, Sucursal);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                bitacora.Sequence = int.Parse(dr["Sequence"].ToString());
                bitacora.IdSucursal = int.Parse(dr["IdSucursal"].ToString());
                bitacora.Sucursal = dr["Sucursal"].ToString();
                bitacora.Estado = int.Parse(dr["Estado"].ToString());
                bitacora.Observaciones = dr["Observaciones"].ToString();
                //bitacora.RegistradoEl = int.Parse(dr["RegistradoEl"].ToString());
                bitacora.RegistradoPor = dr["RegistradoPor"].ToString();
                
            }
            return bitacora;
        }
        public BitacoraUsuario GetCurrentSucursal(string Email)
        {
            BitacoraUsuario Usuario = new BitacoraUsuario();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSucursalUsuario");
            this.Database.AddInParameter(cmd, "@usuario", DbType.String, Email);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Usuario.Sequence = int.Parse(dr["Sequence"].ToString());
                Usuario.Nombre = dr["Nombre"].ToString();
                Usuario.Email = dr["Email"].ToString();
                Usuario.Tipo = dr["Tipo"].ToString();

            }
            return Usuario;
        }

        public bool AddDetalleBitacora(int Bitacora, int Rubro, string Solucion)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddDetalleBitacora");
                this.Database.AddInParameter(cmd, "@Bitacora", DbType.Int32, Bitacora);
                this.Database.AddInParameter(cmd, "@Rubro", DbType.Int32, Rubro);
                this.Database.AddInParameter(cmd, "@Solucion", DbType.String, Solucion);

                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public List<BitacoraDetalle> GetHistorialBitacora(string Del, string Al, string RegistradoPor, int Sucursal)
        {
            List<BitacoraDetalle> rubros = new List<BitacoraDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetHistorialDetalleBitacora");
            this.Database.AddInParameter(cmd, "@Del", DbType.String, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.String, Al);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.Int32, Sucursal);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                rubros.Add(new BitacoraDetalle
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    FolioSolucion = int.Parse(dr["FolioSolucion"].ToString()),
                    Requerimiento = int.Parse(dr["Requerimiento"].ToString()),
                    Solucion = dr["Solucion"].ToString(),
                    RegistradoEl = dr["RegistradoEl"].ToString()
                });
            }
            return rubros;
        }

        public DataTable GetDTHistorialBitacora(string Del, string Al, string RegistradoPor, int Sucursal)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetHistorialDetalleBitacora");
            this.Database.AddInParameter(cmd, "@Del", DbType.String, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.String, Al);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.Int32, Sucursal);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
        public IList<TiendaSIAT> GetTiendasSIAT(string Email)
        {
            List<TiendaSIAT> tiendas = new List<TiendaSIAT>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTiendasBitacora");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                tiendas.Add(new TiendaSIAT
                {
                    Id_tienda = (int)dr["id_tienda"],
                    Nombre = (string)dr["Tienda"],
                    Origen = (string)dr["Origen"]
                });
            }
            return tiendas;
        }
    }
}
