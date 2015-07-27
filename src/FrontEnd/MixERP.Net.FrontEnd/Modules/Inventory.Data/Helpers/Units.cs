﻿/********************************************************************************
Copyright (C) MixERP Inc. (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, version 2 of the License.


MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using MixERP.Net.DbFactory;
using Npgsql;
using System.Data;

namespace MixERP.Net.Core.Modules.Inventory.Data.Helpers
{
    public static class Units
    {
        public static DataTable GetUnitViewByItemCode(string catalog, string itemCode)
        {
            const string sql = "SELECT * FROM core.get_associated_units_from_item_code(@ItemCode) ORDER BY unit_id;";
            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@ItemCode", itemCode);

                return DbOperation.GetDataTable(catalog, command);
            }
        }

        public static bool UnitExistsByName(string catalog, string unitName)
        {
            const string sql = "SELECT 1 FROM core.units WHERE core.units.unit_name=@UnitName;";
            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@UnitName", unitName);

                var value = DbOperation.GetScalarValue(catalog, command);
                if (value != null)
                {
                    return value.ToString().Equals("1");
                }
            }
            return false;
        }
    }
}