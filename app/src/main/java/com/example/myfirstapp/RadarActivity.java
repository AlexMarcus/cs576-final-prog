package com.example.myfirstapp;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.view.ViewGroup.LayoutParams;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;


public class RadarActivity extends AppCompatActivity {

    private Radar pc;
    private PopupWindow mPopup;
    private LinearLayout lin;

    private EditText IP;

    private long lastSend;

    private DatagramSocket socket;
    private InetAddress address;
    private String addr;

    private String lines;

    private int connected;



    @Override
    protected void onCreate(Bundle savedInstanceState){
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_radar);
        pc = (Radar) findViewById(R.id.radar);
        lin = (LinearLayout) findViewById(R.id.linear);
        pc.setradarListener(new Radar.radarListener() {

            @Override
            public void onRadarTouch(float val){
                //System.out.println("in activity");
                //System.out.println(val);
                Button send = (Button) findViewById(R.id.sendEnemies);
                send.setVisibility(View.VISIBLE);
                if(val == -1){
                    LayoutInflater inflater = (LayoutInflater) getApplicationContext().getSystemService(LAYOUT_INFLATER_SERVICE);
                    View customView = inflater.inflate(R.layout.popup,null);

                    mPopup = new PopupWindow(
                            customView,
                            LayoutParams.WRAP_CONTENT,
                            LayoutParams.WRAP_CONTENT
                    );

                    ImageButton closeButton = (ImageButton) customView.findViewById(R.id.ib_close);
                    closeButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View view) {
                            // Dismiss the popup window
                            mPopup.dismiss();
                        }
                    });

                    mPopup.showAtLocation(lin, Gravity.CENTER,0,0);
                }
                else if(val == -2){
                    LayoutInflater inflater = (LayoutInflater) getApplicationContext().getSystemService(LAYOUT_INFLATER_SERVICE);
                    View customView = inflater.inflate(R.layout.popup_err,null);

                    mPopup = new PopupWindow(
                            customView,
                            LayoutParams.WRAP_CONTENT,
                            LayoutParams.WRAP_CONTENT
                    );

                    ImageButton closeButton = (ImageButton) customView.findViewById(R.id.ib_close2);
                    closeButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View view) {
                            // Dismiss the popup window
                            mPopup.dismiss();
                        }
                    });

                    mPopup.showAtLocation(lin, Gravity.CENTER,0,0);
                }

            }

        });

        lastSend = System.currentTimeMillis();

    }

    public void connectSocket(View view) throws IOException{

            IP = (EditText) findViewById(R.id.ip);
            addr = IP.getText().toString();
            new Thread() {
                public void run() {
                    try {
                        socket = new DatagramSocket();
                        System.out.println(addr);
                        address = InetAddress.getByName(addr);
                        byte[] sendBuf = "connected".getBytes();

                        DatagramPacket packet = new DatagramPacket(sendBuf, sendBuf.length, address, 12000);
                        socket.send(packet);
                        connected = 1;
                    } catch (IOException e) {
                        System.out.println(e);
                    }

                }
            }.start();

    }

    public void sendEnemies(View view) throws IOException{
        long curTime = System.currentTimeMillis();
        lines = pc.getLines();


        if(curTime - lastSend > pc.getNumEnemies()*1000) {
            lastSend = curTime;


            pc.clearRadar();

            new Thread() {
                public void run() {

                    try {
                        byte[] sendBuf = lines.getBytes();
                        DatagramPacket packet = new DatagramPacket(sendBuf, sendBuf.length, address, 12000);
                        socket.send(packet);
                    } catch (Exception e) {
                        System.out.println(e);
                    }
                }
            }.start();
        }

    }

    public void disconnectSocket(View view) throws IOException{
        if(connected == 1) {
            IP = (EditText) findViewById(R.id.ip);
            addr = IP.getText().toString();
            new Thread() {
                public void run() {
                    try {

                        address = InetAddress.getByName(addr);
                        byte[] sendBuf = "disconnected".getBytes();

                        DatagramPacket packet = new DatagramPacket(sendBuf, sendBuf.length, address, 12000);
                        socket.send(packet);
                    } catch (IOException e) {
                        System.out.println(e);
                    }
                }
            }.start();
        }

    }


}
