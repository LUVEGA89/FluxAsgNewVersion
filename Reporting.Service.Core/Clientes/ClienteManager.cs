using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Clientes
{
    public class ClienteManager : DataRepository
    {
        public List<Cliente> GetClientesAAA(DateTime Del, DateTime Al)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCustomerSAPByType");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Codigo = (string)dr["Codigo"],
                    Nombre = (string)dr["CardName"],
                    Agente = (string)dr["Agente"],
                    VentaPeriodoActual = (decimal)dr["VentaPeriodo"],
                    VentaPeriodoAnterior = (decimal)dr["VentaPeriodoAnnoAnt"],
                    Crecimiento = (decimal)dr["% Crecimiento"],
                });
            }
            return Detalle;
        }
        public List<Cliente> GetClientesAA(DateTime Del, DateTime Al)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCustomerSAPByTypeAA");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Codigo = (string)dr["Codigo"],
                    Nombre = (string)dr["CardName"],
                    Agente = (string)dr["Agente"],
                    VentaPeriodoActual = (decimal)dr["VentaPeriodo"],
                    VentaPeriodoAnterior = (decimal)dr["VentaPeriodoAnnoAnt"],
                    Crecimiento = (decimal)dr["% Crecimiento"],
                });
            }
            return Detalle;
        }


        public DataTable GetClientesAAAExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCustomerSAPByType");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }


        public DataTable GetClientesAAExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCustomerSAPByTypeAA");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }


        public List<Comentario> GetComentarios(string Codigo)
        {
            List<Comentario> Detalle = new List<Comentario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetComentariosSeguimiento");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Codigo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Comentario
                {
                    Sequence = (int)dr["Sequence"],
                    SequencePadre = DBNull.Value.Equals(dr["SequencePadre"]) ? 0 : (int)dr["SequencePadre"],
                    Cliente = (string)dr["Cliente"],
                    Detalle = (string)dr["Comentario"],
                    RegistradoPor = (string)dr["RegistradoPor"],
                    RegistradoEl = (DateTime)dr["RegistradoEl"],
                    Codigo = Runtime.GetUrlImagenUsuario((int)dr["Codigo"])
                });
            }
            return Detalle;
        }

































        public bool AddComentarios(string Cliente, string Comentario, string RegistradoPor)
        {
            List<Comentario> Detalle = new List<Comentario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddComentarioSeguimiento");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);


            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }



        public bool AddComentariosSecuence(string SequencePadre, string Cliente, string Comentario, string RegistradoPor)
        {
            List<Comentario> Detalle = new List<Comentario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddComentarioSeguimientoSequence");
            this.Database.AddInParameter(cmd, "@SequencePadre", DbType.String, SequencePadre);
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }


























        public DataTable GetSeguimientoClientes(DateTime Del, DateTime Al, string Vendedor = null)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetReporteComentarios");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Vendedor);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
    }
}
