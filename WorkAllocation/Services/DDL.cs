using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WorkAllocation.Models;

namespace WorkAllocation.Services
{
    public class DDL
    {
         
        public static List<VM_Domain> DomainList()
        {
            MyDatabaseClass objdb = new MyDatabaseClass();
            bool hasExceptionThrown = false;
            string errorMessage = "";
            List<VM_Domain> Lst = new List<VM_Domain>();
            DataSet ds = new DataSet();
            ds = objdb.GetDataSet("Select * from Domain_mast where status='A'", ref hasExceptionThrown, ref errorMessage);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    VM_Domain vm = new VM_Domain();
                    vm.DOMAIN = ds.Tables[0].Rows[i]["DOMAIN"].ToString();
                    vm.DOMAIN_CODE = ds.Tables[0].Rows[i]["DOMAIN_CODE"].ToString();
                    Lst.Add(vm);
                }
            }
            return Lst;
        }
        public static List<VM_Porject> ProjectName()
        {
            MyDatabaseClass objdb = new MyDatabaseClass();
            bool hasExceptionThrown = false;
            string errorMessage = "";
            List<VM_Porject> Lst = new List<VM_Porject>();
            DataSet ds = new DataSet();
            ds = objdb.GetDataSet("Select * from project_mast where status='A'", ref hasExceptionThrown, ref errorMessage);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    VM_Porject vm = new VM_Porject();
                    vm.PROJECT_NAME = ds.Tables[0].Rows[i]["PROJECT_NAME"].ToString();
                    vm.PROJECT_CODE = ds.Tables[0].Rows[i]["PROJECT_CODE"].ToString();
                    Lst.Add(vm);
                }
            }
            return Lst;
        }
    }
}