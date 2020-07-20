using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WorkAllocation.Models;

namespace WorkAllocation.Services
{
    public class ProjectService
    {
        public bool INS_UPD_PROJECT(VM_Porject vm)
        {
            MyDatabaseClass objdb = new MyDatabaseClass();
            bool hasExceptionThrown = false;
            string errorMessage = "";
            SortedList sl = new SortedList();
            int counter = 0;
            try
            {
                if (vm.PROJECT_UID == 0)
                {
                    //string maxid = objdb.GetSingleValue("select max(PROJECT_UID) from project_mast", ref hasExceptionThrown, ref errorMessage);
                    //if (maxid == "") maxid = "0";
                    //maxid = (Convert.ToInt32(maxid) + 1).ToString();
                    string sql = "insert into project_mast(PROJECT_CODE,PROJECT_NAME,DOMAIN,START_DATE,END_DATE,STATUS,ENTRY_DATE) values ('" + vm.PROJECT_CODE + "','" + vm.PROJECT_NAME + "','" + vm.DOMAIN + "',convert(date,'" + vm.START_DATE + "',103),convert(date,'" + vm.END_DATE + "',103),'A',GETDATE())";
                    sl.Add(counter, sql); counter++;
                    // string[] emps = vm.EMP_CODE.ToString().Split(',');
                    for (int i = 0; i < vm.EMP_CODE.Length; i++)
                    {
                        sql = "insert into project_team (PROJECT_CODE,EMP_CODE,EMP_TYPE,STATUS,ENTRY_DATE,ENTRY_BY) values ('" + vm.PROJECT_CODE + "','" + vm.EMP_CODE[i].ToString() + "','PMPL','A',GETDATE(),00584)";
                        sl.Add(counter, sql); counter++;

                    }
                    objdb.GetRowsAffected(sl, ref hasExceptionThrown, ref errorMessage);
                }
                else
                {
                    string sql = "update project_mast set PROJECT_CODE='" + vm.PROJECT_CODE + "',PROJECT_NAME='" + vm.PROJECT_NAME + "',DOMAIN='" + vm.DOMAIN + "',START_DATE=convert(date,'" + vm.START_DATE + "',103),END_DATE=convert(date,'" + vm.END_DATE + "',103) where project_uid=" + vm.PROJECT_UID + "";
                    sl.Add(counter, sql); counter++;
                    sql = "delete from project_details where PROJECT_UID=" + vm.PROJECT_UID + "";
                    sl.Add(counter, sql); counter++;
                    //string[] emps = vm.EMP_CODE.ToString().Split(',');
                    for (int i = 0; i < vm.EMP_CODE.Length; i++)
                    {
                        sql = "insert into project_details(PROJECT_CODE,EMP_CODE,EMP_TYPE,STATUS,ENTRY_DATE,ENTRY_BY) values ('" + vm.PROJECT_CODE + "','" + vm.EMP_CODE[i].ToString() + "','PMPL','A',GETDATE(),00584)";
                        sl.Add(counter, sql); counter++;
                    }
                    objdb.GetRowsAffected(sl, ref hasExceptionThrown, ref errorMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public bool DELETE_PROJECT(string PROJECT_UID)
        {
            MyDatabaseClass objdb = new MyDatabaseClass();
            bool hasExceptionThrown = false;
            string errorMessage = "";
            SortedList sl = new SortedList();
            int counter = 0;
            try
            {
                string sql = "update project_mast set status='D' where PROJECT_CODE='" + PROJECT_UID + "'";
                sl.Add(counter, sql); counter++;

                sql = "update project_team set  STATUS='D' where PROJECT_CODE='" + PROJECT_UID + "'";
                sl.Add(counter, sql); counter++;

                objdb.GetRowsAffected(sl, ref hasExceptionThrown, ref errorMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public List<VM_Porject> Project_Gridlist(VM_Porject Request)
        {
            MyDatabaseClass objdb = new MyDatabaseClass();
            bool hasExceptionThrown = false;
            string errorMessage = "";
            List<VM_Porject> Fill_list = new List<VM_Porject>();
            try
            {
                string sqlquery = "";
                if (Request.PROJECT_UID == 0)
                {
                    sqlquery = @"select * from project_mast where status='A'";
                }
                else if (Request.PROJECT_UID > 0)
                {
                    sqlquery = @"select * from project_mast where PROJECT_UID= '" + Request.PROJECT_UID + "'";
                }
                DataSet ds = objdb.GetDataSet(sqlquery, ref hasExceptionThrown, ref errorMessage);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    VM_Porject vm = new VM_Porject();
                    vm.PROJECT_UID = Convert.ToInt32(ds.Tables[0].Rows[i]["PROJECT_UID"].ToString());
                    vm.PROJECT_CODE = ds.Tables[0].Rows[i]["PROJECT_CODE"].ToString();
                    vm.PROJECT_NAME = ds.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                    vm.DOMAIN = ds.Tables[0].Rows[i]["DOMAIN"].ToString();
                    vm.START_DATE = ds.Tables[0].Rows[i]["START_DATE"].ToString();
                    vm.END_DATE = ds.Tables[0].Rows[i]["END_DATE"].ToString();

                    string emp_code = "";
                    DataSet dsemp = objdb.GetDataSet("Select emp_code from project_team where project_code='" + ds.Tables[0].Rows[i]["PROJECT_CODE"].ToString() + "'", ref hasExceptionThrown, ref errorMessage);
                    for (int j = 0; j < dsemp.Tables[0].Rows.Count; j++)
                    {
                        emp_code += dsemp.Tables[0].Rows[j]["emp_code"].ToString() + ",";
                    }
                    vm.EMP_CODE = emp_code.Split(',');

                    Fill_list.Add(vm);
                }
                // return Fill_list;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
            return Fill_list;
        }
    }
}