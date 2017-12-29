package com.yume.hhm.mylibrary;

import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;


public class iBeaconLib {

    private Context context;
    private static iBeaconLib instance;

    public static iBeaconLib instance() {
        if(instance == null) {
            instance = new iBeaconLib();
        }
        return instance;
    }

    public void setContext(Context context){
        this.context = context;
    }

    public String turnOnService() {
        Toast.makeText(context,"test",Toast.LENGTH_LONG).show();
        context.startService(new Intent(context, com.yume.hhm.mylibrary.iBeaconService.class));
        return "test after service";
    }
}
