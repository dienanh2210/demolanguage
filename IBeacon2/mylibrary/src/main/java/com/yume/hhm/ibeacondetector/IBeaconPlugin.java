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

    public void setContext(Context ctx){
        context = ctx;
    }

    public Context getContext(){
        return context;
    }

    public void turnOnService(String json) {
        Intent i = new Intent(context, com.yume.hhm.ibeacondetector.IBeaconService.class);
        i.putExtra("json", json);
        context.startService(i);
    }

    public void turnOffService() {
        context.stopService(new Intent(context, com.yume.hhm.ibeacondetector.IBeaconService.class));
    }
}
