﻿using eSunSpeed.DataAccess;
using eSunSpeed.Formatting;
using eSunSpeedDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eSunSpeed.BusinessLogic
{
    public class MatRcvdPartyBL
    {
        private DBHelper _dbHelper = new DBHelper();

        #region Save MatRcvd Voucher
        public bool SaveMatRcvdVoucher(MatRcvdModel objrcvd)
        {
            string Query = string.Empty;
            bool isSaved = true;

            try
            {
                DBParameterCollection paramCollection = new DBParameterCollection();

                paramCollection.Add(new DBParameter("@VoucherNumber", objrcvd.Voucher_Number));
                paramCollection.Add(new DBParameter("@Series", objrcvd.Series));
                paramCollection.Add(new DBParameter("@MatRcvdDate", objrcvd.Rcvd_Date, System.Data.DbType.DateTime));
                //paramCollection.Add(new DBParameter("@BillNo", objSales.BillNo));
                //paramCollection.Add(new DBParameter("@DueDate", objSales.DueDate));
                paramCollection.Add(new DBParameter("@MatRcvdType", objrcvd.Type));
                paramCollection.Add(new DBParameter("@MatRcvdParty", objrcvd.Party));
                paramCollection.Add(new DBParameter("@MatCentre", objrcvd.MatCenter));

                paramCollection.Add(new DBParameter("@Narration", objrcvd.Narration));
                paramCollection.Add(new DBParameter("@ItemTotalAmount", objrcvd.TotalAmount));
                paramCollection.Add(new DBParameter("@ItemTotalQty", objrcvd.TotalQty));
                paramCollection.Add(new DBParameter("@BSTotalAmount", objrcvd.BSTotalAmount));

                paramCollection.Add(new DBParameter("@CreatedBy", "Admin"));

                System.Data.IDataReader dr =
                    _dbHelper.ExecuteDataReader("spInsertMatRcvddMaster", _dbHelper.GetConnObject(), paramCollection, System.Data.CommandType.StoredProcedure);

                int id = 0;
                dr.Read();
                id = Convert.ToInt32(dr[0]);
                SaveMatRcvdItems(objrcvd.RcvdItem_Voucher, id);
                SaveMatRcvdBillSundry(objrcvd.RcvdBillSundry_Voucher, id);
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw ex;
            }

            return isSaved;
        }
        
        public bool SaveMatRcvdItems(List<Item_VoucherModel> lstItems,int ParentId)
        {
            string Query = string.Empty;
            bool isSaved = true;
            foreach (Item_VoucherModel item in lstItems)
            {
                item.ParentId = ParentId;

                try
                {
                    DBParameterCollection paramCollection = new DBParameterCollection();

                    paramCollection.Add(new DBParameter("@MatRcvdId", item.ParentId));
                    paramCollection.Add(new DBParameter("@Batch", item.Batch));
                    paramCollection.Add(new DBParameter("@Item", item.Item));
                    paramCollection.Add(new DBParameter("@Qty", item.Qty));
                    paramCollection.Add(new DBParameter("@Unit", item.Unit));
                    paramCollection.Add(new DBParameter("@Price", item.Price));
                    paramCollection.Add(new DBParameter("@Amount", item.Amount));
                    //paramCollection.Add(new DBParameter("@TotalQty", item.TotalQty));
                    //paramCollection.Add(new DBParameter("@TotalAmount", item.TotalAmount));

                    paramCollection.Add(new DBParameter("@CreatedBy", "Admin"));

                    System.Data.IDataReader dr =
                    _dbHelper.ExecuteDataReader("spInsertMatReceviedItem", _dbHelper.GetConnObject(), paramCollection, System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    isSaved = false;
                    throw ex;
                }
            }
            return isSaved;
        }

        public bool SaveMatRcvdBillSundry(List<BillSundry_VoucherModel> lstBS,int ParentId)
        {
            string Query = string.Empty;
            bool isSaved = true;

            foreach (BillSundry_VoucherModel bs in lstBS)
            {
                bs.ParentId = ParentId;

                try
                {
                    DBParameterCollection paramCollection = new DBParameterCollection();

                    paramCollection.Add(new DBParameter("@MatRcvdId", bs.ParentId));
                    paramCollection.Add(new DBParameter("@BillSundry", bs.BillSundry));
                    paramCollection.Add(new DBParameter("@Narration", bs.Narration));
                    paramCollection.Add(new DBParameter("@Percentage", bs.Percentage));
                    paramCollection.Add(new DBParameter("@Amount", bs.Amount));
                    paramCollection.Add(new DBParameter("@TotalAmount", bs.TotalAmount));
                    paramCollection.Add(new DBParameter("@CreatedBy", "Admin"));

                    System.Data.IDataReader dr =
                    _dbHelper.ExecuteDataReader("spInsertMatReceviedBS", _dbHelper.GetConnObject(), paramCollection, System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    isSaved = false;
                    throw ex;
                }
            }
            return isSaved;
        }

        public int GetMatRecvdId()
        {
            string Query = "SELECT MAX(MatRcvd_Id) FROM Trans_MatRcvd_Voucher";

            int id = Convert.ToInt32(_dbHelper.ExecuteScalar(Query));

            return id;
        }


        #endregion

        #region UPDATE MatRcvd VOUCHER

        public bool UpdateMatIssueVoucher(MatRcvdModel objMatRcvd)
        {
            string Query = string.Empty;
            bool isUpdated = true;

            try
            {
                DBParameterCollection paramCollection = new DBParameterCollection();

                
                paramCollection.Add(new DBParameter("@Series", objMatRcvd.Series));
                paramCollection.Add(new DBParameter("@Date", objMatRcvd.Rcvd_Date));
                paramCollection.Add(new DBParameter("@VoucherNumber", objMatRcvd.Voucher_Number));                                              
                paramCollection.Add(new DBParameter("@Type", objMatRcvd.Type));
                paramCollection.Add(new DBParameter("@Party", objMatRcvd.Party));
                paramCollection.Add(new DBParameter("@MatCentre", objMatRcvd.MatCenter));
                paramCollection.Add(new DBParameter("@Narration", objMatRcvd.Narration));
                paramCollection.Add(new DBParameter("@qty", objMatRcvd.TotalQty));
                paramCollection.Add(new DBParameter("@amount", objMatRcvd.TotalAmount));
                paramCollection.Add(new DBParameter("@bsamount", objMatRcvd.BSTotalAmount));
                
                paramCollection.Add(new DBParameter("@ModifiedBy", "Admin"));
                paramCollection.Add(new DBParameter("@id", objMatRcvd.Rcvd_Id));

                Query = "UPDATE Trans_MatRcvd_Voucher SET [Series]=@Series,[Rcvd_Date]=@Date," +
                         "[VoucherNo]=@VoucherNumber,[Type]=@Type," +
                        "[Party]=@Party,[MatCentre]=@MatCentre," +
                        "[Narration]=@Narration,[TotalQty]=@qty,[TotalAmount]=@amount,[BSTotalAmount]=@bsamount,[ModifiedBy]=@ModifiedBy " +
                        "WHERE [MatRcvd_Id]=@id ";

                if (_dbHelper.ExecuteNonQuery(Query, paramCollection) > 0)
                {
                    UpdateItemandBS(objMatRcvd);
                    isUpdated = true;
                }
                    
            }
            catch (Exception ex)
            {
                isUpdated = false;
                throw ex;
            }

            return isUpdated;
        }

        private bool UpdateItemandBS(MatRcvdModel objmat)
        {
            try
            {
                //UPDATE Item voucher -CHILD TABLE UPDATES
                foreach (Item_VoucherModel item in objmat.RcvdItem_Voucher)
                {
                    if (item.Item_ID > 0)
                    {
                        UpdateMatRcvdItems(item);

                    }
                    else
                    {
                        SaveMatRcvdPartyItems(item);
                    }
                }

                //Update Bill Sundry Items
                foreach (BillSundry_VoucherModel bs in objmat.RcvdBillSundry_Voucher)
                {
                    if (bs.BSId > 0)
                    {
                        SaveMatRcvdPartyItems(bs);

                    }
                    else
                    {
                        SaveMatRcvdPartyBillSundryVoucher(bs);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        

        public bool UpdateMatRcvdItems(Item_VoucherModel objItem)
        {
            string Query = string.Empty;
            bool isUpdated = true;
          
            try
            {
                DBParameterCollection paramCollection = new DBParameterCollection();

                
                paramCollection.Add(new DBParameter("@Item", objItem.Item));
                paramCollection.Add(new DBParameter("@Batch", objItem.Batch));
                paramCollection.Add(new DBParameter("@Qty", objItem.Qty));
                paramCollection.Add(new DBParameter("@Unit", objItem.Unit));
                paramCollection.Add(new DBParameter("@Price", objItem.Price, System.Data.DbType.Decimal));
                paramCollection.Add(new DBParameter("@Amount", objItem.Amount, System.Data.DbType.Decimal));
                paramCollection.Add(new DBParameter("@TotalQty", objItem.TotalQty, System.Data.DbType.Decimal));
                paramCollection.Add(new DBParameter("@TotalAmount", objItem.TotalAmount, System.Data.DbType.Decimal));
                paramCollection.Add(new DBParameter("@ModifiedBy", "Admin"));
                paramCollection.Add(new DBParameter("@ModifiedDate", DateTime.Now));
                paramCollection.Add(new DBParameter("@Trans_ID", objItem.ParentId));
                paramCollection.Add(new DBParameter("@ItemId", objItem.Item_ID));
                //paramCollection.Add(new DBParameter("@ModifiedBy", objSalesItem.ModifiedBy));
                

                Query = "UPDATE Trans_MatRcvd_Items SET [Item]=@Item,[Batch]=@Batch,[Qty]=@Qty,[Unit]=@Unit," +
                "[Price]=@Price,[Amount]=@Amount,[TotalQty]=@TotalQty,[TotalAmount]=@TotalAmount,[ModifiedBy]=@ModifiedBy,[ModifiedDate]=@ModifiedDate " +
                "WHERE MatRcvd_Id=@Trans_ID AND [ItemId]=@ItemId";


                if (_dbHelper.ExecuteNonQuery(Query, paramCollection) > 0)
                    isUpdated = true;
            }
            catch (Exception ex)
            {
                isUpdated = false;
                throw ex;
            }

            return isUpdated;
        }

        public bool SaveMatRcvdPartyItems(BillSundry_VoucherModel objBSVoucher)
        {
            string Query = string.Empty;
            bool isUpdate = true;

            try
            {
                DBParameterCollection paramCollection = new DBParameterCollection();

                
                paramCollection.Add(new DBParameter("@BillSundry_Name", objBSVoucher.BillSundry));
                paramCollection.Add(new DBParameter("@BillSundry_Amount", objBSVoucher.Amount));
                paramCollection.Add(new DBParameter("@Percentage", objBSVoucher.Percentage));
                paramCollection.Add(new DBParameter("@TotalAmount", objBSVoucher.TotalAmount));
                paramCollection.Add(new DBParameter("@ModifiedBy", objBSVoucher.ModifiedBy));
                paramCollection.Add(new DBParameter("@ModifiedDate", DateTime.Now));
                             
                paramCollection.Add(new DBParameter("@BillSundry_ID", objBSVoucher.BSId));
                paramCollection.Add(new DBParameter("@Trans_ID", objBSVoucher.ParentId));

                Query = "UPDATE Trans_MatRcvd_BS SET [BillSundry]=@BillSundry_Name,[Amount]=@BillSundry_Amount," +
                "[Percentage]=@Percentage,[TotalAmount]=@TotalAmount,[ModifiedBy]=@ModifiedBy,[ModifiedDate]=@ModifiedDate " +
                "WHERE [BSId]=@BillSundry_ID AND [MatRcvd_Id]=@Trans_ID";

                if (_dbHelper.ExecuteNonQuery(Query, paramCollection) > 0)
                    isUpdate = true;
            }
            catch (Exception ex)
            {
                isUpdate = false;
                throw ex;
            }

            return isUpdate;
        }

        public bool SaveMatRcvdPartyItems(Item_VoucherModel item)
        {
            string Query = string.Empty;
            bool isSaved = true;

            try
            {
                DBParameterCollection paramCollection = new DBParameterCollection();

                paramCollection.Add(new DBParameter("@IssueVoucher_ID", item.ParentId));
                paramCollection.Add(new DBParameter("@Issue_Item", item.Item));
                paramCollection.Add(new DBParameter("@Issue_Batch", item.Batch));
                paramCollection.Add(new DBParameter("@Issue_Qty", item.Qty));
                paramCollection.Add(new DBParameter("@Issue_Unit", item.Unit));
                paramCollection.Add(new DBParameter("@Issue_Price", item.Price));
                paramCollection.Add(new DBParameter("@Issue_Amount", item.Amount));
                paramCollection.Add(new DBParameter("@TotalQty", item.TotalQty));
                paramCollection.Add(new DBParameter("@TotalAmount", item.TotalAmount));

                paramCollection.Add(new DBParameter("@CreatedBy", "Admin"));


                Query = "INSERT INTO Trans_MatRcvd_Items ([MatRcvd_Id],[Item],[Batch],[Qty],[Unit]," +
                "[Price],[Amount],[TotalQty],[TotalAmount],[CreatedBy]) VALUES " +
                "(@IssueVoucher_ID,@Issue_Item,@Issue_Batch,@Issue_Qty,@Issue_Unit,@Issue_Price,@Issue_Amount,@TotalQty,@TotalAmount,@CreatedBy)";

                if (_dbHelper.ExecuteNonQuery(Query, paramCollection) > 0)
                    isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw ex;
            }
            return isSaved;
        }

        public bool SaveMatRcvdPartyBillSundryVoucher(BillSundry_VoucherModel objBS)
        {
            string Query = string.Empty;
            bool isSaved = true;

            try
            {
                DBParameterCollection paramCollection = new DBParameterCollection();

                paramCollection.Add(new DBParameter("@Voucher_ID", objBS.ParentId));
                paramCollection.Add(new DBParameter("@BillSundry_Name", objBS.BillSundry));
                paramCollection.Add(new DBParameter("@BillSundry_Amount", objBS.Amount));
                paramCollection.Add(new DBParameter("@Percentage", objBS.Percentage));
                paramCollection.Add(new DBParameter("@TotalAmount", objBS.TotalAmount));
                paramCollection.Add(new DBParameter("@CreatedBy", "Admin"));

                Query = "INSERT INTO Trans_MatRcvd_BS ([MatRcvd_Id],[BillSundry],[Amount]," +
                "[Percentage],[TotalAmount],[CreatedBy]) VALUES " +
                "(@Voucher_ID,@BillSundry_Name,@BillSundry_Amount,@Percentage,@TotalAmount,@CreatedBy)";

                if (_dbHelper.ExecuteNonQuery(Query, paramCollection) > 0)
                    isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw ex;
            }
            return isSaved;
        }

        #endregion

        public List<TransListModel> GetAllMatRcvdList()
        {
            List<TransListModel> lstModel = new List<TransListModel>();
            TransListModel objList;

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT t.MatRcvd_Id, i.itemid, t.Rcvd_date, t.series, t.voucherno, t.party, i.item, i.qty, i.unit, i.price, i.amount, i.totalqty, i.totalamount FROM trans_MatRcvd_Voucher AS t ");
            sbQuery.Append("INNER JOIN trans_MatRcvd_items AS i ON t.MatRcvd_Id=i.MatRcvd_Id;");


            System.Data.IDataReader dr = _dbHelper.ExecuteDataReader(sbQuery.ToString(), _dbHelper.GetConnObject());

            while (dr.Read())
            {
                objList = new TransListModel();

                objList.MatRcvd_id = Convert.ToInt32(dr["MatRcvd_Id"]);

                objList.item_id = Convert.ToInt32(dr["itemid"]);
                objList.Rcvd_date = Convert.ToDateTime(dr["Rcvd_Date"]);
                objList.series = Convert.ToString(dr["series"]);
                objList.voucherno = Convert.ToInt32(dr["VoucherNo"]);
                objList.party = Convert.ToString(dr["party"]);
                objList.item = Convert.ToString(dr["item"]);
                objList.qty = Convert.ToInt32(dr["qty"]);
                objList.unit = Convert.ToString(dr["unit"]);
                objList.price = Convert.ToInt32(dr["price"]);
                objList.amount = Convert.ToInt32(dr["amount"]);
                objList.amount = Convert.ToInt32(dr["amount"]);
                objList.totalqty = Convert.ToInt32((dr["totalqty"]));
                objList.totalamount = Convert.ToInt32((dr["totalamount"]));
                lstModel.Add(objList);

            }
            return lstModel;
        }

        public MatRcvdModel GetAllMatRcvdbyId(int id)
        {
            MatRcvdModel objMatRcvd = new MatRcvdModel();

            string Query = "SELECT * FROM Trans_MatRcvd_Voucher WHERE MatRcvd_Id=" + id;
            System.Data.IDataReader dr = _dbHelper.ExecuteDataReader(Query, _dbHelper.GetConnObject());

            while (dr.Read())
            {

                objMatRcvd.Rcvd_Id = Convert.ToInt32(dr["Matrcvd_Id"]);
                objMatRcvd.Series = dr["series"].ToString();

                objMatRcvd.Rcvd_Date = DataFormat.GetDateTime(dr["Rcvd_Date"]);
                objMatRcvd.Voucher_Number = DataFormat.GetInteger(dr["VoucherNo"]);
                objMatRcvd.Type = dr["Type"].ToString();
                objMatRcvd.Party = dr["party"].ToString();
                objMatRcvd.MatCenter = dr["MatCentre"].ToString();
                objMatRcvd.Narration = dr["Narration"].ToString();
                objMatRcvd.TotalQty = Convert.ToDecimal(dr["TotalQty"]);
                objMatRcvd.TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString());
                objMatRcvd.BSTotalAmount = Convert.ToDecimal(dr["BSTotalAmount"]);

                //SELECT Credit Note Accounts

                string itemQuery = "SELECT * FROM Trans_MatRcvd_Items WHERE MatRcvd_Id=" + id;
                System.Data.IDataReader drItems = _dbHelper.ExecuteDataReader(itemQuery, _dbHelper.GetConnObject());

                objMatRcvd.RcvdItem_Voucher = new List<Item_VoucherModel>();
                Item_VoucherModel objitem;

                while (drItems.Read())
                {
                    objitem = new Item_VoucherModel();

                    objitem.Item_ID = Convert.ToInt32(drItems["ItemId"]);
                    objitem.ParentId = DataFormat.GetInteger(drItems["MatRcvd_Id"]);
                    objitem.Item = drItems["item"].ToString();
                    objitem.Batch = drItems["Batch"].ToString();
                    objitem.Qty = Convert.ToInt32(drItems["qty"].ToString());
                    objitem.Unit = (drItems["unit"].ToString());
                    objitem.Price = Convert.ToDecimal(drItems["price"]);
                    objitem.Amount = Convert.ToInt32(drItems["amount"].ToString());
                    objitem.TotalAmount = Convert.ToDecimal(drItems["TotalAmount"]);
                    objitem.TotalQty = Convert.ToInt32(drItems["TotalQty"].ToString());

                    objMatRcvd.RcvdItem_Voucher.Add(objitem);

                }

                string BSQuery = "SELECT * FROM Trans_MatRcvd_BS WHERE MatRcvd_Id=" + id;
                System.Data.IDataReader drbs = _dbHelper.ExecuteDataReader(BSQuery, _dbHelper.GetConnObject());

                objMatRcvd.RcvdBillSundry_Voucher = new List<BillSundry_VoucherModel>();
                BillSundry_VoucherModel objbs;

                while (drbs.Read())
                {
                    objbs = new BillSundry_VoucherModel();

                    objbs.BSId = Convert.ToInt32(drbs["BSId"]);
                    objbs.ParentId = DataFormat.GetInteger(drbs["MatRcvd_Id"]);
                    objbs.BillSundry = drbs["BillSundry"].ToString();
                    objbs.Percentage = Convert.ToDecimal(drbs["Percentage"].ToString());
                    objbs.Amount = Convert.ToDecimal((drbs["Amount"].ToString()));
                    objbs.TotalAmount = Convert.ToDecimal(drbs["TotalAmount"].ToString());

                    objMatRcvd.RcvdBillSundry_Voucher.Add(objbs);

                }

            }
            return objMatRcvd;
        }

        public bool DeleteMatRcvd(int id)
        {
            bool isDelete = false;
            try
            {
                if (DeleteMatRcvdItems(id))
                {
                    if (DeleteMatRcvdBS(id))
                    {
                        string Query = "DELETE * FROM trans_MatRcvd_Voucher WHERE MatRcvd_Id=" + id;
                        int rowes = _dbHelper.ExecuteNonQuery(Query);
                        if (rowes > 0)
                            isDelete = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isDelete = false;
            }
            return isDelete;
        }

        public bool DeleteMatRcvdItems(int id)
        {
            bool isDelete = false;
            try
            {
                string Query = "DELETE * FROM Trans_MatRcvd_Items WHERE MatRcvd_Id=" + id;
                int rowes = _dbHelper.ExecuteNonQuery(Query);
                if (rowes > 0)
                    isDelete = true;
            }
            catch (Exception ex)
            {
                isDelete = false;
            }
            return isDelete;
        }
        public bool DeleteMatRcvdBS(int id)
        {

            bool isDelete = false;
            try
            {
                string Query = "DELETE * FROM Trans_MatRcvd_BS WHERE MatRcvd_Id=" + id;
                int rowes = _dbHelper.ExecuteNonQuery(Query);
                if (rowes > 0)
                    isDelete = true;
            }
            catch (Exception ex)
            {
                isDelete = false;
            }
            return isDelete;
        }
    }
}
