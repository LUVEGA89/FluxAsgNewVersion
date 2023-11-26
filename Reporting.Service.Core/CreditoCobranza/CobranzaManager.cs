using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CreditoCobranza
{
    public class CobranzaManager : DataRepository
    {
        public void AddConsulta()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddConsultaCobranza");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            
        }
        public void EliminarRegitrosCobranza(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteRegistroCobranza");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
        }
        public List<Consultas> GetUltimasConsultas()
        {
            List<Consultas> resultado = new List<Consultas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUltimasConsultas");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new Consultas {
                    Sequence = (int)dr["Sequence"] ,
                    Comentario = DBNull.Value.Equals(dr["Comentarios"]) ? string.Empty : (string)dr["Comentarios"],
                    Fecha = (DateTime)dr["RegistradoEl"],
                });

            }
            return resultado;
        }

        public List<DetalleCobranza> GetDetalleCobranza()
        {
            List<DetalleCobranza> resultado = new List<DetalleCobranza>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleCobranza");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new DetalleCobranza
                {
                    folio = (int)dr["Sequence"],
                    Identificador = (string)dr["Identificador"],
                    Saldo = (decimal)dr["Saldo"],
                    Fecha = (DateTime)dr["RegistradoEl"],

                });

            }
            return resultado;
        }

        public List<CondicionesPago> GetCobranzaCondicionesPago()
        {
            List<CondicionesPago> resultado = new List<CondicionesPago>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCobranzaCondicionesPago");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new CondicionesPago
                {
                    Identificador = (string)dr["Formas"],
                    Documentos = (int)dr["Total Doc"],
                    SaldoTotal = (decimal)dr["Total"],
                    Participacion = (decimal)dr["%"],

                });

            }
            return resultado;
        }
        public List<DetallePorCanal> GetCobranzaCanalesContado()
        {
            List<DetallePorCanal> resultado = new List<DetallePorCanal>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCobranzaCanalesContado");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new DetallePorCanal
                {
                    Identificador = (string)dr["CANAL"],
                    Total = (decimal)dr["TOTAL"],
                    Financiamiento = (decimal)dr["FINANCIAMIENTO DIARIO"],
                    Participacion = (decimal)dr["%"],

                });

            }
            return resultado;
        }
        public List<DetallePorCanal> GetCobranzaCanalesCredito()
        {
            List<DetallePorCanal> resultado = new List<DetallePorCanal>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCobranzaCanalesCredito");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new DetallePorCanal
                {
                    Identificador = (string)dr["CANAL"],
                    Total = (decimal)dr["TOTAL"],
                    Financiamiento = (decimal)dr["FINANCIAMIENTO DIARIO"],
                    Participacion = (decimal)dr["%"],

                });

            }
            return resultado;
        }
        public List<CuentasPorCobrar> GetCobranzaCanalesCuentasPorCobrar()
        {
            List<CuentasPorCobrar> resultado = new List<CuentasPorCobrar>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCobranzaCanalesCuentasPorCobrar");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new CuentasPorCobrar
                {
                    Canal = (string)dr["CANAL"],
                    Monto = (decimal)dr["CTAS. POR COBRAR"],
                    DiasCartera = (decimal)dr["DIAS DE CARTERA"]

                });

            }
            return resultado;
        }
        public List<DetallePorCanal> GetHistorialCartera()
        {
            List<DetallePorCanal> resultado = new List<DetallePorCanal>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetHistorialCartera");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new DetallePorCanal
                {
                    Identificador = (string)dr["Cartera"],
                    Documentos = (int)dr["NoDoc"],
                    Total = (decimal)dr["Importe"],
                    Participacion = (decimal)dr["Porcentaje"],

                });

            }
            return resultado;
        }

        public List<DetallePorCanal> GetTopClientes()
        {
            List<DetallePorCanal> resultado = new List<DetallePorCanal>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTopClientesFinanzas");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                resultado.Add(new DetallePorCanal
                {
                    Identificador = (string)dr["Identificador"],
                    Total = (decimal)dr["Saldo"],
                    Participacion = (decimal)dr["Porcentaje"],
                    Tipo = (int)dr["Tipo"],
                });

            }
            return resultado;
        }
    }
}
