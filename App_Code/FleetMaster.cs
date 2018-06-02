using System;
using System.Data;
using System.Data.SqlClient;
using GvkFMSAPP.DLL;

/// <summary>
/// Summary description for FleetMaster
/// </summary>
public class FleetMaster
{
    public DataSet InsertManufacturerDetails(string mfname, int mftypid, string mfmodel, int mfdist, int mfmandal, int mfcity, string mfaddress, long mfcontno, string mfcontper, string mfmail, long mftin, long mfern, int mfstatus, string mfinactby, DateTime mfinactdate, DateTime mfcreatedate, string mfcreateby, DateTime mfupdtdate, string mfupdateby)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@fmname", SqlDbType.NVarChar);
            cmd.Parameters["@fmname"].Value = mfname;
            cmd.Parameters.Add("@ftypeid", SqlDbType.Int);
            cmd.Parameters["@ftypeid"].Value = mftypid;
            cmd.Parameters.Add("@fmodel", SqlDbType.NVarChar);
            cmd.Parameters["@fmodel"].Value = mfmodel;
            cmd.Parameters.Add("@dsid", SqlDbType.Int);
            cmd.Parameters["@dsid"].Value = mfdist;
            cmd.Parameters.Add("@mid", SqlDbType.Int);
            cmd.Parameters["@mid"].Value = 0;
            cmd.Parameters.Add("@cid", SqlDbType.Int);
            cmd.Parameters["@cid"].Value = 0;
            cmd.Parameters.Add("@fmaddress", SqlDbType.NVarChar);
            cmd.Parameters["@fmaddress"].Value = mfaddress;
            cmd.Parameters.Add("@fmcontno", SqlDbType.BigInt);
            cmd.Parameters["@fmcontno"].Value = mfcontno;
            cmd.Parameters.Add("@fmcname", SqlDbType.NVarChar);
            cmd.Parameters["@fmcname"].Value = mfcontper;
            cmd.Parameters.Add("@fmemail", SqlDbType.NVarChar);
            cmd.Parameters["@fmemail"].Value = mfmail;
            cmd.Parameters.Add("@fmtin", SqlDbType.BigInt);
            cmd.Parameters["@fmtin"].Value = mftin;
            cmd.Parameters.Add("@fmern", SqlDbType.BigInt);
            cmd.Parameters["@fmern"].Value = mfern;
            cmd.Parameters.Add("@fmstatus", SqlDbType.Int);
            cmd.Parameters["@fmstatus"].Value = mfstatus;
            cmd.Parameters.Add("@fminactby", SqlDbType.Int);
            cmd.Parameters["@fminactby"].Value = mfinactby;
            cmd.Parameters.Add("@fminactdate", SqlDbType.SmallDateTime);
            cmd.Parameters["@fminactdate"].Value = mfinactdate;
            cmd.Parameters.Add("@fmcreatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@fmcreatedate"].Value = mfcreatedate;
            cmd.Parameters.Add("@fmcreateby", SqlDbType.Int);
            cmd.Parameters["@fmcreateby"].Value = mfcreateby;
            cmd.Parameters.Add("@fmupdatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@fmupdatedate"].Value = mfupdtdate;
            cmd.Parameters.Add("@fmupdateby", SqlDbType.Int);
            cmd.Parameters["@fmupdateby"].Value = mfupdateby;
            SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertFleetManufacturerDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertManufacturerDetails", 0L);
        }
        return dataSet;
    }
    public DataSet UpdateManufacturerDetails(int mfId, string mfname, int mftypid, string mfmodel, int mfdist, int mfmandal, int mfcity, string mfaddress, long mfcontno, string mfcontper, string mfmail, long mftin, long mfern)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@ManufacturerId", SqlDbType.Int);
            cmd.Parameters["@ManufacturerId"].Value = mfId;
            cmd.Parameters.Add("@ManufacturerName", SqlDbType.NVarChar);
            cmd.Parameters["@ManufacturerName"].Value = mfname;
            cmd.Parameters.Add("@FleetTypeId", SqlDbType.Int);
            cmd.Parameters["@FleetTypeId"].Value = mftypid;
            cmd.Parameters.Add("@FleetModel", SqlDbType.NVarChar);
            cmd.Parameters["@FleetModel"].Value = mfmodel;
            cmd.Parameters.Add("@DistrictId", SqlDbType.Int);
            cmd.Parameters["@DistrictId"].Value = mfdist;
            cmd.Parameters.Add("@MandalId", SqlDbType.Int);
            cmd.Parameters["@MandalId"].Value = 0;
            cmd.Parameters.Add("@CityId", SqlDbType.Int);
            cmd.Parameters["@CityId"].Value = 0;
            cmd.Parameters.Add("@ManufacturerAddress", SqlDbType.NVarChar);
            cmd.Parameters["@ManufacturerAddress"].Value = mfaddress;
            cmd.Parameters.Add("@ManufacturerContactNo", SqlDbType.BigInt);
            cmd.Parameters["@ManufacturerContactNo"].Value = mfcontno;
            cmd.Parameters.Add("@ManufacturerContactPerson", SqlDbType.NVarChar);
            cmd.Parameters["@ManufacturerContactPerson"].Value = mfcontper;
            cmd.Parameters.Add("@ManufacturerEmailId", SqlDbType.NVarChar);
            cmd.Parameters["@ManufacturerEmailId"].Value = mfmail;
            cmd.Parameters.Add("@ManufacturerTin", SqlDbType.BigInt);
            cmd.Parameters["@ManufacturerTin"].Value = mftin;
            cmd.Parameters.Add("@ManufacturerErn", SqlDbType.BigInt);
            cmd.Parameters["@ManufacturerErn"].Value = mfern;
            SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateManufacturerDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateManufacturerDetails", 0L);
        }
        return dataSet;
    }
    public DataSet FillGrid_FleetManufacturerDetails()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_FleetManufacturerDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGrid_FleetManufacturerDetails", 0L);
        }
        return dataSet;
    }
    public DataSet RowEditManufacturerDetails(int mfId)
    {
        SqlCommand cmd = new SqlCommand();
        var dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@manufactureId", SqlDbType.Int);
            cmd.Parameters["@manufactureId"].Value = mfId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditFleetManufacturerDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IRowEditManufacturerDetails", 0L);
        }
        return dataSet;
    }
    public DataSet FillGrid_FabricatorDetails()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_FabricatorDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGrid_FabricatorDetails", 0L);
        }
        return dataSet;
    }
    public DataSet InsertFabricatorDetails(string fname, int ftype, int fdist, int fmandal, int fcity, string faddress, long fcontno, string fcontper, string fpan, string femail, long ftin, long fern, int fstatus, string finactby, DateTime finactdate, DateTime fcreatedate, string fcreateby, DateTime fupdtdate, string fupdateby)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@fabname", SqlDbType.NVarChar);
            cmd.Parameters["@fabname"].Value = fname;
            cmd.Parameters.Add("@fabtypeid", SqlDbType.Int);
            cmd.Parameters["@fabtypeid"].Value = ftype;
            cmd.Parameters.Add("@fabdsid", SqlDbType.Int);
            cmd.Parameters["@fabdsid"].Value = fdist;
            cmd.Parameters.Add("@fabmid", SqlDbType.Int);
            cmd.Parameters["@fabmid"].Value = 0;
            cmd.Parameters.Add("@fabcid", SqlDbType.Int);
            cmd.Parameters["@fabcid"].Value = 0;
            cmd.Parameters.Add("@fabaddress", SqlDbType.NVarChar);
            cmd.Parameters["@fabaddress"].Value = faddress;
            cmd.Parameters.Add("@fabcontno", SqlDbType.BigInt);
            cmd.Parameters["@fabcontno"].Value = fcontno;
            cmd.Parameters.Add("@fabcontperson", SqlDbType.NVarChar);
            cmd.Parameters["@fabcontperson"].Value = fcontper;
            cmd.Parameters.Add("@fabpanno", SqlDbType.NVarChar);
            cmd.Parameters["@fabpanno"].Value = fpan;
            cmd.Parameters.Add("@fabemail", SqlDbType.NVarChar);
            cmd.Parameters["@fabemail"].Value = femail;
            cmd.Parameters.Add("@fabtin", SqlDbType.BigInt);
            cmd.Parameters["@fabtin"].Value = ftin;
            cmd.Parameters.Add("@fabern", SqlDbType.BigInt);
            cmd.Parameters["@fabern"].Value = fern;
            cmd.Parameters.Add("@fabstatus", SqlDbType.Int);
            cmd.Parameters["@fabstatus"].Value = fstatus;
            cmd.Parameters.Add("@fabinactby", SqlDbType.Int);
            cmd.Parameters["@fabinactby"].Value = Convert.ToInt32(finactby);
            cmd.Parameters.Add("@fabinactdate", SqlDbType.SmallDateTime);
            cmd.Parameters["@fabinactdate"].Value = finactdate;
            cmd.Parameters.Add("@fabcreatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@fabcreatedate"].Value = fcreatedate;
            cmd.Parameters.Add("@fabcreateby", SqlDbType.Int);
            cmd.Parameters["@fabcreateby"].Value = Convert.ToInt32(fcreateby);
            cmd.Parameters.Add("@fabupdatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@fabupdatedate"].Value = fupdtdate;
            cmd.Parameters.Add("@fabupdateby", SqlDbType.Int);
            cmd.Parameters["@fabupdateby"].Value = Convert.ToInt32(fupdateby);
            SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertFabricatorDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertFabricatorDetails", 0L);
        }
        return dataSet;
    }
    public DataSet UpdateFabricatorDetails(int fId, string fname, int ftype, int fdist, int fmandal, int fcity, string faddress, long fcontno, string fcontper, string fpan, string fmail, long ftin, long fern)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@FabricatorId", SqlDbType.Int);
            cmd.Parameters["@FabricatorId"].Value = fId;
            cmd.Parameters.Add("@FabricatorName", SqlDbType.NVarChar);
            cmd.Parameters["@FabricatorName"].Value = fname;
            cmd.Parameters.Add("@FabricatorTypeId", SqlDbType.Int);
            cmd.Parameters["@FabricatorTypeId"].Value = ftype;
            cmd.Parameters.Add("@DistrictId", SqlDbType.Int);
            cmd.Parameters["@DistrictId"].Value = fdist;
            cmd.Parameters.Add("@MandalId", SqlDbType.Int);
            cmd.Parameters["@MandalId"].Value = 0;
            cmd.Parameters.Add("@CityId", SqlDbType.Int);
            cmd.Parameters["@CityId"].Value = 0;
            cmd.Parameters.Add("@FabricatorAddress", SqlDbType.NVarChar);
            cmd.Parameters["@FabricatorAddress"].Value = faddress;
            cmd.Parameters.Add("@FabricatorContactNo", SqlDbType.BigInt);
            cmd.Parameters["@FabricatorContactNo"].Value = fcontno;
            cmd.Parameters.Add("@FabricatorContactPerson", SqlDbType.NVarChar);
            cmd.Parameters["@FabricatorContactPerson"].Value = fcontper;
            cmd.Parameters.Add("@FabricatorPanNo", SqlDbType.NVarChar);
            cmd.Parameters["@FabricatorPanNo"].Value = fpan;
            cmd.Parameters.Add("@FabricatorEmailId", SqlDbType.NVarChar);
            cmd.Parameters["@FabricatorEmailId"].Value = fmail;
            cmd.Parameters.Add("@FabricatorTin", SqlDbType.BigInt);
            cmd.Parameters["@FabricatorTin"].Value = ftin;
            cmd.Parameters.Add("@FabricatorErn", SqlDbType.BigInt);
            cmd.Parameters["@FabricatorErn"].Value = fern;
            SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateFabricatorDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateFabricatorDetails", 0L);
        }
        return dataSet;
    }
    public DataSet RowEditFabricatorDetails(int fId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@fabricatorId", SqlDbType.Int);
            cmd.Parameters["@fabricatorId"].Value = fId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditFabricatorDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IRowEditFabricatorDetails", 0L);
        }
        return dataSet;
    }
    public DataSet FillGridSpareParts()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_SpareParts]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-FillGridSpareParts", 0L);
        }
        return dataSet;
    }
    public DataSet EditSpareParts(int spId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@SpID", SqlDbType.Int);
            cmd.Parameters["@SpID"].Value = spId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_GetEditSparePartDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-FillGridSpareParts", 0L);
        }
        return dataSet;
    }
    public int DeleteSpareParts(int id)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@SpID", SqlDbType.Int);
            cmd.Parameters["@SpID"].Value = id;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_DeactivateSparePart]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-FillGridSpareParts", 0L);
        }
        return num;
    }
    public int UpdateSpareParts(int sparePartId, string spareName, int manufacturerSpareId, int manufacturerId, int sparePartGroupId, string groupName, Decimal cost)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@SparePartID", SqlDbType.Int).Value = sparePartId;
            cmd.Parameters.Add("@SpareName", SqlDbType.VarChar);
            cmd.Parameters["@SpareName"].Value = spareName;
            cmd.Parameters.Add("@ManufacturerSpareID", SqlDbType.Int);
            cmd.Parameters["@ManufacturerSpareID"].Value = manufacturerSpareId;
            cmd.Parameters.Add("@ManufacturerID", SqlDbType.Int);
            cmd.Parameters["@ManufacturerID"].Value = manufacturerId;
            cmd.Parameters.Add("@SparePartGroupID", SqlDbType.Int);
            cmd.Parameters["@SparePartGroupID"].Value = sparePartGroupId;
            cmd.Parameters.Add("@GroupName", SqlDbType.VarChar);
            cmd.Parameters["@GroupName"].Value = groupName;
            cmd.Parameters.Add("@Cost", SqlDbType.Money);
            cmd.Parameters["@Cost"].Value = cost;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_Upd_SpareParts]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: btSave_Click()-UpdateSparePart", 0L);
        }
        return num;
    }
    public int InsertSpareParts(string spareName, int manufacturerSpareId, int manufacturerId, int sparePartGroupId, string groupName, Decimal cost)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@SpareName", SqlDbType.VarChar);
            cmd.Parameters["@SpareName"].Value = spareName;
            cmd.Parameters.Add("@ManufacturerSpareID", SqlDbType.Int);
            cmd.Parameters["@ManufacturerSpareID"].Value = manufacturerSpareId;
            cmd.Parameters.Add("@ManufacturerID", SqlDbType.Int);
            cmd.Parameters["@ManufacturerID"].Value = manufacturerId;
            cmd.Parameters.Add("@SparePartGroupID", SqlDbType.Int);
            cmd.Parameters["@SparePartGroupID"].Value = sparePartGroupId;
            cmd.Parameters.Add("@GroupName", SqlDbType.VarChar);
            cmd.Parameters["@GroupName"].Value = groupName;
            cmd.Parameters.Add("@Cost", SqlDbType.Money);
            cmd.Parameters["@Cost"].Value = cost;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_Ins_SpareParts]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: btSave_Click()-InsertSparePart", 0L);
        }
        return num;
    }
    public DataSet FillGrid_VehicleTypes()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_VehicleTypes]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGrid_VehicleTypes", 0L);
        }
        return dataSet;
    }
    public DataSet InsertVehicleTypes(string vehicleType, string vehicleTypeDescription)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@vehicletype", SqlDbType.NVarChar);
            cmd.Parameters["@vehicletype"].Value = vehicleType;
            cmd.Parameters.Add("@vehicledescription", SqlDbType.NVarChar);
            cmd.Parameters["@vehicledescription"].Value = vehicleTypeDescription;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertVehicleTypesDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertVehicleTypes", 0L);
        }
        return dataSet;
    }
    public DataSet UpdateVehicleTypes(int vehicleId, string vehicleType, string vehicleTypeDescription)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@vehicleid", SqlDbType.Int);
            cmd.Parameters["@vehicleid"].Value = vehicleId;
            cmd.Parameters.Add("@vehicletype", SqlDbType.NVarChar);
            cmd.Parameters["@vehicletype"].Value = vehicleType;
            cmd.Parameters.Add("@vehicledescription", SqlDbType.NVarChar);
            cmd.Parameters["@vehicledescription"].Value = vehicleTypeDescription;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateVehicleTypesDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateVehicleTypes", 0L);
        }
        return dataSet;
    }
    public DataSet RowEditVehicleTypes(int vehicleId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@vehicleTypeId", SqlDbType.Int);
            cmd.Parameters["@vehicleTypeId"].Value = vehicleId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditVehicleTypes]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IRowEditVehicleTypes", 0L);
        }
        return dataSet;
    }
    public DataSet FillGrid_TyresDetails()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_TyresDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGrid_TyresDetails", 0L);
        }
        return dataSet;
    }
    public DataSet InsertTyresDetails(string tyreItemCode, string tyreNumber, string make, string model, string size, int tStatus, DateTime tInactdate, DateTime tCreatedate, string tCreateby, DateTime tUpdatedate, string tUpdateby)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@tyreItemCode", SqlDbType.NChar);
            cmd.Parameters["@tyreItemCode"].Value = tyreItemCode;
            cmd.Parameters.Add("@tyreNumber", SqlDbType.NVarChar);
            cmd.Parameters["@tyreNumber"].Value = tyreNumber;
            cmd.Parameters.Add("@tMake", SqlDbType.NVarChar);
            cmd.Parameters["@tMake"].Value = make;
            cmd.Parameters.Add("@tModel", SqlDbType.NVarChar);
            cmd.Parameters["@tModel"].Value = model;
            cmd.Parameters.Add("@tSize", SqlDbType.NVarChar);
            cmd.Parameters["@tSize"].Value = size;
            cmd.Parameters.Add("@tStatus", SqlDbType.Int);
            cmd.Parameters["@tStatus"].Value = tStatus;
            cmd.Parameters.Add("@tInactdate", SqlDbType.SmallDateTime);
            cmd.Parameters["@tInactdate"].Value = tInactdate;
            cmd.Parameters.Add("@tCreatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@tCreatedate"].Value = tCreatedate;
            cmd.Parameters.Add("@tCreateby", SqlDbType.Int);
            cmd.Parameters["@tCreateby"].Value = tCreateby;
            cmd.Parameters.Add("@tUpdatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@tUpdatedate"].Value = tUpdatedate;
            cmd.Parameters.Add("@tUpdateby", SqlDbType.Int);
            cmd.Parameters["@tUpdateby"].Value = tUpdateby;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertTyresDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertTyresDetails", 0L);
        }
        return dataSet;
    }
    public DataSet UpdateTyresDetails(int tyreId, string tyreItemCode, string tyreNumber, string make, string model, string size)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@tyreId", SqlDbType.Int);
            cmd.Parameters["@tyreId"].Value = tyreId;
            cmd.Parameters.Add("@tyreItemCode", SqlDbType.NChar);
            cmd.Parameters["@tyreItemCode"].Value = tyreItemCode;
            cmd.Parameters.Add("@tyreNumber", SqlDbType.NVarChar);
            cmd.Parameters["@tyreNumber"].Value = tyreNumber;
            cmd.Parameters.Add("@Make", SqlDbType.NVarChar);
            cmd.Parameters["@Make"].Value = make;
            cmd.Parameters.Add("@Model", SqlDbType.NVarChar);
            cmd.Parameters["@Model"].Value = model;
            cmd.Parameters.Add("@Size", SqlDbType.NVarChar);
            cmd.Parameters["@Size"].Value = size;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateTyresDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateTyresDetails", 0L);
        }
        return dataSet;
    }
    public DataSet RowEditTyresDetails(int tyreId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@tyreId", SqlDbType.Int);
            cmd.Parameters["@tyreId"].Value = tyreId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditTyresDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IRowEditTyresDetails", 0L);
        }
        return dataSet;
    }
    public DataSet InsertBatteryDetails(string mbatteryitemcode, string mbmake, string mbmodel, string mbcapacity, DateTime expiry, int mbstatus, DateTime mbinactdate, DateTime mbcreationdate, string mbcreateby, DateTime mbupdatedate, string mbupdateby, ref string mboutput)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@batteryitemcode", SqlDbType.NChar);
            cmd.Parameters["@batteryitemcode"].Value = mbatteryitemcode;
            cmd.Parameters.Add("@bmake", SqlDbType.NVarChar);
            cmd.Parameters["@bmake"].Value = mbmake;
            cmd.Parameters.Add("@bmodel", SqlDbType.NVarChar);
            cmd.Parameters["@bmodel"].Value = mbmodel;
            cmd.Parameters.Add("@bcapacity", SqlDbType.NVarChar);
            cmd.Parameters["@bcapacity"].Value = mbcapacity;
            cmd.Parameters.Add("@bexpiry", SqlDbType.DateTime);
            cmd.Parameters["@bexpiry"].Value = expiry;
            cmd.Parameters.Add("@bstatus", SqlDbType.Int);
            cmd.Parameters["@bstatus"].Value = mbstatus;
            cmd.Parameters.Add("@binactdate", SqlDbType.SmallDateTime);
            cmd.Parameters["@binactdate"].Value = mbinactdate;
            cmd.Parameters.Add("@bcreationdate", SqlDbType.SmallDateTime);
            cmd.Parameters["@bcreationdate"].Value = mbcreationdate;
            cmd.Parameters.Add("@bcreateby", SqlDbType.Int);
            cmd.Parameters["@bcreateby"].Value = mbcreateby;
            cmd.Parameters.Add("@bupdatedate", SqlDbType.SmallDateTime);
            cmd.Parameters["@bupdatedate"].Value = mbupdatedate;
            cmd.Parameters.Add("@bupdateby", SqlDbType.Int);
            cmd.Parameters["@bupdateby"].Value = mbupdateby;
            cmd.Parameters.Add("@output", SqlDbType.NVarChar, 50);
            cmd.Parameters["@output"].Direction = ParameterDirection.Output;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertBatteryDetails]");
            mboutput = cmd.Parameters["@output"].Value.ToString();
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSBatterydetails;Method: Page_Load()-InsertBatteryDetails", 0L);
        }
        return dataSet;
    }

    public DataSet UpdateBatteryDetails(int ubatid, string batitemcode, string umake, string umodel, string ucapac, DateTime expiry)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@BatteryId", SqlDbType.Int);
            cmd.Parameters["@BatteryId"].Value = ubatid;
            cmd.Parameters.Add("@BatteryItemCode", SqlDbType.NChar);
            cmd.Parameters["@BatteryItemCode"].Value = batitemcode;
            cmd.Parameters.Add("@Make", SqlDbType.NVarChar);
            cmd.Parameters["@Make"].Value = umake;
            cmd.Parameters.Add("@Model", SqlDbType.NVarChar);
            cmd.Parameters["@Model"].Value = umodel;
            cmd.Parameters.Add("@Capacity", SqlDbType.NVarChar);
            cmd.Parameters["@Capacity"].Value = ucapac;
            cmd.Parameters.Add("@Expiry", SqlDbType.DateTime);
            cmd.Parameters["@Expiry"].Value = expiry;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateBatteryDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSBatterydetails;Method: Page_Load()-UpdateBatteryDetails", 0L);
        }
        return dataSet;
    }

    public DataSet FillGrid_BatteryDetails()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_BatteryDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSBatterydetails;Method: Page_Load()-IFillGrid_BatteryDetails", 0L);
        }
        return dataSet;
    }

    public DataSet RowEditBatteryDetails(int bfId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@batteryId", SqlDbType.Int);
            cmd.Parameters["@batteryId"].Value = bfId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditBatteryDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSBatterydetails;Method: Page_Load()-IRowEditBatteryDetails", 0L);
        }
        return dataSet;
    }
    public DataSet FillGridAgencyDetails()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_AgencyDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-FillGridAgencyDetails", 0L);
        }
        return dataSet;
    }

    public DataSet EditAgencyDetails(int agencyId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@AgencyID", SqlDbType.Int);
            cmd.Parameters["@AgencyID"].Value = agencyId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_GetEditAgencyDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-FillGridAgencyDetails", 0L);
        }
        return dataSet;
    }

    public int DeleteAgencyDetails(int agencyId)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@AgencyID", SqlDbType.Int);
            cmd.Parameters["@AgencyID"].Value = agencyId;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_DeactivateAgency]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-FillGridAgencyDetails", 0L);
        }
        return num;
    }

    public int InsertAgencyDetails(string agencyName,  int districtId, int mandalId, int cityId, long contactNum, string panNum, long tin, string address)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@AgencyName", SqlDbType.VarChar);
            cmd.Parameters["@AgencyName"].Value = agencyName;
            cmd.Parameters.Add("@DistrictID", SqlDbType.Int);
            cmd.Parameters["@DistrictID"].Value = districtId;
            cmd.Parameters.Add("@MandalID", SqlDbType.Int);
            cmd.Parameters["@MandalID"].Value = 0;
            cmd.Parameters.Add("@CityID", SqlDbType.Int);
            cmd.Parameters["@CityID"].Value = 0;
            cmd.Parameters.Add("@ContactNum", SqlDbType.BigInt);
            cmd.Parameters["@ContactNum"].Value = contactNum;
            cmd.Parameters.Add("@PANNum", SqlDbType.NVarChar);
            cmd.Parameters["@PANNum"].Value = panNum;
            cmd.Parameters.Add("@TIN", SqlDbType.BigInt);
            cmd.Parameters["@TIN"].Value = tin;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar);
            cmd.Parameters["@Address"].Value = address;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_Ins_AgencyDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: btnSaveAgencyDetails_Click()-InsertAgencyDetails", 0L);
        }
        return num;
    }

    public int UpdateAgencyDetails(int agencyId, string agencyName, int districtId, int mandalId, int cityId, long contactNum, string panNum, long tin, string address)
    {
        SqlCommand cmd = new SqlCommand();
        int num = 0;
        try
        {
            cmd.Parameters.Add("@AgencyID", SqlDbType.Int);
            cmd.Parameters["@AgencyID"].Value = agencyId;
            cmd.Parameters.Add("@AgencyName", SqlDbType.VarChar);
            cmd.Parameters["@AgencyName"].Value = agencyName;
            cmd.Parameters.Add("@DistrictID", SqlDbType.Int);
            cmd.Parameters["@DistrictID"].Value = districtId;
            cmd.Parameters.Add("@MandalID", SqlDbType.Int);
            cmd.Parameters["@MandalID"].Value = 0;
            cmd.Parameters.Add("@CityID", SqlDbType.Int);
            cmd.Parameters["@CityID"].Value = 0;
            cmd.Parameters.Add("@ContactNum", SqlDbType.BigInt);
            cmd.Parameters["@ContactNum"].Value = contactNum;
            cmd.Parameters.Add("@PANNum", SqlDbType.NVarChar);
            cmd.Parameters["@PANNum"].Value = panNum;
            cmd.Parameters.Add("@TIN", SqlDbType.BigInt);
            cmd.Parameters["@TIN"].Value = tin;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar);
            cmd.Parameters["@Address"].Value = address;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_Upd_AgencyDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: btnSaveAgencyDetails_Click()-UpdateAgencyDetails", 0L);
        }
        return num;
    }
    public DataSet FillInsuranceAgencies()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_GetInsuranceAgency]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGridMapEquipments", 0L);
        }
        return dataSet;
    }

    public DataSet GetInsuranceAgenciesByInsuranceId(int insuranceId)
    {
        DataSet dataSet = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@InsuranceId", SqlDbType.Int);
            cmd.Parameters["@InsuranceId"].Value = insuranceId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_GetInsuranceAgencyByInsuranceId]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGridMapEquipments", 0L);
        }
        return dataSet;
    }

    public int InsertInsuranceAgency(string insuranceAgency, string address, string contactPerson, long contactNumber)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@InsuranceAgency", SqlDbType.VarChar);
            cmd.Parameters["@InsuranceAgency"].Value = insuranceAgency;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar);
            cmd.Parameters["@Address"].Value = address;
            cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar);
            cmd.Parameters["@ContactPerson"].Value = contactPerson;
            cmd.Parameters.Add("@ContactNumber", SqlDbType.BigInt);
            cmd.Parameters["@ContactNumber"].Value = contactNumber;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_Insert_InsuranceAgencies]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertInsuranceAgency", 0L);
        }
        return num;
    }

    public int UpdateInsuranceAgency(int insuranceId, string insuranceAgency, string address, string contactPerson, long contactNumber)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@InsuranceId", SqlDbType.Int);
            cmd.Parameters["@InsuranceId"].Value = insuranceId;
            cmd.Parameters.Add("@InsuranceAgency", SqlDbType.VarChar);
            cmd.Parameters["@InsuranceAgency"].Value = insuranceAgency;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar);
            cmd.Parameters["@Address"].Value = address;
            cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar);
            cmd.Parameters["@ContactPerson"].Value = contactPerson;
            cmd.Parameters.Add("@ContactNumber", SqlDbType.BigInt);
            cmd.Parameters["@ContactNumber"].Value = contactNumber;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_FMS_UpdInsuranceAgencyDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateInsuranceAgency", 0L);
        }
        return num;
    }

    public int DeleteInsuranceAgency(int insuranceId)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@InsuranceId", SqlDbType.Int);
            cmd.Parameters["@InsuranceId"].Value = insuranceId;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_DeleteInsuranceAgency]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-DeleteInsuranceAgencyDetails", 0L);
        }
        return num;
    }

    public int CheckInsuranceAgency(string insuranceAgency)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@InsuranceAgency", SqlDbType.VarChar);
            cmd.Parameters["@InsuranceAgency"].Value = insuranceAgency;
            num = Convert.ToInt32(SQLHelper.ExecuteScalar(cmd, CommandType.StoredProcedure, "P_CheckInsuranceAgencyByInsuranceAgency"));
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-CheckInsuranceAgency", 0L);
        }
        return num;
    }

    public int CheckInsuranceAgency(int insuranceId, string insuranceAgency)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@InsuranceId", SqlDbType.Int);
            cmd.Parameters["@InsuranceId"].Value = insuranceId;
            cmd.Parameters.Add("@InsuranceAgency", SqlDbType.VarChar);
            cmd.Parameters["@InsuranceAgency"].Value = insuranceAgency;
            num = Convert.ToInt32(SQLHelper.ExecuteScalar(cmd, CommandType.StoredProcedure, "P_CheckInsuranceAgency"));
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-CheckInsuranceAgency", 0L);
        }
        return num;
    }
    public DataSet GetSelectAllMedicalEquipmentByEquipmentTypeId(int equipmentTYpeId)
    {
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@EquipmentTypeId",equipmentTYpeId);
            return SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "P_SC_SelectMedicalEquipmentsByEquipmentTypeId");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.ToString(), "Class: VehicleInsurance - Method: GetSelectAllRoadTax()", 0L);
            return null;
        }
    }
    public int DeleteVehicleEquipmentMapping(int vehicleId)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@VehicleId", SqlDbType.Int);
            cmd.Parameters["@VehicleId"].Value = vehicleId;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[dbo].[P_FMS_DeleteVehicleEquipmentMapping]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster.cs ;Method: Page_Load()-DeleteVehicleEquipmentMapping", 0L);
        }
        return num;
    }
    public int InsertVehicleEquipmentMapping(int medicalEquipmentId, int vehicleId)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@VehicleId", SqlDbType.Int);
            cmd.Parameters["@VehicleId"].Value = vehicleId;
            cmd.Parameters.Add("@MedicalEquipmentId", SqlDbType.Int);
            cmd.Parameters["@MedicalEquipmentId"].Value = medicalEquipmentId;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_FMS_InsertVehicleEquipmentMapping]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertVehicleEquipmentMapping", 0L);
        }
        return num;
    }
    public  DataSet GetVehicles()
    {
        try
        {
            return SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "P_Get_VehiclesForMapping");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "Class: DistrictVehicleMapping - Method: GetVehicles()",0L);
            return null;
        }
    }
    public DataSet FillManufacturerNames()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_ManufacturerNames]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillManufacturerNames", 0L);
        }
        return dataSet;
    }
    public DataSet FillGrid_MaintenanceWorksServiceGroup()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_MaintenanceWorksServiceGroup]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGrid_MaintenanceWorksServiceGroup", 0L);
        }
        return dataSet;
    }
    public DataSet InsertMaintenanceWorksServiceGroupDetails(string serviceGroupName, int manufacturerId, DateTime creationDate, int createdBy)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@serviceGroupName", SqlDbType.NVarChar);
            cmd.Parameters["@serviceGroupName"].Value = serviceGroupName;
            cmd.Parameters.Add("@manufacturerId", SqlDbType.Int);
            cmd.Parameters["@manufacturerId"].Value = manufacturerId;
            cmd.Parameters.Add("@createdDate", SqlDbType.SmallDateTime);
            cmd.Parameters["@createdDate"].Value = creationDate;
            cmd.Parameters.Add("@createdBy", SqlDbType.Int);
            cmd.Parameters["@createdBy"].Value = createdBy;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertMaintenanceWorksServiceGroupDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertMaintenanceWorksServiceGroupDetails", 0L);
        }
        return dataSet;
    }
    public DataSet UpdateMaintenanceWorksServiceGroupDetails(int serviceGroupId, string serviceGroupName, int manufacturerId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@serviceGroupId", SqlDbType.Int);
            cmd.Parameters["@serviceGroupId"].Value = serviceGroupId;
            cmd.Parameters.Add("@serviceGroupName", SqlDbType.NVarChar);
            cmd.Parameters["@serviceGroupName"].Value = serviceGroupName;
            cmd.Parameters.Add("@manufacturerId", SqlDbType.Int);
            cmd.Parameters["@manufacturerId"].Value = manufacturerId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateMaintenanceWorksServiceGroupDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateMaintenanceWorksServiceGroupDetails", 0L);
        }
        return dataSet;
    }
    public DataSet RowEditMaintenanceWorksServiceGroupDetails(int sgId)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@serviceGroupId", SqlDbType.Int);
            cmd.Parameters["@serviceGroupId"].Value = sgId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditMaintenanceWorksServiceGroupDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IRowEditMaintenanceWorksServiceGroupDetails", 0L);
        }
        return dataSet;
    }
    public DataSet FillServiceGroupNames()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_ServiceGroupNames]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillServiceGroupNames", 0L);
        }
        return dataSet;
    }
    public DataSet FillGrid_MaintenanceWorksMaster()
    {
        DataSet dataSet = new DataSet();
        try
        {
            dataSet = SQLHelper.ExecuteAdapter(new SqlCommand(), CommandType.StoredProcedure, "[dbo].[P_Get_MaintenanceWorksMaster]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IFillGrid_MaintenenceWorksMaster", 0L);
        }
        return dataSet;
    }
    public DataSet InsertMaintenanceWorksMasterDetails(int serviceGroupId, int vehicleManufacturer, string serviceGroupName, string serviceName, string subserviceName, Decimal costAGrade, Decimal costOtherThanAGrade, string timeTaken, int flag)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@serviceGroupId", SqlDbType.Int);
            cmd.Parameters["@serviceGroupId"].Value = serviceGroupId;
            cmd.Parameters.Add("@VehicleManufacturer", SqlDbType.Int);
            cmd.Parameters["@VehicleManufacturer"].Value = vehicleManufacturer;
            cmd.Parameters.Add("@ServiceGroupName", SqlDbType.VarChar);
            cmd.Parameters["@ServiceGroupName"].Value = serviceGroupName;
            cmd.Parameters.Add("@serviceName", SqlDbType.NVarChar);
            cmd.Parameters["@serviceName"].Value = serviceName;
            cmd.Parameters.Add("@subserviceName", SqlDbType.NVarChar);
            cmd.Parameters["@subserviceName"].Value = subserviceName;
            cmd.Parameters.Add("@costAGrade", SqlDbType.Money);
            cmd.Parameters["@costAGrade"].Value = costAGrade;
            cmd.Parameters.Add("@costOtherThanAGrade", SqlDbType.Money);
            cmd.Parameters["@costOtherThanAGrade"].Value = costOtherThanAGrade;
            cmd.Parameters.Add("@TimeTaken", SqlDbType.VarChar);
            cmd.Parameters["@TimeTaken"].Value = timeTaken;
            cmd.Parameters.AddWithValue("@flag", flag);
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_InsertMaintenanceWorksMasterDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-InsertMaintenanceWorksMasterDetails", 0L);
        }
        return dataSet;
    }
    public DataSet RowEditMaintenanceWorksMasterDetails(int seviceId)
    {
        var cmd = new SqlCommand();
        var dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@serviceId", SqlDbType.Int);
            cmd.Parameters["@serviceId"].Value = seviceId;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_EditMaintenanceWorksMasterDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-IRowEditMaintenanceWorksMasterDetails", 0L);
        }
        return dataSet;
    }

    public DataSet UpdateMaintenanceWorksMasterDetails(int serviceId, int serviceGroupId, string serviceName, decimal costAGrade, decimal costOtherThanAGrade, string subserviceName, string timeTaken)
    {
        SqlCommand cmd = new SqlCommand();
        DataSet dataSet = new DataSet();
        try
        {
            cmd.Parameters.Add("@serviceId", SqlDbType.Int);
            cmd.Parameters["@serviceId"].Value = serviceId;
            cmd.Parameters.Add("@serviceGroupID", SqlDbType.Int).Value = serviceGroupId;
            cmd.Parameters.Add("@subserviceName", SqlDbType.NVarChar);
            cmd.Parameters["@subserviceName"].Value = subserviceName;
            cmd.Parameters.Add("@serviceName", SqlDbType.NVarChar);
            cmd.Parameters["@serviceName"].Value = serviceName;
            cmd.Parameters.Add("@costAGrade", SqlDbType.Money);
            cmd.Parameters["@costAGrade"].Value = costAGrade;
            cmd.Parameters.Add("@costOtherThanAGrade", SqlDbType.Money);
            cmd.Parameters["@costOtherThanAGrade"].Value = costOtherThanAGrade;
            cmd.Parameters.Add("@TimeTaken", SqlDbType.VarChar);
            cmd.Parameters["@TimeTaken"].Value = timeTaken;
            dataSet = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_UpdateMaintenanceWorksMasterDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSFleetMaster;Method: Page_Load()-UpdateMaintenanceWorksMasterDetails", 0L);
        }
        return dataSet;
    }
    public DataSet GetSubService(string subservice)
    {
        var cmd = new SqlCommand();
        cmd.Parameters.Add("@serviceGroupName", SqlDbType.NVarChar);
        cmd.Parameters["@serviceGroupName"].Value = Convert.ToString(subservice);
        var ds = SQLHelper.ExecuteAdapter(cmd, CommandType.StoredProcedure, "[dbo].[P_Get_ManufacturerName_SelectedIndex]");
        return ds;
    }
    public int InsPetroCardIssueDetails(int districtId, string petroCardNum, int agencyId, int cardTypeId, DateTime validityEndDate, int issuedToFe, DateTime petroCardIssuedDate, int status, int createdBy, DateTime createdDate, int updatedBy, DateTime updatedDate, int vehicleId, int userDistrictId)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@DistrictID", SqlDbType.Int);
            cmd.Parameters["@DistrictID"].Value = districtId;
            cmd.Parameters.Add("@PetroCardNum", SqlDbType.VarChar);
            cmd.Parameters["@PetroCardNum"].Value = petroCardNum;
            cmd.Parameters.Add("@AgencyID", SqlDbType.Int);
            cmd.Parameters["@AgencyID"].Value = agencyId;
            cmd.Parameters.Add("@CardTypeID", SqlDbType.Int);
            cmd.Parameters["@CardTypeID"].Value = cardTypeId;
            cmd.Parameters.Add("@ValidityEndDate", SqlDbType.DateTime);
            cmd.Parameters["@ValidityEndDate"].Value = validityEndDate;
            cmd.Parameters.Add("@IssuedToFE", SqlDbType.Int);
            cmd.Parameters["@IssuedToFE"].Value = issuedToFe;
            cmd.Parameters.Add("@PetroCardIssuedDate", SqlDbType.DateTime);
            cmd.Parameters["@PetroCardIssuedDate"].Value = petroCardIssuedDate;
            cmd.Parameters.Add("@Status", SqlDbType.Bit);
            cmd.Parameters["@Status"].Value = 0;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);
            cmd.Parameters["@CreatedBy"].Value = createdBy;
            cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
            cmd.Parameters["@CreatedDate"].Value = createdDate;
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int);
            cmd.Parameters["@UpdatedBy"].Value = updatedBy;
            cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime);
            cmd.Parameters["@UpdatedDate"].Value = updatedDate;
            cmd.Parameters.Add("@VehicleID", SqlDbType.Int);
            cmd.Parameters["@VehicleID"].Value = vehicleId;
            cmd.Parameters.Add("@UserDistrictID", SqlDbType.Int);
            cmd.Parameters["@UserDistrictID"].Value = userDistrictId;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_InsPetroCardIssueDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FuelManagement ;Method: btSave_Click-InsPetroCardIssueDetails", 0L);
        }
        return num;
    }
    public int UpdPetroCardIssueDetails(int petroCardIssueId, int districtId, string petroCardNum, int agencyId, int cardTypeId, DateTime validityEndDate, int issuedToFe, DateTime petroCardIssuedDate, int vehicleId, int userDistrictId)
    {
        int num = 0;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@PetroCardIssueID", SqlDbType.Int);
            cmd.Parameters["@PetroCardIssueID"].Value = petroCardIssueId;
            cmd.Parameters.Add("@DistrictID", SqlDbType.Int);
            cmd.Parameters["@DistrictID"].Value = districtId;
            cmd.Parameters.Add("@PetroCardNum", SqlDbType.VarChar);
            cmd.Parameters["@PetroCardNum"].Value = petroCardNum;
            cmd.Parameters.Add("@AgencyID", SqlDbType.Int);
            cmd.Parameters["@AgencyID"].Value = agencyId;
            cmd.Parameters.Add("@CardTypeID", SqlDbType.Int);
            cmd.Parameters["@CardTypeID"].Value = cardTypeId;
            cmd.Parameters.Add("@ValidityEndDate", SqlDbType.DateTime);
            cmd.Parameters["@ValidityEndDate"].Value = validityEndDate;
            cmd.Parameters.Add("@IssuedToFE", SqlDbType.Int);
            cmd.Parameters["@IssuedToFE"].Value = issuedToFe;
            cmd.Parameters.Add("@PetroCardIssuedDate", SqlDbType.DateTime);
            cmd.Parameters["@PetroCardIssuedDate"].Value = petroCardIssuedDate;
            cmd.Parameters.Add("@VehicleID", SqlDbType.Int);
            cmd.Parameters["@VehicleID"].Value = vehicleId;
            cmd.Parameters.Add("@UserDistrictID", SqlDbType.Int);
            cmd.Parameters["@UserDistrictID"].Value = userDistrictId;
            num = SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "[P_UpdPetroCardIssueDetails]");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FuelManagement ;Method: btSave_Click-UpdPetroCardIssueDetails", 0L);
        }
        return num;
    }
    public  int InsNewVehAllocation_new(int vehicleId, string vehicleNo, int distId, string district, int segmentId, string segment, int mandalId, string mandal, int cityId, string city, int baseLocationId, string baseLocation, string contactNumber, string flag, string newSegFlag, string newSegMandalIds, string latitude, string longitude, string vehType)
    {
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@VehicleNumber", SqlDbType.VarChar);
            cmd.Parameters["@VehicleNumber"].Value = vehicleNo;
            cmd.Parameters.Add("@VehicleId", SqlDbType.Int);
            cmd.Parameters["@VehicleId"].Value = vehicleId;
            cmd.Parameters.Add("@District", SqlDbType.VarChar);
            cmd.Parameters["@District"].Value = district;
            cmd.Parameters.Add("@DistrictId", SqlDbType.Int);
            cmd.Parameters["@DistrictId"].Value = distId;
            cmd.Parameters.Add("@Segment", SqlDbType.VarChar);
            cmd.Parameters["@Segment"].Value = segment;
            cmd.Parameters.Add("@SegmentId", SqlDbType.Int);
            cmd.Parameters["@SegmentId"].Value = segmentId;
            cmd.Parameters.Add("@Mandal", SqlDbType.VarChar);
            cmd.Parameters["@Mandal"].Value = mandal;
            cmd.Parameters.Add("@MandalId", SqlDbType.Int);
            cmd.Parameters["@MandalId"].Value = mandalId;
            cmd.Parameters.Add("@City", SqlDbType.VarChar);
            cmd.Parameters["@City"].Value = city;
            cmd.Parameters.Add("@CityId", SqlDbType.Int);
            cmd.Parameters["@CityId"].Value = cityId;
            cmd.Parameters.Add("@BaseLocation", SqlDbType.VarChar);
            cmd.Parameters["@BaseLocation"].Value = baseLocation;
            cmd.Parameters.Add("@BaseLocationId", SqlDbType.Int);
            cmd.Parameters["@BaseLocationId"].Value = baseLocationId;
            cmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar);
            cmd.Parameters["@ContactNumber"].Value = contactNumber;
            cmd.Parameters.Add("@Flag", SqlDbType.VarChar);
            cmd.Parameters["@Flag"].Value = flag;
            cmd.Parameters.Add("@NewSegFlag", SqlDbType.VarChar);
            cmd.Parameters["@NewSegFlag"].Value = newSegFlag;
            cmd.Parameters.Add("@MandalIds", SqlDbType.VarChar);
            cmd.Parameters["@MandalIds"].Value = newSegMandalIds;
            cmd.Parameters.Add("@Latitude", SqlDbType.Float);
            cmd.Parameters["@Latitude"].Value = latitude;
            cmd.Parameters.Add("@Longitude", SqlDbType.Float);
            cmd.Parameters["@Longitude"].Value = longitude;
            cmd.Parameters.Add("@vehType", SqlDbType.VarChar);
            cmd.Parameters["@vehType"].Value = vehType;

            return SQLHelper.ExecuteNonQuery(cmd, CommandType.StoredProcedure, "P_VAS_InsNewVehAllocation_new");
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
}