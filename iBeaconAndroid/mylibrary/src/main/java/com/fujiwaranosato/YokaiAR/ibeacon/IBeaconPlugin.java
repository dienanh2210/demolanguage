package com.fujiwaranosato.YokaiAR.ibeacon;

import android.content.Context;
import android.content.Intent;


public class IBeaconPlugin {

    private Context context;
    private static IBeaconPlugin instance;
    private String title;
    private String detail;

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

    public String getTitle() {
        return title;
    }

    public String getDetail() {
        return detail;
    }

    public void turnOnService(String json, String title, String detail) {
        this.title = title;
        this.detail = detail;
        Intent i = new Intent(context, com.fujiwaranosato.YokaiAR.ibeacon.IBeaconService.class);
        i.putExtra("json", json);
        context.startService(i);
    }

    public void turnOffService() {
        context.stopService(new Intent(context, com.fujiwaranosato.YokaiAR.ibeacon.IBeaconService.class));
    }
}
