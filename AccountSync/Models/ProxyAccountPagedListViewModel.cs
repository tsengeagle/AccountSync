using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountSync.Models
{
    public class ProxyAccountPagedListViewModel
    {
        public ProxyAccountSearchViewModel SearchPatameter { get; set; }
        public IPagedList<ProxyAccountViewModel> ProxyAccounts { get; set; }
        public SelectList DeptNames { get; set; }
        public int PageIndex { get; set; }
    }
}