using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.MetasCobranza
{
    public class MetasCobranzaManager : DataRepository
    {
        public List<Meta> GetMetas(int IdCanal)
        {
            List<Meta> Info = new List<Meta>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetMetasCobranza");
            this.Database.AddInParameter(cmd, "@IdCanal", DbType.Int32, IdCanal);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Meta x = new Meta();
                x.Canal = new Canal();
                x.Identifier = (int)dr["Sequence"];
                x.FechaMeta = (DateTime)dr["FechaMeta"];
                x.Vencido = (decimal)dr["MontoVencido"];
                x.Cobrado = (decimal)dr["MontoCobrado"];
                x.MetaMonto = (decimal)dr["Meta"];

                //x.VencidoAnioPasado = (decimal)dr["VencidoAnioPasado"];
                //x.VencidoAnioActual = (decimal)dr["VencidoAnioActual"];
                //x.MetaAnioPasado = (decimal)dr["MetaAnioPasado"];
                //x.MetaAnioActual = (decimal)dr["MetaAnioActual"];
                //x.CobradoAnioPasado = dr["CobradoAnioPasado"] == DBNull.Value ? 0.00m : (decimal)dr["CobradoAnioPasado"];
                //x.CobradoAnioActual = dr["CobradoAnioActual"] == DBNull.Value ? 0.00m : (decimal)dr["CobradoAnioActual"];

                x.RegistradoEl = (DateTime)dr["RegistradoEl"];
                x.RegistradoPor = (string)dr["RegistradoPor"];
                x.ActualizadoEl = dr["ActualizadoEl"] == DBNull.Value ? null : (DateTime?)dr["ActualizadoEl"];
                x.ActualizadoPor = dr["ActualizadoPor"] == DBNull.Value ? "" : (string)dr["ActualizadoPor"];
                x.Canal.Sequence = dr["SeqCanal"] != DBNull.Value ? (int)dr["SeqCanal"] : 0;
                x.Canal.Nombre = dr["Canal"] != DBNull.Value ? (string)dr["Canal"] : "No especificado";
                x.Canal.Descripcion = dr["CanalDescripcion"] != DBNull.Value ? (string)dr["CanalDescripcion"] : "No especificado";                

                Info.Add(x);
            }
            return Info;
        }


        public string AddMeta(Meta meta)
        {
            string resultado = string.Empty;
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddMetaCobranza");
            this.Database.AddInParameter(cmd, "@FechaMeta", DbType.Date, meta.FechaMeta);            
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, meta.RegistradoPor);
            this.Database.AddOutParameter(cmd, "@SeqMetaCobranza", DbType.Int32, 32);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
            {
                var SeqMetaCobranza = (int)cmd.Parameters["@SeqMetaCobranza"].Value;
                foreach (var item in meta.Detalles)
                {                    
                    if (AddMetaDetalle(item, SeqMetaCobranza))
                    {
                        resultado += "" + item.Nombre + ": Correcto, ";                        
                    }
                    else
                    {
                        resultado += "" + item.Nombre + ": Error, ";                        
                    }
                }
                return resultado;
            }
            else
            {
                return "Erro API: AddMetodoCobranza";
            }

        }
        public bool AddMetaDetalle(Canal Canal,int SeqMtaCbrza)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddMetaCobranzaDetalle");            
            this.Database.AddInParameter(cmd, "@MontoVencido", DbType.Decimal, Canal.MontoVencido);
            this.Database.AddInParameter(cmd, "@MetaMonto", DbType.Decimal, Canal.MontoMeta);
            this.Database.AddInParameter(cmd, "@SeqCanal", DbType.Int32, Canal.Sequence);
            this.Database.AddInParameter(cmd, "@SeqMetaCobranza", DbType.Int32, SeqMtaCbrza);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public string UpdateMeta(Meta meta)
        {
            string resultado = string.Empty;
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateMetaCobranza");            
            this.Database.AddInParameter(cmd, "@FechaMeta", DbType.Date, meta.FechaMeta);            
            this.Database.AddInParameter(cmd, "@ActualizadoPor", DbType.String, meta.ActualizadoPor);
            this.Database.AddOutParameter(cmd, "@SeqMetaCobranza", DbType.Int32, 32);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
            {
                var SeqMetaCobranza = (int)cmd.Parameters["@SeqMetaCobranza"].Value;
                foreach (var item in meta.Detalles)
                {
                    if (UpdateMetaDetalle(item, SeqMetaCobranza))
                    {
                        resultado += "" + item.Nombre + ": Correcto, ";
                    }
                    else
                    {
                        resultado += "" + item.Nombre + ": Error, ";
                    }
                }
                return resultado;
            }
            else
            {
                return "Erro API: UpdateMetodoCobranza";
            }

        }
        public bool UpdateMetaDetalle(Canal Canal, int SeqMtdCbrza)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateMetaCobranzaDetalle");            
            this.Database.AddInParameter(cmd, "@MontoVencido", DbType.Decimal, Canal.MontoVencido);
            this.Database.AddInParameter(cmd, "@MetaMonto", DbType.Decimal, Canal.MontoMeta);
            this.Database.AddInParameter(cmd, "@SeqCanal", DbType.Int32, Canal.Sequence);
            this.Database.AddInParameter(cmd, "@SeqMetaCobranza", DbType.Int32, SeqMtdCbrza);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public List<FacturaDetalle> GetFacturaDetalles(DateTime Del, DateTime Al, int IdCanal)        
        {
            List<FacturaDetalle> lista = new List<FacturaDetalle>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetFacturasVencidasPorCanal");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Al);
            this.Database.AddInParameter(command, "@IdCanal", DbType.Int32, IdCanal);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                FacturaDetalle fa = new FacturaDetalle();
                fa.IdFactura = (int)dr["Factura"];
                fa.FechaFactura = (DateTime)dr["FecFactura"];
                fa.FechaVencimiento = (DateTime)dr["FecVencimiento"];
                fa.CarCode = (string)dr["CardCode"];
                fa.CardName = (string)dr["CardName"];
                fa.Canal = (string)dr["Canal"];
                fa.Saldo = (decimal)dr["Saldo"];
                lista.Add(fa);
            };
            command.Dispose();
            dr.Close();
            dr.Dispose();
            return lista;
        }

        public AcumuladosyMetas GetAcumuladoVencidas(DateTime Del, DateTime Al, int Tipo)
        {
            AcumuladosyMetas Info = new AcumuladosyMetas();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAcumuladoFacturasVencidas");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Info.Acumulado = (decimal)dr["Total"];
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Canal canal = new Canal();                    
                    canal.Sequence = (int)dr["SeqCanal"];
                    canal.Nombre = (string)dr["Canal"];
                    canal.MontoMeta = (decimal)dr["MontoMeta"];
                    Info.AddMetaExistente(canal);
                }
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Canal canal = new Canal();

                    canal.Sequence = (int)dr["Sequence"];
                    canal.Nombre = (string)dr["Nombre"];
                    canal.Total = (decimal)dr["Total"];
                    Info.AddCanal(canal);
                }
            }

            return Info;
        }

        public bool SincronizarPagos()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prSynPagosMetasCobranza");

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

    }
}
