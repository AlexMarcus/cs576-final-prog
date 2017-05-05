import java.io.*;
import java.net.*;

public class udpClient{
	public static void main(String[] args) throws IOException{
		byte[] sendBuf = new byte[256];
		sendBuf = "Successful ping".getBytes();
		DatagramSocket socket = new DatagramSocket();
		InetAddress address = InetAddress.getByName("149.125.39.106");
		DatagramPacket packet = new DatagramPacket(sendBuf, sendBuf.length, address, 12000);
		socket.send(packet);
	}
}
