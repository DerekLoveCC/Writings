module com
{
    module astock
    {
        class CompanyInfo
        {
            int id;
            string name;
            string addr;
        }
    }
}
module com
{
    module astock
    {
        interface AStockService
        {
            CompanyInfo GetCompanyInfo(int id);
        }
    }
}