module astock
{
    class CompanyInfo
    {
        int id;
        string name;
        string addr;
    }
}
module astock
{
    class AStockService
    {
        CompanyInfo GetCompanyInfo(int id);
    }
}