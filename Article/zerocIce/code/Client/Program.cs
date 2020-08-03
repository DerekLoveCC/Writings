using com.astock;
using Ice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var communicator = Util.initialize(ref args))//创建Communicator对象
            {
                ObjectPrx basePrx = communicator.stringToProxy("AStockService:default -p 10000");//创建客户端基类代理

                AStockServicePrx aStockServicePrx = AStockServicePrxHelper.checkedCast(basePrx);//把基类代理转换为子类代理
                var companyInfo = aStockServicePrx.GetCompanyInfo(1000);//调用GetCompanyInfo方法

                Console.WriteLine($"id:{companyInfo.id} name:{companyInfo.name} addr:{companyInfo.addr}");//输出返回结果
            }

            Console.Read();
        }
    }
}
