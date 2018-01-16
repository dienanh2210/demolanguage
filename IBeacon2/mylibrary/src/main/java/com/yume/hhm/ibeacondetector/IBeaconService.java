package com.yume.hhm.ibeacondetector;

import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.os.IBinder;
import android.os.RemoteException;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;

import com.unity3d.player.UnityPlayerActivity;

import org.altbeacon.beacon.Beacon;
import org.altbeacon.beacon.BeaconConsumer;
import org.altbeacon.beacon.BeaconManager;
import org.altbeacon.beacon.BeaconParser;
import org.altbeacon.beacon.Identifier;
import org.altbeacon.beacon.RangeNotifier;
import org.altbeacon.beacon.Region;
import org.altbeacon.beacon.startup.BootstrapNotifier;
import org.altbeacon.beacon.startup.RegionBootstrap;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;


public class IBeaconService extends Service implements BootstrapNotifier, BeaconConsumer {
    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    private BeaconManager beaconManager;
    private Identifier identifier;
    private Region region;
    private String beaconName;
    private RegionBootstrap regionBootstrap;

    private NotificationCompat.Builder mBuilder;
    private int mNotificationId = 001;
    private NotificationManager mNotifyMgr;

    private Context context;

    private List<String> listMinor;

    public void onCreate() {
        super.onCreate();

        context = IBeaconPlugin.instance().getContext();

        PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, new Intent(context, UnityPlayerActivity.class), PendingIntent.FLAG_UPDATE_CURRENT);
        int id = getResources().getIdentifier("noti", "drawable", "com.yume.yokaiget");

        mNotifyMgr = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
        mBuilder = new NotificationCompat.Builder(context)
                .setSmallIcon(id)
                .setContentTitle("iBeacon detected!!!")
                .setContentText("Tap to show!!!")
                .setContentIntent(pendingIntent);

        beaconManager = BeaconManager.getInstanceForApplication(getApplicationContext());
        beaconManager.getBeaconParsers().add(new BeaconParser().setBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24"));
        beaconManager.setBackgroundBetweenScanPeriod(1000);
        identifier = Identifier.parse("B0FC4601-14A6-43A1-ABCD-CB9CFDDB4013");
        beaconName = "Yokai_get_ibeacon";
        region = new Region(beaconName, identifier, null, null);
    }


    @Override
    public void onDestroy(){
        super.onDestroy();
        beaconManager.unbind(this);
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        regionBootstrap = new RegionBootstrap(this, region);
        beaconManager.bind(this);
        if (intent != null && intent.getExtras() != null){
            String json = intent.getStringExtra("json");
            parseJSON(json);
        }
        return START_NOT_STICKY;
    }

    private void parseJSON(String json){
        listMinor = new ArrayList<>();
        try {
            JSONObject jsonObj = new JSONObject(json);
            JSONArray jsonArray = jsonObj.getJSONArray("iBeacons");
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject ib = jsonArray.getJSONObject(i);
                listMinor.add(ib.getString("minor_id"));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void didEnterRegion(Region region) {
        try {
            beaconManager.startRangingBeaconsInRegion(region);
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void didExitRegion(Region region) {
        try {
            beaconManager.stopRangingBeaconsInRegion(region);
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void didDetermineStateForRegion(int i, Region region) {

    }

    @Override
    public void onBeaconServiceConnect() {
        beaconManager.addRangeNotifier(new RangeNotifier() {
            @Override
            public void didRangeBeaconsInRegion(Collection<Beacon> collection, Region region) {
                if (collection.size() > 0) {
                    for (Beacon b : collection) {
                        for(String minor : listMinor) {
                            if(b.getId3().toString().equals(minor)) {
                                mNotifyMgr.notify(mNotificationId, mBuilder.build());
                            }
                        }
                    }
                }
            }
        });
    }
}
