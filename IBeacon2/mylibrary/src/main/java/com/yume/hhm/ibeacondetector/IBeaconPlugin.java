package com.yume.hhm.ibeacondetector;

import android.content.Context;
import android.content.Intent;
import android.widget.Toast;


public class IBeaconPlugin {

    private Context context;
    private static IBeaconPlugin instance;

    public static IBeaconPlugin instance() {
        if(instance == null) {
            instance = new IBeaconPlugin();
        }
        return instance;
    }

    public void setContext(Context context){
        this.context = context;
    }

    public Context getContext(){
        return context;
    }

    public void turnOnService(String json) {
        Toast.makeText(context,"on service",Toast.LENGTH_LONG).show();
        Intent i = new Intent(context, com.yume.hhm.ibeacondetector.IBeaconService.class);
        i.putExtra("json", json);
        context.startService(i);
    }

    public void turnOffService() {
        Toast.makeText(context,"off service",Toast.LENGTH_LONG).show();
        context.stopService(new Intent(context, com.yume.hhm.ibeacondetector.IBeaconService.class));
    }
}
