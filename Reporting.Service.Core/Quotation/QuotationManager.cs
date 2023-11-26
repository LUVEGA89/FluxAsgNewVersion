using Reporting.Service.Core.Empresa;
using Reporting.Service.Core.Servicio;
using Reporting.Service.Core.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Quotation
{
    public class QuotationManager : Catalog<Quotation, int, QuotationCriteria>
    {
        readonly EmpresaManager empresaManager = new EmpresaManager();
        readonly ServicioManager servicioManager = new ServicioManager();

        protected override string FindPagedItemsProcedure => "prFindQuotations";
        protected override Quotation LoadItem(IDataReader dr)
        {
            Quotation quotation = new Quotation
            {
                Identifier = (int)dr["Sequence"],
                Referencia = dr["Referencia"] == DBNull.Value ? "" : (string)dr["Referencia"],
                Empresa = empresaManager.Find((int)dr["EmpresaId"]),
                Client = dr["NombreCliente"] == DBNull.Value ? "" : (string)dr["NombreCliente"],
                ClienteId = dr["ClienteId"] == DBNull.Value ? 0 : (int)dr["ClienteId"],
                ClientRef = dr["ClientRef"] == DBNull.Value ? "" : (string)dr["ClientRef"],
                Origin = dr["SiglasOrigen"] == DBNull.Value ? "" : (string)dr["SiglasOrigen"],
                Destination = dr["SiglasDestino"] == DBNull.Value ? "" : (string)dr["SiglasDestino"],
                Service = dr["TipoServicioId"] == DBNull.Value ? 0 : (int)dr["TipoServicioId"],
                ServiceObject = servicioManager.Find((int)dr["TipoServicioId"]),
                RegistradaEl = (DateTime)dr["CreadaEl"],
                RegistradaPor = (string)dr["UserName"],
                Status = (QuotationStatus)(int)dr["Status"]
            };

            switch (quotation.Service)
            {
                case 1://Obc
                    quotation.ObcServiceType = (int)dr["TipoServicioId"];
                    quotation.ObcPickup = dr["Pickup"] == DBNull.Value ? 0.0m : (decimal)dr["Pickup"];
                    quotation.ObcHotel = dr["Hotel"] == DBNull.Value ? "" : (string)dr["Hotel"];
                    quotation.ObcDelibery = dr["Delibery"] == DBNull.Value ? "" : (string)dr["Delibery"];
                    quotation.ObcCustoms = dr["Customs"] == DBNull.Value ? "" : (string)dr["Customs"];
                    quotation.ObcOther = dr["Other"] == DBNull.Value ? "" : (string)dr["Other"];
                    quotation.ObcProfit = dr["Profit"] == DBNull.Value ? "" : (string)dr["Profit"];
                    quotation.ObcSellPrice = dr["SellPrice"] == DBNull.Value ? 0.0m : (decimal)dr["SellPrice"];
                    quotation.ObcPickupDate = ((DateTime)dr["PickupDate"]);
                    //quotation.ObcCosts = ((decimal)dr["Costs"]).ToString();
                    quotation.ObcSchedule = dr["Schedule"] == DBNull.Value ? "" : (string)dr["Schedule"];
                    quotation.ObcDisclaimerEta = (DateTime)dr["Eta"];

                    //traemos el detalle
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            QuotationDetails quotationDetails = new QuotationDetails 
                            { 
                                Obc = dr["NumeroObc"] == DBNull.Value ? 0 : (int)dr["NumeroObc"],
                                Day = dr["Day"] == DBNull.Value ? 0 : (int)dr["Day"],
                                ObcFee = dr["ObcFee"] == DBNull.Value ? 0.0m : (decimal)dr["ObcFee"],
                                Flights = dr["Flights"] == DBNull.Value ? 0.0m: (decimal)dr["Flights"]
                            };
                            quotation.QuotationDetails.Add(quotationDetails);
                        }
                    }
                    break;
                case 2://Charter
                    quotation.CharterServiceType = dr["TipoServicioId"] == DBNull.Value ? 0 : (int)dr["TipoServicioId"];
                    quotation.CharterBuy = dr["Buy"] == DBNull.Value ? "" : (string)dr["Buy"];
                    quotation.CharterRute = dr["route"] == DBNull.Value ? "" : (string)dr["route"];
                    quotation.CharterAircraft = dr["Aircraft"] == DBNull.Value ? "" : (string)dr["Aircraft"];
                    quotation.CharterPositioning = dr["Positioning"] == DBNull.Value ? "" : (string)dr["Positioning"];
                    quotation.CharterLiveLeg = dr["Liveleg"] == DBNull.Value ? "" : (string)dr["Liveleg"];
                    quotation.CharterCosts = dr["Costs"] == DBNull.Value ? "" : ((decimal)dr["Costs"]).ToString();
                    break;
                case 3://Nfo
                    quotation.NfoServiceType = (int)dr["TipoServicioId"];
                    quotation.NfoBuy = dr["Buy"] == DBNull.Value ? "" : (string)dr["Buy"];
                    quotation.NfoRute = dr["route"] == DBNull.Value ? "" : (string)dr["route"];
                    quotation.NfoAirline = dr["Airline"] == DBNull.Value ? "" : (string)dr["Airline"];
                    quotation.NfoCosts = dr["Costs"] == DBNull.Value ? "" : ((decimal)dr["Costs"]).ToString();
                    break;
                case 4://HotShot
                    quotation.HostshotBuy = dr["Buy"] == DBNull.Value ? "" : (string)dr["Buy"];
                    quotation.HostshotTransitTime = dr["TransitTime"] == DBNull.Value ? "" : (string)dr["TransitTime"];
                    quotation.HostshotCosts = dr["Costs"] == DBNull.Value ? "" : ((decimal)dr["Costs"]).ToString();
                    break;
            }

            return quotation;
        }
        protected override void BeforeAddExecuted(DataContext<Quotation, int, QuotationCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
            base.BeforeAddExecuted(context);
        }
        protected override void BeforUpdateExcecuted(DataContext<Quotation, int, QuotationCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
            base.BeforUpdateExcecuted(context);
        }
        protected override DbCommand PrepareAddStatement(Quotation item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddQuotation");

            this.Database.AddInParameter(command, "@Empresa", DbType.Int32, item.Empresa.Identifier);
            this.Database.AddInParameter(command, "@Client", DbType.String, item.Client);
            this.Database.AddInParameter(command, "@ClientRef", DbType.String, item.ClientRef);
            this.Database.AddInParameter(command, "@Origin", DbType.String, item.Origin);
            this.Database.AddInParameter(command, "@Destination", DbType.String, item.Destination);
            this.Database.AddInParameter(command, "@Service", DbType.Int32, item.Service);
            this.Database.AddInParameter(command, "@Usuario", DbType.String, item.CreadaPor);

            if(item.PadreId != 0)
                this.Database.AddInParameter(command, "@PadreId", DbType.Int32, item.PadreId);

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
                    this.Database.AddInParameter(command, "@SellPrice", DbType.String, item.ObcSellPrice);
                    //this.Database.AddInParameter(command, "@Option", DbType.String, item.ObcO); esto del option es para poner el padre a una cotizacion y poder clonarla
                    this.Database.AddInParameter(command, "@PickUpDate", DbType.DateTime, item.ObcPickupDate);
                    this.Database.AddInParameter(command, "@Costs", DbType.String, item.ObcCosts);
                    this.Database.AddInParameter(command, "@Schedule", DbType.String, item.ObcSchedule);
                    this.Database.AddInParameter(command, "@Eta", DbType.DateTime, item.ObcDisclaimerEta);
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

            this.Database.AddOutParameter(command, "@IdCotizacion", DbType.Int32, 4);
            return command;
        }
        protected override DbCommand PrepareUpdateStatement(Quotation item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateQuotation");

            this.Database.AddInParameter(command, "@IdCotizacion", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@Empresa", DbType.Int32, item.Empresa.Identifier);
            this.Database.AddInParameter(command, "@Client", DbType.String, item.Client);
            this.Database.AddInParameter(command, "@ClientRef", DbType.String, item.ClientRef);
            this.Database.AddInParameter(command, "@Origin", DbType.String, item.Origin);
            this.Database.AddInParameter(command, "@Destination", DbType.String, item.Destination);
            this.Database.AddInParameter(command, "@Service", DbType.Int32, item.Service);
            this.Database.AddInParameter(command, "@Usuario", DbType.String, item.CreadaPor);

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
                    this.Database.AddInParameter(command, "@SellPrice", DbType.String, item.ObcSellPrice);
                    //this.Database.AddInParameter(command, "@Option", DbType.String, item.ObcO); esto del option es para poner el padre a una cotizacion y poder clonarla
                    this.Database.AddInParameter(command, "@PickUpDate", DbType.DateTime, item.ObcPickupDate);
                    this.Database.AddInParameter(command, "@Costs", DbType.String, item.ObcCosts);
                    this.Database.AddInParameter(command, "@Schedule", DbType.String, item.ObcSchedule);
                    this.Database.AddInParameter(command, "@Eta", DbType.DateTime, item.ObcDisclaimerEta);
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
        protected override void CommandAddComplete(DataContext<Quotation, int, QuotationCriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdCotizacion"].Value;

            if(context.Item.Service == 1) 
            {
                context.Command.Parameters.Clear();

                context.Command.CommandText = "prAddCotizacionDetalle";
                this.Database.AddInParameter(context.Command, "@CotizacionId", DbType.Int32, context.Item.Identifier);
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

            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }
        protected override void CommandUpdateComplete(DataContext<Quotation, int, QuotationCriteria> context)
        {
            if (context.Item.Service == 1)
            {
                context.Command.Parameters.Clear();

                context.Command.CommandText = "prAddCotizacionDetalle";
                this.Database.AddInParameter(context.Command, "@CotizacionId", DbType.Int32, context.Item.Identifier);
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
        protected override void CommandAddException(DataContext<Quotation, int, QuotationCriteria> context, Exception ex)
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

            base.CommandAddException(context, ex);
        }
        protected override void CommandUpdateException(DataContext<Quotation, int, QuotationCriteria> context, Exception ex)
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
        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetQuotation");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }
        protected override DbCommand PrepareFindPagedItemsStatement(QuotationCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Status", DbType.Int32, criteria.Status);
            this.Database.AddInParameter(cmd, "@EmpresaId", DbType.Int32, criteria.EmpresaId);

            return cmd;
        }
        public void Approve(int id) 
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prApproveQuotation");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            this.Database.ExecuteNonQuery(cmd);
        }
        public void Decline(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeclineQuotation");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            this.Database.ExecuteNonQuery(cmd);
        }
        public int GetLastIdQuotation()
        {
            DbCommand command = this.Database.GetStoredProcCommand("spGetLastIndexQuotation");
            IDataReader dr = this.Database.ExecuteReader(command);
            int lastId = 0;
            while (dr.Read())
            {
                lastId = (int)dr["Sequence"];

            }

            return lastId;
        }

        public int UpdateSchedule(int idQuotation, string schedule)
        {
            DbCommand command = this.Database.GetStoredProcCommand("spUpdateScheduleQuotation");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, idQuotation);
            this.Database.AddInParameter(command, "@Schedule", DbType.String, schedule);

            return this.Database.ExecuteNonQuery(command); ;
        }
    }
}
