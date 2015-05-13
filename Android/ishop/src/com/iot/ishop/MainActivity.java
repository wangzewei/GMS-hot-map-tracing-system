package com.iot.ishop;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintStream;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.List;

import android.net.wifi.ScanResult;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
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

@SuppressLint("HandlerLeak") public class MainActivity extends Activity 
{

	static Socket socket = null;
	PrintStream p = null;
	BufferedReader b = null;
	static final String IP = "172.17.133.10";
	static final int PORT = 8737;
	Button button1,button2;
	WifiManager _wManager;
	WifiInfo _wInfo;
	List<ScanResult> los;
	String textString = Context.WIFI_SERVICE;
	Handler mHandler = new Handler()
    {
    	@Override
    	public void handleMessage(Message msg)
    	{
    		Toast.makeText(getBaseContext(), (msg.obj).toString(), Toast.LENGTH_SHORT).show();
    	}
    };
	
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        button1 = (Button) findViewById(R.id.button1);
        button2 = (Button) findViewById(R.id.button2);
        button1.setOnClickListener(new OnClickListener() 
        {
			@Override
			public void onClick(View arg0) 
			{
				AsyncTask<Void, Void, String> task = new AsyncTask<Void, Void, String>() 
				{
	                @Override
	                protected String doInBackground(Void... params) 
	                {
	                    return reportAction(getwifistate());
	                }

					@Override
	                protected void onPostExecute(String result) 
	                {
	                    super.onPostExecute(result);
	                    Toast toast = Toast.makeText(getApplicationContext(), result, Toast.LENGTH_SHORT);
	                    toast.show();
	                }
	            };
	            
	            task.execute((Void[])null);
	        }
		});
        button2.setOnClickListener(new OnClickListener() 
        {			
			@Override
			public void onClick(View arg0) 
			{
				AsyncTask<Void, Void, String> task = new AsyncTask<Void, Void, String>() 
						{
			                @Override
			                protected String doInBackground(Void... params) 
			                {
			                    return getAction() ;
			                }

							@Override
			                protected void onPostExecute(String result) 
			                {
			                    super.onPostExecute(result);
			                    Toast toast = Toast.makeText(getApplicationContext(), result, Toast.LENGTH_SHORT);
			                    toast.show();
			                }
			            };
			            task.execute((Void[])null);
			}
		});
    }
    
    private List<ScanResult>  getwifistate()
    {
    	_wManager=(WifiManager) getSystemService(textString);
    	_wInfo = _wManager.getConnectionInfo();
    	return _wManager.getScanResults();
    }
    
    private String reportAction(List<ScanResult> _los)
    {
		try 
		{
			socket = new Socket(IP,PORT);
			p = new PrintStream(socket.getOutputStream(), true, "US-ASCII");
			
			for (ScanResult l : _los) 
			{
				//L means locate
				p.println("L"+","+_wInfo.getMacAddress()+","
				+_wInfo.getBSSID()+","+l.BSSID.toString()+","+l.level);
			}
		} 
		catch (UnknownHostException e) 
		{
			e.printStackTrace();
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}
    	finally
    	{
    		try 
    		{
				socket.close();
				p.close();
			} 
    		catch (IOException e) 
    		{
				e.printStackTrace();
			}
    		
    	}
    	return "Message goes";
    }
   
    private String getAction()
    {
    	String tmp = null;
		try 
		{
			socket = new Socket(IP,PORT);
			p = new PrintStream(socket.getOutputStream(), true, "ascii");
			b= new BufferedReader(new InputStreamReader(socket.getInputStream()));
			p.println("G"+","+_wInfo.getMacAddress()+","+_wInfo.getBSSID());
			while(true)
			{
				if(b.ready())
				{
					tmp = b.readLine();
					break;
				}
			}
		} 
		catch (UnknownHostException e) 
		{
			e.printStackTrace();
		} 
		catch (IOException e) 
		{
			e.printStackTrace();
		}
    	finally
    	{
    		try 
    		{
				socket.close();
				p.close();
				b.close();
			} 
    		catch (IOException e) 
    		{
				e.printStackTrace();
			}
    	}
    	return tmp;
    }
    
    @Override
    public boolean onCreateOptionsMenu(Menu menu) 
    {
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    
}

/*
 * 
 */