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

namespace MixERP.Net.Entities.Models.Transactions
{
    public sealed class JournalDetail
    {
        public string Account { get; set; }
        public string AccountNumber { get; set; }
        public string CashRepositoryCode { get; set; }
        public decimal Credit { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Debit { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal LocalCurrencyCredit { get; set; }
        public decimal LocalCurrencyDebit { get; set; }
        public string StatementReference { get; set; }
    }
}