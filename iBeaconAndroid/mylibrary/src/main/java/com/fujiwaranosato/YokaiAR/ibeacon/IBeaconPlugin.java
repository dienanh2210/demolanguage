package com.fujiwaranosato.YokaiAR.ibeacon;

import android.content.Context;
import android.content.Intent;

import com.unity3d.player.UnityPlayer;

import org.altbeacon.beacon.BeaconManager;
import org.altbeacon.beacon.BeaconParser;
import org.altbeacon.beacon.service.BeaconService;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;


public class IBeaconPlugin {

    private Context context;
    private static IBeaconPlugin instance;
    public static String title;
    private String detail;
    public static boolean notBackground;
    public static List<IbeaconStruct> listIbeacon;

    public static IBeaconPlugin instance() {

        return instance;
    }

    public static void SetInstance() {

        instance = new IBeaconPlugin();

    }

    public void setContext(Context ctx) {
        context = ctx;

    }

    public Context getContext() {
        return context;
    }

    public String getDetail() {
        return detail;
    }

    public void turnOnService(String json, String title, String detail) {

        changeJsonValue(json, title);
        title = title;
        this.detail = detail;
        Intent i = new Intent(context, com.fujiwaranosato.YokaiAR.ibeacon.IBeaconService.class);
        i.putExtra("json", json);
        context.startService(i);
    }

    public void changeJsonValue(String json, String title) {
        this.title = title;
        listIbeacon = new ArrayList<>();
        try {
            JSONObject jsonObj = new JSONObject(json);
            JSONArray jsonArray = jsonObj.getJSONArray("iBeacons");
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject ib = jsonArray.getJSONObject(i);
                listIbeacon.add(new IbeaconStruct(ib.getString("minorId"), ib.getString("uuId"), ib.getString("majorId"), ib.getString("timeLastShown")));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void isRunningBackground() {
        notBackground = false;
    }

    public void isNotRunningBackground() {
        notBackground = true;
    }

    public void turnOffService() {
        context.stopService(new Intent(context, com.fujiwaranosato.YokaiAR.ibeacon.IBeaconService.class));
    }

    public static void sendMessageToUnity(String ibeacon) {
        UnityPlayer.UnitySendMessage("Main Camera", "ReceiveMessage", ibeacon);
    }
}
