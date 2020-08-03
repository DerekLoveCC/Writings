import com.astock.AStockService;
import com.astock.CompanyInfo;
import com.zeroc.Ice.Current;

public class AStockServiceServer implements AStockService {
    @Override
    public CompanyInfo GetCompanyInfo(int id, Current current) {
        CompanyInfo info = new CompanyInfo();
        info.id = 1234;
        info.name = "中国平安";
        info.addr = "深圳";
        return info;
    }
}
