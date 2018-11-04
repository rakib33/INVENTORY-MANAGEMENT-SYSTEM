    <headrunat="server">  
    <metaname="viewport"content="width=device-width"/>  
    <title> Data Report With MVC 4</title>  
      
       <scriptrunat="server">  
      
          void Page_Load(object sender, EventArgs e)  
          {  
      
             if (!IsPostBack)  
             {  
      
                List<ReportViewerMVC.Customers> customers = null;  
      
                using (ReportViewerMVC.EntityFrameworkTestEntities _entities = new ReportViewerMVC.EntityFrameworkTestEntities())  
                {  
      
                   customers = _entities.Customers.ToList();  
      
                   ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/MyReport.rdlc");  
      
      
                   ReportDataSource RDS = newReportDataSource("DataSet1", customers);  
      
                   ReportViewer1.LocalReport.DataSources.Add(RDS);  
      
                   ReportViewer1.LocalReport.Refresh();  
      
                }  
      
             }  
          }  
      
      
       </script>  
      
    </head>  