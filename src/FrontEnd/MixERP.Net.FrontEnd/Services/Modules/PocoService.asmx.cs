﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using MixERP.Net.Entities.Core;
using MixERP.Net.EntityParser;
using MixERP.Net.Framework.Extensions;
using MixERP.Net.FrontEnd.Base;
using Newtonsoft.Json;
using PetaPoco;

namespace MixERP.Net.FrontEnd.Services.Modules
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class PocoService : MixERPWebService
    {
        private const int PAGE_SIZE = 50;
        private const bool SHOW_ALL = false;

        [WebMethod]
        public string GetPocoView(string pocoName, long pageNumber, string filterName, bool byOffice)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);
            List<Filter> filters = Service.GetFilters(this.Catalog, poco, filterName);

            IEnumerable<object> result = Service.GetView(this.Catalog, poco, pageNumber, filters, byOffice, this.OfficeId, SHOW_ALL, PAGE_SIZE);

            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public long GetTotalPages(string pocoName, string filterName, bool byOffice)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);
            List<Filter> filters = Service.GetFilters(this.Catalog, poco, filterName);

            long result = Service.GetTotalPages(this.Catalog, poco, filters, byOffice, this.OfficeId, SHOW_ALL, PAGE_SIZE);
            return result;
        }

        [WebMethod]
        public string GetFilteredPocoView(string pocoName, long pageNumber, List<Filter> filters, bool byOffice)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);

            IEnumerable<object> result = Service.GetView(this.Catalog, poco, pageNumber, filters, byOffice, this.OfficeId, SHOW_ALL, PAGE_SIZE);

            return JsonConvert.SerializeObject(result);
        }

        [WebMethod]
        public long GetFilteredTotalPages(string pocoName, List<Filter> filters, bool byOffice)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);

            long result = Service.GetTotalPages(this.Catalog, poco, filters, byOffice, this.OfficeId, SHOW_ALL, PAGE_SIZE);
            return result;
        }

        [WebMethod]
        public List<Filter> GetFilters(string pocoName, string filterName)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);
            return Service.GetFilters(this.Catalog, poco, filterName);
        }

        [WebMethod]
        public void SaveFilter(string pocoName, List<Filter> filters)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);
            Service.SaveFilter(this.Catalog, poco, filters);
        }

        [WebMethod]
        public dynamic GetFilterNames(string pocoName)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);
            return Service.GetFilterNames(this.Catalog, poco);
        }

        [WebMethod]
        public void MakeDefaultFilter(string pocoName, string filterName)
        {
            object poco = PocoHelper.GetInstanceOf(pocoName);
            Service.MakeDefaultFilter(this.Catalog, poco, filterName);
        }

        [WebMethod]
        public EntityView GetFormView(string pocoName)
        {
            EntityView scrud = new EntityView();
            List<EntityColumn> columns = new List<EntityColumn>();

            object poco = PocoHelper.GetInstanceOf(pocoName);

            PrimaryKeyAttribute primaryKey =
                poco.GetType().GetAttributeValue((PrimaryKeyAttribute attribute) => attribute);
            scrud.PrimaryKey = primaryKey.Value;

            Type type = poco.GetType();
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (new[] {"AuditUserId", "AuditTs"}.Contains(info.Name))
                {
                    continue;
                }

                EntityColumn column = new EntityColumn();

                column.PropertyName = info.Name;
                column.DataType = info.PropertyType.ToString();

                if (column.DataType.StartsWith("System.Nullable`1["))
                {
                    column.IsNullable = true;
                    column.DataType = column.DataType.Replace("System.Nullable`1[", "").Replace("]", "");
                }

                column.ColumnName =
                    info.GetCustomAttributes(typeof (ColumnAttribute), false)
                        .Cast<ColumnAttribute>()
                        .Select(c => c.Name)
                        .FirstOrDefault();

                ColumnDbType dbType =
                    info.GetCustomAttributes(typeof (ColumnDbType), false).Cast<ColumnDbType>().FirstOrDefault();

                if (dbType != null)
                {
                    column.DbDataType = dbType.Name;
                    column.Value = dbType.DefaultValue;
                    column.MaxLength = dbType.MaxLength;

                    if (column.Value.StartsWith("nextval"))
                    {
                        column.Value = "";
                    }
                }

                if (column.ColumnName != null)
                {
                    column.IsPrimaryKey =
                        column.ColumnName.ToUpperInvariant().Equals(scrud.PrimaryKey.ToUpperInvariant());
                }

                if (column.IsPrimaryKey)
                {
                    column.IsSerial = primaryKey.autoIncrement;
                }

                columns.Add(column);
            }

            scrud.Columns = columns;
            return scrud;
        }

    }
}