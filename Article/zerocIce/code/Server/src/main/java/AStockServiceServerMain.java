import com.zeroc.Ice.Communicator;
import com.zeroc.Ice.ObjectAdapter;
import com.zeroc.Ice.Util;


public class AStockServiceServerMain {
    public static void main(String[] args) {
        try (Communicator communicator = Util.initialize()) {//创建communicator
            ObjectAdapter oa = communicator.createObjectAdapterWithEndpoints("AStockServiceAdapter", "default -p 10000");//创建一个Adatper，Id是AStockServiceAdapter，绑定到10000端口

            AStockServiceServer servant = new AStockServiceServer();//我们的服务
            oa.add(servant, Util.stringToIdentity("AStockService"));//把我们创建的服务加到上面创建的adapter里
            oa.activate();//激活adapter
            System.out.println("AStock Service Server is running");//输出启动log
            communicator.waitForShutdown();//等待结束
        }
    }
}
