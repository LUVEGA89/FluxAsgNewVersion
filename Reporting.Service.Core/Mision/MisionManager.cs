using Reporting.Service.Core.Cliente;
using Reporting.Service.Core.Empresa;
using Reporting.Service.Core.Quotation;
using Reporting.Service.Core.Servicio;
using Reporting.Service.Core.Usuarios;
using Resporting.Service.Core.Proveedor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mision
{
    public class MisionManager : Catalog<Mision, int, MisionCriteria>
    {
        private readonly ProveedorManager proveedorManager = new ProveedorManager();
        readonly EmpresaManager empresaManager = new EmpresaManager();
        readonly ServicioManager servicioManager = new ServicioManager();
        protected override string FindPagedItemsProcedure => "prFindMisions";

        protected override Mision LoadItem(IDataReader dr)
        {
            Mision quotation = new Mision
            {
                Identifier = (int)dr["Sequence"],
                Referencia = (string)dr["Referencia"],
                Empresa = empresaManager.Find((int)dr["EmpresaId"]),
                Client = dr["NombreCliente"] == DBNull.Value ? "" : (string)dr["NombreCliente"],
                ClienteId = dr["ClienteId"] == DBNull.Value ? 0 : (int)dr["ClienteId"],
                ClientRef = dr["ClientRef"] == DBNull.Value ? "" : (string)dr["ClientRef"],
                Origin = (string)dr["SiglasOrigen"],
                Destination = (string)dr["SiglasDestino"],
                Service = (int)dr["TipoServicioId"],
                ServiceObject = servicioManager.Find((int)dr["TipoServicioId"]),
                RegistradaEl = (DateTime)dr["CreadaEl"],
                Status = (QuotationStatus)(int)dr["Status"],
                NumberOfPackages = dr["NumeroBultos"] == DBNull.Value ? 0 : (decimal)dr["NumeroBultos"],
                Weight = dr["Peso"] == DBNull.Value ? 0 : (decimal)dr["Peso"],
                Description = dr["Descripcion"] == DBNull.Value ? "" : (string)dr["Descripcion"],
                GrupoWhatsapp = dr["GrupoWhatsapp"] == DBNull.Value ? "" : (string)dr["GrupoWhatsapp"]
            };

            switch (quotation.Service)
            {
                case 1://Obc
                    quotation.ObcServiceType = (int)dr["TipoServicioId"];
                    quotation.ObcPickup = dr["Pickup"] == DBNull.Value ? 0.0m : (decimal)dr["Pickup"];
                    quotation.ObcHotel = (string)dr["Hotel"];
                    quotation.ObcDelibery = (string)dr["Delibery"];
                    quotation.ObcCustoms = (string)dr["Customs"];
                    quotation.ObcOther = (string)dr["Other"];
                    quotation.ObcProfit = (string)dr["Profit"];
                    quotation.ObcEB = dr["EB"] == DBNull.Value ? 0.0m : (decimal)dr["EB"];
                    quotation.ObcSellPrice = dr["SellPrice"] == DBNull.Value ? 0.0m : (decimal)dr["SellPrice"];
                    quotation.ObcPickupDate = ((DateTime)dr["PickupDate"]);
                    //quotation.ObcCosts = ((decimal)dr["Costs"]).ToString();
                    quotation.ObcSchedule = (string)dr["Schedule"];
                    quotation.ObcDisclaimerEta = (DateTime)dr["Eta"];

                    quotation.CommentsTable = dr["ComentariosTabla"] == DBNull.Value ? "" : (string)dr["ComentariosTabla"];
                    quotation.CommentsCustom = dr["ComentariosCustom"] == DBNull.Value ? "" : (string)dr["ComentariosCustom"];
                    quotation.CommentsDelibery = dr["ComentariosDelibery"] == DBNull.Value ? "" : (string)dr["ComentariosDelibery"];
                    quotation.CommentsHotel = dr["ComentariosHotel"] == DBNull.Value ? "" : (string)dr["ComentariosHotel"];
                    quotation.CommentsOther = dr["ComentariosOther"] == DBNull.Value ? "" : (string)dr["ComentariosOther"];
                    quotation.CommentsPickup = dr["ComentariosPickup"] == DBNull.Value ? "" : (string)dr["ComentariosPickup"];

                    //traemos el detalle
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            QuotationDetails quotationDetails = new QuotationDetails
                            {
                                Obc = (int)dr["NumeroObc"],
                                Day = (int)dr["Day"],
                                ObcFee = (decimal)dr["ObcFee"],
                                Flights = (decimal)dr["Flights"]
                            };
                            quotation.QuotationDetails.Add(quotationDetails);
                        }
                    }
                    break;
                case 2://Charter
                    quotation.CharterServiceType = (int)dr["TipoServicioId"];
                    quotation.CharterBuy = (string)dr["Buy"];
                    quotation.CharterRute = (string)dr["route"];
                    quotation.CharterAircraft = (string)dr["Aircraft"];
                    quotation.CharterPositioning = (string)dr["Positioning"];
                    quotation.CharterLiveLeg = (string)dr["Liveleg"];
                    quotation.CharterCosts = ((decimal)dr["Costs"]).ToString();
                    break;
                case 3://Nfo
                    quotation.NfoServiceType = (int)dr["TipoServicioId"];
                    quotation.NfoBuy = (string)dr["Buy"];
                    quotation.NfoRute = (string)dr["route"];
                    quotation.NfoAirline = (string)dr["Airline"];
                    quotation.NfoCosts = ((decimal)dr["Costs"]).ToString();
                    break;
                case 4://HotShot
                    quotation.HostshotBuy = (string)dr["Buy"];
                    quotation.HostshotTransitTime = (string)dr["TransitTime"];
                    quotation.HostshotCosts = ((decimal)dr["Costs"]).ToString();
                    break;
            }

            return quotation;
        }

        protected override DbCommand PrepareAddStatement(Mision item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetMision");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }
        protected override void BeforUpdateExcecuted(DataContext<Mision, int, MisionCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
            base.BeforUpdateExcecuted(context);
        }
        protected override DbCommand PrepareUpdateStatement(Mision item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateMision");

            this.Database.AddInParameter(command, "@IdMision", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@Empresa", DbType.Int32, item.Empresa.Identifier);
            this.Database.AddInParameter(command, "@Client", DbType.String, item.Client);
            this.Database.AddInParameter(command, "@ClientRef", DbType.String, item.ClientRef);
            this.Database.AddInParameter(command, "@Origin", DbType.String, item.Origin);
            this.Database.AddInParameter(command, "@Destination", DbType.String, item.Destination);
            this.Database.AddInParameter(command, "@Service", DbType.Int32, item.Service);
            this.Database.AddInParameter(command, "@Usuario", DbType.String, item.CreadaPor);

            this.Database.AddInParameter(command, "@NumberOfPackages", DbType.Decimal, item.NumberOfPackages);
            this.Database.AddInParameter(command, "@Weight", DbType.Decimal, item.Weight);
            this.Database.AddInParameter(command, "@Description", DbType.String, item.Description);
            this.Database.AddInParameter(command, "@GrupoWhatsapp", DbType.String, item.GrupoWhatsapp);

            switch (item.Service)
            {
                case 1://Obc
                    this.Database.AddInParameter(command, "@ServiceType", DbType.Int32, item.ObcServiceType);//door-door
                    this.Database.AddInParameter(command, "@Pickup", DbType.Decimal, item.ObcPickup);
                    this.Database.AddInParameter(command, "@Hotel", DbType.Decimal, item.ObcHotel);
                    this.Database.AddInParameter(command, "@Delibery", DbType.String, item.ObcDelibery);
                    this.Database.AddInParameter(command, "@Customs", DbType.String, item.ObcCustoms);
                    this.Database.AddInParameter(command, "@Other", DbType.String, item.ObcOther);
                    this.Database.AddInParameter(command, "@Profit", DbType.String, item.ObcProfit);
                    this.Database.AddInParameter(command, "@EB", DbType.Decimal, item.ObcEB);
                    this.Database.AddInParameter(command, "@SellPrice", DbType.String, item.ObcSellPrice);
                    //this.Database.AddInParameter(command, "@Option", DbType.String, item.ObcO); esto del option es para poner el padre a una cotizacion y poder clonarla
                    this.Database.AddInParameter(command, "@PickUpDate", DbType.DateTime, item.ObcPickupDate);
                    this.Database.AddInParameter(command, "@Costs", DbType.String, item.ObcCosts);
                    this.Database.AddInParameter(command, "@Schedule", DbType.String, item.ObcSchedule);
                    this.Database.AddInParameter(command, "@Eta", DbType.DateTime, item.ObcDisclaimerEta);

                    this.Database.AddInParameter(command, "@CommentsTable", DbType.String, item.CommentsTable);
                    this.Database.AddInParameter(command, "@CommentsCustom", DbType.String, item.CommentsCustom);
                    this.Database.AddInParameter(command, "@CommentsDelibery", DbType.String, item.CommentsDelibery);
                    this.Database.AddInParameter(command, "@CommentsHotel", DbType.String, item.CommentsHotel);
                    this.Database.AddInParameter(command, "@CommentsOther", DbType.String, item.CommentsOther);
                    this.Database.AddInParameter(command, "@CommentsPickup", DbType.String, item.CommentsPickup);
                    break;
                case 2://Charter
                    this.Database.AddInParameter(command, "@ServiceType", DbType.Int32, item.CharterServiceType);//door-door
                    this.Database.AddInParameter(command, "@Buy", DbType.String, item.CharterBuy);
                    this.Database.AddInParameter(command, "@Aircraft", DbType.String, item.CharterAircraft);
                    this.Database.AddInParameter(command, "@Positioning", DbType.String, item.CharterPositioning);
                    this.Database.AddInParameter(command, "@Rute", DbType.String, item.CharterRute);
                    this.Database.AddInParameter(command, "@Liveleg", DbType.String, item.CharterLiveLeg);
                    this.Database.AddInParameter(command, "@Costs", DbType.Decimal, item.CharterCosts);
                    break;
                case 3://Nfo
                    this.Database.AddInParameter(command, "@ServiceType", DbType.Int32, item.NfoServiceType);//door-door
                    this.Database.AddInParameter(command, "@Buy", DbType.String, item.NfoBuy);
                    this.Database.AddInParameter(command, "@Airline", DbType.String, item.NfoAirline);
                    this.Database.AddInParameter(command, "@Rute", DbType.String, item.NfoRute);
                    this.Database.AddInParameter(command, "@Costs", DbType.Decimal, item.NfoCosts);
                    break;
                case 4://HotShot
                    this.Database.AddInParameter(command, "@Buy", DbType.String, item.HostshotBuy);
                    this.Database.AddInParameter(command, "@Transittime", DbType.String, item.HostshotTransitTime);
                    this.Database.AddInParameter(command, "@Costs", DbType.Decimal, item.HostshotCosts);
                    break;
            }

            return command;
        }

        protected override void CommandUpdateComplete(DataContext<Mision, int, MisionCriteria> context)
        {
            if (context.Item.Service == 1)
            {
                context.Command.Parameters.Clear();

                context.Command.CommandText = "prAddMisionDetalle";
                this.Database.AddInParameter(context.Command, "@IdMision", DbType.Int32, context.Item.Identifier);
                this.Database.AddInParameter(context.Command, "@Obc", DbType.Int32);
                this.Database.AddInParameter(context.Command, "@Day", DbType.Int32);
                this.Database.AddInParameter(context.Command, "@ObcFee", DbType.Decimal);
                this.Database.AddInParameter(context.Command, "@Flights", DbType.Decimal);
                DbParameter Obc = context.Command.Parameters["@Obc"];
                DbParameter Day = context.Command.Parameters["@Day"];
                DbParameter ObcFee = context.Command.Parameters["@ObcFee"];
                DbParameter Flights = context.Command.Parameters["@Flights"];

                foreach (var item in context.Item.QuotationDetails)
                {
                    Obc.Value = item.Obc;
                    Day.Value = item.Day;
                    ObcFee.Value = item.ObcFee;
                    Flights.Value = item.Flights;

                    this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                }
            }

            base.CommandUpdateComplete(context);
            context.Transaction.Commit();
        }
        protected override void CommandUpdateException(DataContext<Mision, int, MisionCriteria> context, Exception ex)
        {
            if (context.Transaction != null)
            {
                DbConnection connection = context.Transaction.Connection;
                context.Transaction.Rollback();

                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            base.CommandUpdateException(context, ex);
        }
        public List<Proveedor> GetProveedoresMision(int cotizacion)
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            proveedores = proveedorManager.GetProveedoresMision(new ProveedorFilter() { Cotizacion = cotizacion });

            return proveedores;
        }

        public List<Proveedor> GetProveedoresAvailable(int cotizacion)
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            proveedores = proveedorManager.GetProveedoresAvailable(new ProveedorFilter() { Cotizacion = cotizacion });

            return proveedores;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(MisionCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@EmpresaId", DbType.Int32, criteria.EmpresaId);

            return cmd;
        }

        public void AgregarProveedoresAMision(int proveedorId, int misionId, int servicioId)
        {
            try
            {
                proveedorManager.AgregarProveedoresAMision(proveedorId, misionId, servicioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.Contains("No es posible agregar mas proveedores") ? ex.Message : "Error al agregar el proveedor");
            }
        }
        public void EliminarProveedoresAMision(int proveedorId, int cotizacionId, string servicio)
        {
            try
            {
                proveedorManager.EliminarProveedoresAMision(proveedorId, cotizacionId, servicio);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el proveedor");
            }
        }
        public void Complete(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prCompleteMission");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            this.Database.ExecuteNonQuery(cmd);
        }

        public List<MisionReporte> GetReport(DateTime Del, DateTime Al)
        {
            List<MisionReporte> misions = new List<MisionReporte>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetReportMisions");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                MisionReporte mision = new MisionReporte();

                mision.QuoteXero = dr["QuoteXero"] == DBNull.Value ? "" : (string)dr["QuoteXero"];
                mision.InvoiceXero = dr["InvoiceXero"] == DBNull.Value ? "" : (string)dr["InvoiceXero"];
                mision.StartDate = dr["StartDate"] == DBNull.Value ? null : (DateTime?)dr["StartDate"];
                mision.EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"];
                mision.Client = dr["Client"] == DBNull.Value ? "" : (string)dr["Client"];
                mision.FluxReference = dr["FluxReference"] == DBNull.Value ? "" : (string)dr["FluxReference"];
                mision.ClientRef = dr["ClientRef"] == DBNull.Value ? "" : (string)dr["ClientRef"];
                mision.Costs = dr["Costs"] == DBNull.Value ? 0.0m : (decimal)dr["Costs"];
                mision.Taxes = dr["Taxes"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes"];
                mision.Taxes10 = dr["Taxes10"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes10"];
                mision.Wrapping = dr["Wrapping"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping"];
                mision.Wrapping10 = dr["Wrapping10"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping10"];
                mision.Obc = dr["Obc"] == DBNull.Value ? 0 : (int)dr["Obc"];
                mision.ObcFee = dr["ObcFee"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFee"];
                mision.ObcCost = dr["ObcCost"] == DBNull.Value ? 0.0m : (decimal)dr["ObcCost"];
                mision.ExtraBaggage = dr["ExtraBaggage"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraBaggage"];
                mision.EB10 = dr["EB10"] == DBNull.Value ? 0.0m : (decimal)dr["EB10"];
                mision.ExtraCost = dr["ExtraCost"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraCost"];
                mision.FinalCost = dr["FinalCost"] == DBNull.Value ? 0.0m : (decimal)dr["FinalCost"];
                mision.Proveedores = dr["Proveedores"] == DBNull.Value ? "" : (string)dr["Proveedores"];


                misions.Add(mision);
                //misions.Add(new MisionReporte
                //{
                //    QuoteXero = dr["QuoteXero"] == DBNull.Value ? "" : (string)dr["QuoteXero"],
                //    InvoiceXero = dr["InvoiceXero"] == DBNull.Value ? "" : (string)dr["InvoiceXero"],
                //    StartDate = dr["StartDate"] == DBNull.Value ? null : (DateTime?)dr["StartDate"],
                //    EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"],
                //    Client = dr["Client"] == DBNull.Value ? "" : (string)dr["Client"],
                //    FluxReference = dr["FluxReference"] == DBNull.Value ? "" : (string)dr["FluxReference"],
                //    ClientRef = dr["ClientRef"] == DBNull.Value ? "" : (string)dr["ClientRef"],
                //    Costs = dr["Costs"] == DBNull.Value ? 0.0m : (decimal)dr["Costs"],
                //    Taxes = dr["Taxes"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes"],
                //    Taxes10 = dr["Taxes10"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes10"],
                //    Wrapping = dr["Wrapping"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping"],
                //    Wrapping10 = dr["Wrapping10"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping10"],
                //    Obc = dr["Obc"] == DBNull.Value ? 0.0m : (decimal)dr["Obc"],
                //    ObcFee = dr["ObcFee"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFee"],
                //    ExtraBaggage = dr["ExtraBaggage"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraBaggage"],
                //    EB10 = dr["EB10"] == DBNull.Value ? 0.0m : (decimal)dr["EB10"],
                //    ExtraCost = dr["ExtraCost"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraCost"],
                //    FinalCost = dr["FinalCost"] == DBNull.Value ? 0.0m : (decimal)dr["FinalCost"]
                //});
            }

            return misions;
        }

        public List<MisionReporte> GetReportCierre(int Mision)
        {
            List<MisionReporte> misions = new List<MisionReporte>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetReportCierreMision");
            this.Database.AddInParameter(cmd, "@MisionId", DbType.Int32, Mision);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                MisionReporte mision = new MisionReporte();

                mision.QuoteXero = dr["QuoteXero"] == DBNull.Value ? "" : (string)dr["QuoteXero"];
                mision.InvoiceXero = dr["InvoiceXero"] == DBNull.Value ? "" : (string)dr["InvoiceXero"];
                mision.StartDate = dr["StartDate"] == DBNull.Value ? null : (DateTime?)dr["StartDate"];
                mision.EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"];
                mision.Client = dr["Client"] == DBNull.Value ? "" : (string)dr["Client"];
                mision.FluxReference = dr["FluxReference"] == DBNull.Value ? "" : (string)dr["FluxReference"];
                mision.ClientRef = dr["ClientRef"] == DBNull.Value ? "" : (string)dr["ClientRef"];
                mision.Costs = dr["Costs"] == DBNull.Value ? 0.0m : (decimal)dr["Costs"];
                mision.Taxes = dr["Taxes"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes"];
                mision.TaxesCotizacion = dr["TaxesCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["TaxesCotizacion"];
                mision.Taxes10 = dr["Taxes10"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes10"];
                mision.Wrapping = dr["Wrapping"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping"];
                mision.Wrapping10 = dr["Wrapping10"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping10"];
                mision.Obc = dr["Obc"] == DBNull.Value ? 0 : (int)dr["Obc"];
                mision.ObcFee = dr["ObcFee"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFee"];
                mision.ObcCost = dr["ObcCost"] == DBNull.Value ? 0.0m : (decimal)dr["ObcCost"];
                mision.ExtraBaggage = dr["ExtraBaggage"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraBaggage"];
                mision.EB10 = dr["EB10"] == DBNull.Value ? 0.0m : (decimal)dr["EB10"];
                mision.ExtraCost = dr["ExtraCost"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraCost"];
                mision.FinalCost = dr["FinalCost"] == DBNull.Value ? 0.0m : (decimal)dr["FinalCost"];
                mision.Schedule = dr["Schedule"] == DBNull.Value ? "" : (string)dr["Schedule"];
                mision.PickupCotizacion = dr["PickupCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["PickupCotizacion"];
                mision.DeliberyCotizacion = dr["DeliberyCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["DeliberyCotizacion"];
                mision.HotelCotizacion = dr["HotelCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["HotelCotizacion"];
                mision.OtherCotizacion = dr["OtherCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["OtherCotizacion"];
                mision.ProfitCotizacion = dr["ProfitCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["ProfitCotizacion"];
                mision.ObcFeeCotizacion = dr["ObcFeeCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFeeCotizacion"];
                mision.FlightsCotizacion = dr["FlightsCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["FlightsCotizacion"];
                mision.PickupCotizacion = dr["PickupCotizacion"] == DBNull.Value ? 0.0m : (decimal)dr["PickupCotizacion"];
                mision.CommentsObc = dr["ComentariosTabla"] == DBNull.Value ? "" : (string)dr["ComentariosTabla"];
                mision.CommentsPickup = dr["ComentariosPickup"] == DBNull.Value ? "" : (string)dr["ComentariosPickup"];
                mision.CommentsCustom = dr["ComentariosCustom"] == DBNull.Value ? "" : (string)dr["ComentariosCustom"];
                mision.CommentsHotel = dr["ComentariosHotel"] == DBNull.Value ? "" : (string)dr["ComentariosHotel"];
                mision.CommentsDelibery = dr["ComentariosDelibery"] == DBNull.Value ? "" : (string)dr["ComentariosDelibery"];
                mision.CommentsOther = dr["ComentariosOther"] == DBNull.Value ? "" : (string)dr["ComentariosOther"];

                mision.PickupMision = dr["PickupMision"] == DBNull.Value ? 0.0m : (decimal)dr["PickupMision"];
                mision.DeliberyMision = dr["DeliberyMision"] == DBNull.Value ? 0.0m : (decimal)dr["DeliberyMision"];
                mision.HotelMision = dr["HotelMision"] == DBNull.Value ? 0.0m : (decimal)dr["HotelMision"];
                mision.OtherMision = dr["OtherMision"] == DBNull.Value ? 0.0m : (decimal)dr["OtherMision"];
                mision.ProfitMision = dr["ProfitMision"] == DBNull.Value ? 0.0m : (decimal)dr["ProfitMision"];
                mision.ObcFeeMision = dr["ObcFeeMision"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFeeMision"];
                mision.FlightsMision = dr["FlightsMision"] == DBNull.Value ? 0.0m : (decimal)dr["FlightsMision"];
                mision.PickupMision = dr["PickupMision"] == DBNull.Value ? 0.0m : (decimal)dr["PickupMision"];

                misions.Add(mision);
                //misions.Add(new MisionReporte
                //{
                //    QuoteXero = dr["QuoteXero"] == DBNull.Value ? "" : (string)dr["QuoteXero"],
                //    InvoiceXero = dr["InvoiceXero"] == DBNull.Value ? "" : (string)dr["InvoiceXero"],
                //    StartDate = dr["StartDate"] == DBNull.Value ? null : (DateTime?)dr["StartDate"],
                //    EndDate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"],
                //    Client = dr["Client"] == DBNull.Value ? "" : (string)dr["Client"],
                //    FluxReference = dr["FluxReference"] == DBNull.Value ? "" : (string)dr["FluxReference"],
                //    ClientRef = dr["ClientRef"] == DBNull.Value ? "" : (string)dr["ClientRef"],
                //    Costs = dr["Costs"] == DBNull.Value ? 0.0m : (decimal)dr["Costs"],
                //    Taxes = dr["Taxes"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes"],
                //    Taxes10 = dr["Taxes10"] == DBNull.Value ? 0.0m : (decimal)dr["Taxes10"],
                //    Wrapping = dr["Wrapping"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping"],
                //    Wrapping10 = dr["Wrapping10"] == DBNull.Value ? 0.0m : (decimal)dr["Wrapping10"],
                //    Obc = dr["Obc"] == DBNull.Value ? 0.0m : (decimal)dr["Obc"],
                //    ObcFee = dr["ObcFee"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFee"],
                //    ExtraBaggage = dr["ExtraBaggage"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraBaggage"],
                //    EB10 = dr["EB10"] == DBNull.Value ? 0.0m : (decimal)dr["EB10"],
                //    ExtraCost = dr["ExtraCost"] == DBNull.Value ? 0.0m : (decimal)dr["ExtraCost"],
                //    FinalCost = dr["FinalCost"] == DBNull.Value ? 0.0m : (decimal)dr["FinalCost"]
                //});
            }

            return misions;
        }
    }
}
