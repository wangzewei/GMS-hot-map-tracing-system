package com.iot.ishop;

import java.util.List;
import android.net.wifi.ScanResult;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.Toast;

public class MainActivity extends Activity 
{
	@SuppressLint("HandlerLeak") final Handler mHandler= new Handler()
	{
		@Override
		public void handleMessage(Message msg)
		{
			Toast.makeText(getBaseContext(), (msg.obj).toString(), Toast.LENGTH_SHORT).show();
		}
	};
		
	ConnectionHandler mConnectionHandler =new ConnectionHandler()
	{		
		@Override
		public void didReceiveData(String data) 
		{
			// TODO Auto-generated method stub
			Message m1 = new Message();
			m1.what = 2;
			m1.obj=(Object)("Echo"+data);
			mHandler.sendMessage(m1);
			//Toast.makeText(current, data, Toast.LENGTH_SHORT).show();
		}
		
		@Override
		public void didDisconnect(Exception error)
		{
			// TODO Auto-generated method stub
			Message m1 = new Message();
			m1.what = 0;
			m1.obj=(Object)"Disconnect";
			mHandler.sendMessage(m1);
			
		}
		
		@Override
		public void didConnect()
		{
			// TODO Auto-generated method stub
			Message m1 = new Message();
			m1.what = 1;
			m1.obj=(Object)"Connect";
			mHandler.sendMessage(m1);
		}
	};
	
	final AsyncConnection aConnection = new AsyncConnection("172.17.133.10",8737,mConnectionHandler);
	
	Button button1,button2,button3;
	WifiManager _wManager;
	WifiInfo _wInfo;
	List<ScanResult> los;
	String textString = Context.WIFI_SERVICE;
	
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        aConnection.execute();
        button1 = (Button) findViewById(R.id.button1);
        button2 = (Button) findViewById(R.id.button2);  
        button1.setOnClickListener(new OnClickListener() 
        {
			@Override
			public void onClick(View arg0) 
			{
				reportAction(getwifistate());
			}
	                
		});
        button2.setOnClickListener(new OnClickListener() 
        {			
			@Override
			public void onClick(View arg0) 
			{
				getAction();
			}
			               
		});
        
    }
    
    private List<ScanResult>  getwifistate()
    {
    	_wManager=(WifiManager) getSystemService(textString);
    	_wInfo = _wManager.getConnectionInfo();
    	return _wManager.getScanResults();
    }
    
    private void reportAction(List<ScanResult> _los)
    {
    	for (ScanResult l : _los)
    	{
				aConnection.write("L"+","+_wInfo.getMacAddress()+","
				+_wInfo.getBSSID()+","+l.BSSID.toString()+","+l.level);
		}
    }
   
    private void getAction()
    {
    	aConnection.write("L"+","+_wInfo.getMacAddress()+","
				+_wInfo.getBSSID());
    }
    
    @Override
    public boolean onCreateOptionsMenu(Menu menu) 
    {
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    
}

/*

 */