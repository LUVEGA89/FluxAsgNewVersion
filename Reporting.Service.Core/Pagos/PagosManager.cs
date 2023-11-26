using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Pagos
{
    public class PagosManager : DataRepository
    {
        public List<Documentos> GetDocumentosAPago(string Codigo, int Folio, int Factura, string Usuario)
        {
            List<Documentos> rubros = new List<Documentos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDocumentosPorPagar");
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            if (Folio > 0)
                this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
            if (Codigo != "")
                this.Database.AddInParameter(cmd, "@Codigo", DbType.String, Codigo);
            if (Factura > 0)
                this.Database.AddInParameter(cmd, "@Factura", DbType.Int32, Factura);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                rubros.Add(new Documentos
                {
                    Folio = (int)dr["Folio"],
                    Codigo = (string)dr["Codigo"],
                    Nombre = (string)dr["Nombre"],
                    Fecha = (string)dr["Fecha"],
                    Total = (decimal)dr["Total"],
                    Pagado = (decimal)dr["Pagado"],
                    Saldo = (decimal)dr["Saldo"],
                    Referencia = int.Parse(dr["Referencia"].ToString()),
                    Documento = (string)dr["Documento"]

                });
            }
            return rubros;
        }
        public List<Parciales> GetPagosParciales(int Documentos)
        {
            List<Parciales> rubros = new List<Parciales>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPagosParciales");
            this.Database.AddInParameter(cmd, "@Documento", DbType.Int32, Documentos);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                rubros.Add(new Parciales
                {
                    Sequence = (int)dr["Sequence"],
                    Documento = (int)dr["Documento"],
                    TipoPago = (string)dr["TipoPago"],
                    Banco = (string)dr["Banco"],
                    FechaDeposito = (string)dr["FechaDeposito"],
                    Monto = (decimal)dr["Monto"],
                    Referencia = (string)dr["Referencia"],
                    NoCuenta = (string)dr["NoCuenta"],
                    Beneficiario = (string)dr["Beneficiario"],
                    Imagen = (string)dr["Imagen"],

                    Origen = (string)dr["Origen"],
                    Aplicado = (int)dr["Aplicado"],
                    Eliminado = (int)dr["Eliminado"]
                });
                
            }
            return rubros;
        }
        public List<Documentos> GetDocumentosPagados()
        {
            List<Documentos> rubros = new List<Documentos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDocumentosPagados");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                rubros.Add(new Documentos
                {
                    Folio = (int)dr["Folio"],
                    Codigo = (string)dr["Codigo"],
                    Nombre = (string)dr["Nombre"],
                    Fecha = (string)dr["Fecha"],
                    Total = (decimal)dr["Total"],
                    Pagado = (decimal)dr["Pagado"],
                    Saldo = (decimal)dr["Pendiente"],
                    Documento = (string)dr["Documento"],
                    Referencia = int.Parse(dr["Referencia"].ToString())
                });
            }
            return rubros;
        }

        public bool AddPagoParcial(string Origen, int Documento, decimal Monto, string TipoPago, string Banco, string FechaDeposito, string Referencia, string NoCuenta, string Beneficiario, string RegistradoPor, int SequenceImage, int SyncSAP)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddPagosParciales");
                this.Database.AddInParameter(cmd, "@Origen", DbType.String, Origen);
                this.Database.AddInParameter(cmd, "@Documento", DbType.Int32, Documento);
                this.Database.AddInParameter(cmd, "@Monto", DbType.Decimal, Monto);
                this.Database.AddInParameter(cmd, "@TipoPago", DbType.String, TipoPago);
                this.Database.AddInParameter(cmd, "@Banco", DbType.String, Banco);
                this.Database.AddInParameter(cmd, "@FechaDeposito", DbType.Date, FechaDeposito);
                this.Database.AddInParameter(cmd, "@Referencia", DbType.String, Referencia);
                this.Database.AddInParameter(cmd, "@NoCuenta", DbType.String, NoCuenta);
                this.Database.AddInParameter(cmd, "@Beneficiario", DbType.String, Beneficiario);
                this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
                this.Database.AddInParameter(cmd, "@SequenceImage", DbType.Int32, SequenceImage);
                this.Database.AddInParameter(cmd, "@SyncSAP", DbType.Int32, SyncSAP);

                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool prUpdatePedidoSIVE(int Pedido)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdatePedidoSIVE");
                this.Database.AddInParameter(cmd, "@Pedido", DbType.String, Pedido);
                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool prUpdatePedidoSAP(int Folio)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdatePedidoSAP");
                this.Database.AddInParameter(cmd, "@Folio", DbType.String, Folio);
                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool prUpdatePedidoSAPPArcial(int Sequence)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdatePedidoSAPParcial");
                this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool prUpdatePedidoSIVEParcial(int Sequence, int Pedido)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdatePedidoSIVEParcial");
                this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
                this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, Pedido);
                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool EliminarPagoParcial(int Sequence)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prEliminarPagoPArcial");
                this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
                IDataReader dr = this.Database.ExecuteReader(cmd);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public int AddImage(string Nombre, string Image64)
        {
            try
            {
                int Sequence = 0;
                DbCommand command = this.Database.GetStoredProcCommand("prAddImageFile");
                this.Database.AddInParameter(command, "@Nombre", DbType.String, Nombre);
                this.Database.AddInParameter(command, "@Image64", DbType.String, Image64);
                this.Database.AddOutParameter(command, "@Sequence", DbType.Int32, 0);
                this.Database.ExecuteNonQuery(command);

                Sequence = Convert.ToInt32(this.Database.GetParameterValue(command, "@Sequence"));
                command.Dispose();

                return Sequence;
            }
            catch (Exception ex)
            {
                var exec = ex;
                return 0;
            }
        }

        public string GetEmailPagoRegistro(int Dccumento)
        {
            try
            {
                string Email = "";
                DbCommand command = this.Database.GetStoredProcCommand("prGetEmailPagoRegistrado");
                this.Database.AddInParameter(command, "@Documento", DbType.Int32, Dccumento);
                this.Database.AddOutParameter(command, "@Email", DbType.String, 500);
                this.Database.ExecuteNonQuery(command);

                Email = this.Database.GetParameterValue(command, "@Email").ToString();
                command.Dispose();

                return Email;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
