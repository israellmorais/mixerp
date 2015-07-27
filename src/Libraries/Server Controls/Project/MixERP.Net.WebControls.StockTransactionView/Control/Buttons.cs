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

using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace MixERP.Net.WebControls.StockTransactionViewFactory
{
    public partial class StockTransactionView
    {
        private void CreateButtons(Control container)
        {
            using (HtmlGenericControl buttons = new HtmlGenericControl("div"))
            {
                buttons.Attributes.Add("class", "basic ui buttons");

                this.CreateAddButton(buttons);
                this.CreateMergeToOrderButton(buttons);
                this.CreateMergeToDeliveryButton(buttons);
                this.CreateMergeToGRNButton(buttons);
                this.CreateReturnButton(buttons);
                this.CreateFlagButton(buttons);
                this.CreatePrintButton(buttons);

                container.Controls.Add(buttons);
            }
        }
    }
}