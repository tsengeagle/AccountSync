using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountSync.Models
{
    public class AccountPagedListViewModel
    {
        public AccountParameterViewModel Parameter { get; set; }
        public IPagedList<DB_GEN.GenProxyAccount> Accounts { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public SelectList Depts { get; set; }

    }
}